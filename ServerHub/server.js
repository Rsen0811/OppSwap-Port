// JavaScript source code
const http = require("http");

const websocketServer = require("websocket").server;
const httpServer = http.createServer();
httpServer.listen(9992, () =>//process.env.PORT
  console.log("BEEP BOOP, COMPUTER NOISES ON 9092")
);

// hashmap of all clients
const clients = {};
const connections = {}; // reverse of hashmap
// off all games
const games = {};
const pings = {};

const wsServer = new websocketServer({
  httpServer: httpServer,
});

wsServer.on("request", (request) => {
  const connection = request.accept(null, request.origin);
  connection.on("open", () => console.log("Seasame has been opened!"));
  connection.on("close", () => {
    console.log("Leaving so soon?");
    // finds the entire client that has been disconnected, by looking at reversed object, sets status to closed
    clients[connections[connection]].status = "closed";
    delete connections[connection];
  });

  connection.on("message", (message) => {
    const incoming = JSON.parse(message.utf8Data);
    if (incoming.method === "connect") connect(connection);
    else if (incoming.method === "ping") ping(connection);
    else if (incoming.method === "createNewGame") createNewGame(connection, incoming);
    else if (incoming.method === "joinGame") joinGame(connection, incoming.gameId, incoming.clientId);
    else if (incoming.method === "fetchGames") fetchGames(connection, incoming.clientId, incoming.query);
    else if (incoming.method === "updatePosition") updatePosition(incoming.clientId, incoming.position);
    else if (incoming.method === "getTargetPosition") getTargetPosition(connection, incoming.gameId, incoming.clientId);
    else if (incoming.method === "startGame") startGame(connection, incoming.gameId, incoming.clientId);
    else if (incoming.method === "reconnect") reconnect(connection, incoming.clientId, incoming.oldId);
    else if (incoming.method === "kill") kill(connection, incoming.gameId, incoming.clientId);
    else if (incoming.method === "setName") setName(connection, incoming.clientId, incoming.name);
  });
});

function setName(connection, clientId, name) {
  let client = clients[clientId];
  client.name = name;

  const payLoad = {
    clientId: clientId,
    name: name,
    gamesJoined: client.currentGames
  }
  const package = {method:"nickName", payload:JSON.stringify(payLoad)}

  Object.keys(clients).forEach((client) => {
    if (connectionOpen(client)) {
      // if the connection is status closed, then dont try sending message
      clients[client].connection.send(JSON.stringify(package));
    }
  })
  // send client message that name properly changed using connection
}

function kill(connection, gameId, clientId) {
  let game = games[gameId];
  let target = game.targets.getTarget(clientId);
  game.targets.removeNode(target);
  // now use connection to send server message that the kill was successfull
  let newTarget = game.targets.getTarget(clientId);
  const payLoad = { 
    gameId: gameId,
    targetId: newTarget,
    targetName: clients[newTarget].name

    //TODO add target nickname when we implement those
  }
  const package = {method:"newTarget", payload:JSON.stringify(payLoad)}
  connection.send(JSON.stringify(package))
  updateDeath(gameId, target)

  if (game.targets.getTarget(clientId) === clientId) {
    winnerBroadcast(gameId, clientId);
  }
}

function winnerBroadcast(gameId, winnerId) {
  let game = games[gameId];
  const payLoad2 = {
    gameId: gameId,
    winnerName: clients[winnerId].name, 
    winnerId: winnerId
  }

  const package2 = {method:"winner", payload:JSON.stringify(payLoad2)}
  game.clientIds.forEach((client) => {
    if (connectionOpen(client)) {
      // if the connection is status closed, then dont try sending message
      clients[client].connection.send(JSON.stringify(package2));
    }
  });
}

function updateDeath(gameId, playerId) {
  let game = games[gameId];
  const payLoad = {
    gameId: gameId,
    playerId: playerId
  }
  const package = {method:"deathUpdatePayload", payload:JSON.stringify(payLoad)}

  game.clientIds.forEach((client) => {
    if (connectionOpen(client)) {
      // if the connection is status closed, then dont try sending message
      clients[client].connection.send(JSON.stringify(package));
    }
  });
}

function reconnect(connection, clientId, oldId) { // right now just use clientId for debug
  let client = clients[oldId];
  if (client === undefined) return;

  if (connections[client.connection] != null) delete connections[client.connection];
  delete clients[clientId];

  client.connection = connection;
  client.status = "open";
  client.currentGames.forEach(gameId => {
    //rejoin games
    let game = games[gameId]
    const payLoad = {
      gameName: game.gameName,
      gameId: game.gameId,
    };
    console.log("userId: " + clientId + " has rejoined game: " + game.gameId);
    const package = { method: "forceJoin", payload: JSON.stringify(payLoad) };
    connection.send(JSON.stringify(package))
    
    if (game.visibility === false) { // now tell client that some rooms are started
      let targetId = game.targets.getTarget(oldId);
      const payload2 = {
        //TODO eventually change this to nickname
        targetId: targetId,
        targetName: clients[targetId].name,
        gameId: game.gameId
      };
      const package2 = {
        method: "gameStarted",
        payload: JSON.stringify(payload2),
      };
      connection.send(JSON.stringify(package));
    }
  })
}

