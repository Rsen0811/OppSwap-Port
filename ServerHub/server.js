// JavaScript source code
const http = require("http");

const websocketServer = require("websocket").server
const httpServer = http.createServer();
httpServer.listen(9992, () => console.log("BEEP BOOP, COMPUTER NOISES ON 9092"))

// hashmap of all clients
const clients = {};
const connections = {}; // reverse of hashmap
// off all games
const games = {};
const pings = {};

const wsServer = new websocketServer({
    "httpServer": httpServer
})


wsServer.on("request", request => {
    const connection = request.accept(null, request.origin);
    connection.on("open", () => console.log("Seasame has been opened!"));
    connection.on("close", () => {
        console.log("Leaving so soon?");
        // finds the entire client that has been disconnected, by looking at reversed object, sets status to closed
        clients[connections[connection]].status = "closed";
        delete connections[connection];
    });
    
    connection.on("message", message => {
        const incoming = JSON.parse(message.utf8Data)
        if (incoming.method === "connect") connect(connection);
        if (incoming.method === "ping") ping(connection);
        if (incoming.method === "createNewGame") createNewGame(connection, incoming);
        if (incoming.method === "joinGame") joinGame(connection, incoming.gameId, incoming.clientId);
        if (incoming.method === "fetchGames") fetchGames(connection, incoming.query);
        if (incoming.method === "updatePosition") updatePosition(incoming.gamesJoined, incoming.clientId, incoming.position);
        if (incoming.method === "TP") TP(connection, incoming.gameId, incoming.clientId)
    })

})
function TP(connection, gameId, clientId) { //#=============== fakecode
    games[gameId].clientIds.forEach(id => {
        if (id != clientId) {
            const packet = {
                "method": "TP",
                "payload": games[gameId].positions[id]
            }
            connection.send(JSON.stringify(packet));
            return;
        }
    })
}
function connect(connection) {
    pings[connection] = 0;

    const clientId = guid();
    clients[clientId] = new Client(connection);    
    connections[connection] = clientId; // gets client id from connection

    const payLoad = {
        "clientId": clientId
    }
    const package = { "method": "connect", "payload": JSON.stringify(payLoad) }
    // send back the client connect
    connection.send(JSON.stringify(package));
} 

function ping(connection) {
    console.log("Suceessful PING: ---")
    console.log("pingnumber: " + ++pings[connection]) // proves that connections are not kept over different client sessions
    console.log("Connection: " + connection.remoteAddress)
}

function playerJoinUpdate(gameId) {
    const payLoad = {
        "gameId" : gameId,
        "clients": games[gameId].clientIds
    }
    const package = { "method": "playerJoinUpdate", "payload": JSON.stringify(payLoad) }

    games[gameId].clientIds.forEach(client => { 
        //if(client[client]!=null){ //if the connection is null because fake data with null connection
        if (clients[client].status === "open") { // if the connection is status closed, then dont try sending message
                clients[client].connection.send(JSON.stringify(package));
        } 
        //}   
    });
}

function createNewGame(connection, incoming) {
    const incomingName = incoming.value;
    const room = new Room(incomingName);
    games[room.gameId] = room

    console.log("Game " + room.gameId + " created by userId: " + incoming.clientId)
    //joinGame(connection, gameId, incoming.clientId);
}

function joinGame(connection, gameId, clientId) {
    const game = games[gameId]

    game.addPlayer(clientId)
    const payLoad = {
        "gameName": game.gameName,
        "gameId": game.gameId
    }
    console.log("userId: "+clientId+" has joined game: "+game.gameId);
    const package = { "method": "forceJoin", "payload": JSON.stringify(payLoad) }
    //if(connection!==null){ //if the connection is null because fake data with null connection
    connection.send(JSON.stringify(package));
    //}
    playerJoinUpdate(game.gameId);
}

function fetchGames(connection, query) {
    let gameNames = []
    let gameIds = []
    Object.keys(games).forEach(gameKey => { // gamekey is the gameId, but i decided not to use the same var name
        const game = games[gameKey]
        if (game.gameName.includes(query)) {
            gameNames.push(game.gameName);
            gameIds.push(game.gameId);
        }
    });

    payLoad = {
        "names": gameNames,
        "ids": gameIds
    }
    
    const package = { "method": "fetchGames", "payload": JSON.stringify(payLoad) }
    connection.send(JSON.stringify(package));
}

function updatePosition(gamesJoined, clientId, position) {
    gamesJoined.forEach(element => { // each element is an entire room, the id is a feild of that object
        let game = games[element.Id] // apparently the entire game is sent when i send all the games, so it makes sense that i am specifically looking for the id of each game
        game.positions[clientId] = position;
    })
    console.log("client: "+clientId+"'s position has been updated to "+position);
}


// GUID generator 
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
const guid = () => (S4() + S4() + "-" + S4() + "-4" + S4().substring(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
/* make sure to uncomment the if statements when debugged
function runDebug() {
    gameId="";
    clientId="";

    clients["imcracked"]=null;
    incoming={
    "value" : "coolRoom",
    "clientId" : "imcracked"
    };
    createNewGame(null,incoming)

    Object.keys(games).forEach(gameKey => {gameId=gameKey})
    Object.keys(clients).forEach(clientKey => {clientId=clientKey})
    joinGame(null,gameId,clientId);

    games[gameId].positions[clientId] = "10, 4";
}*/
//runDebug();
//===============================================================================
class Room {
    constructor(name) {
        this.gameName = name;
        this.gameId = guid();
        this.clientIds = [];
        this.targets = null;
        this.positions = {};
        this.settings = null;
        this.paused = false;
    }

    postPos(playerId, position) {
        positions[playerId] = position;
    }
    fetchPos(playerId) {
        return positions[playerId];
    }
    addPlayer(playerId) {
        this.clientIds.push(playerId);
        this.positions[playerId] = "0,0";
    }
}

class Client {
    constructor(connection) {
        this.connection = connection;
        this.status = "open";
    }
}
//==============================================================================
class LinkedList {
    /**
     * Creates a new linked list with the given array of client IDs.
     * The client IDs are randomly shuffled before creating the linked list.
     * @param {Array<string>} clientIDs - The array of client IDs to use.
     */
    LinkedList(clientIDs) {
      // Create a new map to store the linked list  
      // Shuffle the array of client IDs
      map=new Map();
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
    toString(start){
        var ans=start;
        var currNode=this.map.get(start);
        while(currNode!=start){
            console.log()
            ans+="-->"+currNode;
            currNode=this.map.get(currNode);
        }
        return ans+"-->"+start;
    }
  }