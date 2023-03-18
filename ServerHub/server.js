// JavaScript source code
const http = require("http");

const websocketServer = require("websocket").server
const httpServer = http.createServer();
httpServer.listen(9792, () => console.log("BEEP BOOP, COMPUTER NOISES ON 9092"))

// hashmap of all clients
const clients = {};
// off all games
const games = {};
const pings = {};

const wsServer = new websocketServer({
    "httpServer": httpServer
})

wsServer.on("request", request => {
    const connection = request.accept(null, request.origin);
    connection.on("open", () => console.log("Seasame has been opened!"));
    connection.on("close", () => console.log("Leaving so soon?"));
    
    connection.on("message", message => {
        const incoming = JSON.parse(message.utf8Data)
        if (incoming.method === "connect") connect(connection);
        if (incoming.method === "ping") ping(connection);
        if (incoming.method === "createNewGame") createNewGame(connection, incoming);
    })

})

function connect(connection) {
    pings[connection] = 0;

    const clientId = guid();
    clients[clientId] = {
        "connection": connection
    }

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

function createNewGame(connection, incoming) {
    gameId = guid()
    games[gameId] = {
        "name": incoming.value,
        "id": gameId,
        "clients": [incoming.clientId]
    }

    const payLoad = {
        "gameName": incoming.value,
        "gameId": gameId
    }
    const package = { "method": "forceJoin", "payload": JSON.stringify(payLoad) }
    connection.send(JSON.stringify(package));
}

// GUID generator 
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
const guid = () => (S4() + S4() + "-" + S4() + "-4" + S4().substring(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();