function getTargetPosition(connection, gameId, clientId) {
  const game = games[gameId];
  if (game.visibility === true) {return} //TODO check for if this actually works to TODO rename variable to visible, so i can stop using === true
  const targetId = game.targets.getTarget(clientId);
  if (clients === undefined || clients[targetId] === undefined) {
    return;
  }
  const targetPos = clients[targetId].position;
  
  const payLoad = {
    targetPosition: targetPos,
    gameId: gameId
  };
  
  const package = { method: "getPosition", payload: JSON.stringify(payLoad) };
  connection.send(JSON.stringify(package));
}

function connect(connection) {
  pings[connection] = 0;

  const clientId = guid();
  clients[clientId] = new Client(connection);
  connections[connection] = clientId; // gets client id from connection

  const payLoad = {
    clientId: clientId,
  };
  const package = { method: "connect", payload: JSON.stringify(payLoad) };
  // send back the client connect
  connection.send(JSON.stringify(package));
}

function ping(connection) {
  console.log("Suceessful PING: ---");
  console.log("pingnumber: " + ++pings[connection]); // proves that connections are not kept over different client sessions
  console.log("Connection: " + connection.remoteAddress);
  sendServerMessage(connection, "Successful Ping");
}

function playerJoinUpdate(gameId) {
  const payLoad = {
    gameId: gameId,
    clientIds: games[gameId].clientIds,
    clientNames: games[gameId].clientNames
  };
  const package = {
    method: "playerJoinUpdate",
    payload: JSON.stringify(payLoad),
  };

  games[gameId].clientIds.forEach((client) => {
    //recomment
    if (connectionOpen(client)) {
      // if the connection is status closed, then dont try sending message
      clients[client].connection.send(JSON.stringify(package));
    }
    //}
  });
}

function connectionOpen(clientId) {
  //TODO comment this
  //return true;
  return clients[clientId].status === "open";
}

function createNewGame(connection, incoming) {
  const incomingName = incoming.value;
  const game = new Room(incomingName);
  games[game.gameId] = game;

  console.log(
    "Game " + game.gameId + " created by userId: " + incoming.clientId
  );
  joinGame(connection, game.gameId, incoming.clientId);
}

function joinGame(connection, gameId, clientId) {
  const game = games[gameId];
  clients[clientId].currentGames.push(gameId);

  if (game.visibility) {
    game.addPlayer(clientId);
    const payLoad = {
      gameName: game.gameName,
      gameId: game.gameId,
    };
    console.log("userId: " + clientId + " has joined game: " + game.gameId);
    const package = { method: "forceJoin", payload: JSON.stringify(payLoad) };
    //TODO comment this
    //if (connection !== null)
      //if the connection is null because fake data with null connection
      connection.send(JSON.stringify(package));
    playerJoinUpdate(game.gameId);
  } else {
    //send a message that says "could not join game because it is closed"
    sendServerMessage(connection, "Could not join game because it is closed");
  }
}

function fetchGames(connection, clientId, query) {
  let gameNames = [];
  let gameIds = [];
  let client = clients[clientId]

  let clientConnected = client.currentGames;
  Object.keys(games).forEach((gameKey) => {
    // gamekey is the gameId, but i decided not to use the same var name
    const game = games[gameKey];
    if (!clientConnected.includes(gameKey) && game.visibility && 
              (query === "" || game.gameName.includes(query) || game.gameId.includes(query))) {
      gameNames.push(game.gameName);
      gameIds.push(game.gameId);
    }
  });

  const payLoad = {
    names: gameNames,
    ids: gameIds,
  };

  const package = { method: "fetchGames", payload: JSON.stringify(payLoad) };
  connection.send(JSON.stringify(package));
  if (gameIds.length == 0) {
    sendServerMessage(connection,"We searched and searched but someone gave you the wrong name... R.I.P.");
  }
}

function updatePosition(clientId, position) {
  clients[clientId].position = position;
  console.log(
    "client: " + clientId + "'s position has been updated to " + position
  );
}

