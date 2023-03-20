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
    })

})

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
        if (clients[client].status === "open") { // if the connection is status closed, then dont try sending message
            clients[client].connection.send(JSON.stringify(package));
        }   
    });
}

function createNewGame(connection, incoming) {
    gameId = guid()
    games[gameId] = {
        "name": incoming.value,
        "id": gameId,
        "clients": []
    }

    console.log("Game " + gameId + " created by userId: " + incoming.clientId)
    joinGame(connection, gameId, incoming.clientId);
}

function joinGame(connection, gameId, clientId) {
    game = games[gameId]

    game.clients.push(clientId)
    const payLoad = {
        "gameName": game.name,
        "gameId": gameId
    }

    const package = { "method": "forceJoin", "payload": JSON.stringify(payLoad) }
    connection.send(JSON.stringify(package));
    playerJoinUpdate(gameId);
}



// GUID generator 
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
const guid = () => (S4() + S4() + "-" + S4() + "-4" + S4().substring(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();