const connection = new signalR.HubConnectionBuilder().withUrl("/Reviews").build();

connection.start().then(function () {
    console.log("Connection started successfully.");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveReview", function (username, comment, rating) {
    const li = document.createElement("li");
    document.getElementById("reviewsList").appendChild(li);
    li.textContent = `${username}: ${comment} (Rating: ${rating})`;
});