function startGame(connection, gameId, clientId) { // dont start the game if the game shouldnt be started
  let game = games[gameId];
  if (game.clientIds.length < 2){
    sendServerMessage(connection, "This game requires friends. Please add one more person to the room before starting.");
    return;
  }
  //set the game's visibility to false

  if (game.visibility) {
    //TODO add check for 2 or more clients
    game.visibility = false;
    //create a linked list
    game.targets = new LinkedList(game.clientIds);
    //send everybody their target && a message that says gameStarted
    game.clientIds.forEach((element) => {
      if (clients[element].status === "open") {
        let currConn = clients[element].connection;
        //creating a payload with the oppenents
        let targetId = game.targets.getTarget(element);
        const payload = {
          //TODO eventually change this to nickname
          targetId: targetId,
          targetName: clients[targetId].name,
          gameId: gameId
        };
        const package = {
          method: "gameStarted",
          payload: JSON.stringify(payload),
        };
        currConn.send(JSON.stringify(package));
      }
    });
  } else {
    //add code that sends a game has already started
    sendServerMessage(connection, "The game has already started");
  }
}

// GUID generator
function S4() {
  return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
const guid = () => (S4() + S4() + "-" + S4() + "-4" + S4().substring(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();



//make sure to uncomment the if statements when debugged
function runDebug() { //TODO move this to the bottom
  gameId = "";
  clientId = "";

  clients["imcracked"] = null;
  incoming = {
    value: "coolRoom",
    clientId: "imcracked",
  };
  createNewGame(null, incoming);

  Object.keys(games).forEach((gameKey) => {
    gameId = gameKey;
  });
  Object.keys(clients).forEach((clientKey) => {
    clientId = clientKey;
  });

  for (let i = 0; i < 5; i++) {}
  joinGame(null, gameId, clientId);

  clients[clientId].position = "10, 4";
}
//runDebug();
//===============================================================================
class Room {
  constructor(name) {
    this.gameName = name;
    this.gameId = guid();
    this.clientIds = [];
    this.clientNames = [];
    this.targets = null;
    this.settings = null;
    this.paused = false;
    this.visibility = true;
    this.targets = null;
  }
  addPlayer(playerId) {
    this.clientIds.push(playerId);
    this.clientNames.push(clients[playerId].name)
    //this.positions[playerId] = "0,0";
  }
}

class Client {
  constructor(connection) {
    this.name = "guest",
    this.connection = connection;
    this.status = "open";
    this.currentGames = []; // ================================================ TODO
    this.position = "0,0";
  }
}
//==============================================================================
class LinkedList {
  /**
   * Creates a new linked list with the given array of client IDs.
   * The client IDs are randomly shuffled before creating the linked list.
   * @param {Array<string>} clientIDs - The array of client IDs to use.
   */
  constructor(clientIDs) {
    // Create a new map to store the linked list
    // Shuffle the array of client IDs
    this.map = new Map();
    const shuffledIDs = this.shuffleArray(clientIDs);
    // Create the linked list from the shuffled client IDs
    if (shuffledIDs.length > 0) {
      for (var i = 0; i < shuffledIDs.length - 1; i++) {
        this.map.set(shuffledIDs[i], shuffledIDs[i + 1]);
      }
      // Make the last client ID point to the first one to make the list circular
      this.map.set(shuffledIDs[shuffledIDs.length - 1], shuffledIDs[0]);
    }
  }

  /**
   * Removes the node with the given client ID from the linked list.
   * @param {string} clientID - The client ID to remove.
   */
  removeNode(clientID) {
    if (this.map.has(clientID)) {
      var prevID = "";
      for (var id of this.map.keys()) {
        if (this.map.get(id) === clientID) {
          prevID = id;
          break;
        }
      }
      var nextID = this.map.get(clientID);
      this.map.set(prevID, nextID);
      this.map.delete(clientID);
    }
  }

  /**
   * Returns the target client ID of the node with the given client ID.
   * @param {string} clientID - The client ID to get the target for.
   * @returns {string|null} The target client ID or null if the node is not found.
   */
  getTarget(clientID) {
    return this.map.get(clientID) || null;
  }

  /**
   * Shuffles the given array of strings using the Fisher-Yates shuffle algorithm.
   * @param {Array<string>} array - The array of strings to shuffle.
   * @returns {Array<string>} A shuffled copy of the input array.
   */
  shuffleArray(array) {
    for (var i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
  }
  toString(start) {
    var ans = start;
    var currNode = this.map.get(start);
    while (currNode != start) {
      console.log();
      ans += "-->" + currNode;
      currNode = this.map.get(currNode);
    }
    return ans + "-->" + start;
  }
}
function sendServerMessage(connection, message) {
    const payLoad = {
        message: message
    };

    const package = { method: "serverMessage", payload: JSON.stringify(payLoad) };
    connection.send(JSON.stringify(package));
  }
