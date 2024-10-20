const connection = new signalR.HubConnectionBuilder().withUrl("/Reviews").build();

connection.on("ReceiveReview", function (username, comment, rating) {
    const div = document.createElement("div");
    const h4 = document.createElement("h4");
    const div2 = document.createElement("div");
    const p = document.createElement("p");

    div.classList.add("review");
    h4.textContent = username;
    div2.classList.add("star-rating");
    p.textContent = comment;

 
    div.appendChild(h4);
    div.appendChild(div2);
    div.appendChild(p);

    for (let i = 1; i <= 5; i++) {
        const span = document.createElement("span");
        span.classList.add("star");
        span.textContent = "★";
        if (i <= rating) {
            span.classList.add("filled");
        }
        div2.appendChild(span);
    }

    const container = document.getElementById("reviewsList");
    container.appendChild(div);
});


const submit = document.getElementById("submitReview");

if (submit !== null) {
    document.getElementById("submitReview").addEventListener("click", function (event) {



        const username = document.getElementById("Username").value;
        const comment = document.getElementById("Comment").value.trim();
        const rating = document.querySelector('input[name="rating"]:checked');
        const productId = document.getElementById("productId").value
        const userId = document.getElementById("userId").value



        if (!comment || !rating) {
            alert("Please provide a comment and select a rating.");
            return;
        }

        const review = {
            username: username,
            comment: comment,
            rating: rating.value,
            productId: productId,
            userId: userId
        };

        let jsonReview = JSON.stringify(review)

        connection.send("SendReview", jsonReview)
            .then(() => console.log("Review sent successfully"))
            .catch(function (err) {
                return console.error(err.toString());
            });



    });

}

connection.start().then(function () {
    console.log("Connection started successfully.");
}).catch(function (err) {
    return console.error(err.toString());
});
