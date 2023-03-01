// JavaScript source code
const http = require("http");

const websocketServer = require("websocket").server
const httpServer = http.createServer();
httpServer.listen(9090, () => console.log("BEEP BOOP, COMPUTER NOISES ON 9090"))

// hashmap of all clients
const clients = {};
// off all games
const games = {};

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
    })

})

function connect(connection) {
    const clientId = guid();
    clients[clientId] = {
        "connection": connection
    }

    const payLoad = {
        "method": "connect",
        "clientId": clientId
    }

    // send back the client connect
    connection.send(JSON.stringify(payLoad));
}

// GUID generator 
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
const guid = () => (S4() + S4() + "-" + S4() + "-4" + S4().substring(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();