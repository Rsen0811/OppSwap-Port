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
    games[gameId].clients.forEach(id => {
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
    clients[clientId] = {
        "connection": connection,
        "status": "open"
    }
    connections[connection] = clientId;

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
        "clients": games[gameId].clients
    }
    const package = { "method": "playerJoinUpdate", "payload": JSON.stringify(payLoad) }

    games[gameId].clients.forEach(client => { 
        //if(client[client]!=null){ //if the connection is null because fake data with null connection
        if (clients[client].status === "open") { // if the connection is status closed, then dont try sending message
                clients[client].connection.send(JSON.stringify(package));
        } 
        //}   
    });
}

function createNewGame(connection, incoming) {
    gameId = guid()
    games[gameId] = {
        "name": incoming.value,
        "id": gameId,
        "clients": [],
        "positions": {}
    }

    console.log("Game " + gameId + " created by userId: " + incoming.clientId)
    //joinGame(connection, gameId, incoming.clientId);
}

function joinGame(connection, gameId, clientId) {
    game = games[gameId]

    game.clients.push(clientId)
    game.positions[clientId] = "0, 0";
    const payLoad = {
        "gameName": game.name,
        "gameId": gameId
    }
    console.log("userId: "+clientId+" has joined game: "+gameId);
    const package = { "method": "forceJoin", "payload": JSON.stringify(payLoad) }
    //if(connection!==null){ //if the connection is null because fake data with null connection
    connection.send(JSON.stringify(package));
    //}
    playerJoinUpdate(gameId);
}

function fetchGames(connection, query) {
    gameNames = []
    gameIds = []
    Object.keys(games).forEach(gameKey => { // gamekey is the gameId, but i decided not to use the same var name
        game = games[gameKey]
        if (game.name.includes(query)) {
            gameNames.push(game.name);
            gameIds.push(game.id);
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
    gamesJoined.forEach(element => {
        game = games[element.Id]
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
