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


document.getElementById("submitReview").addEventListener("click", function (event) {



    const username = document.getElementById("Username").value;
    const comment = document.getElementById("Comment").value.trim();
    const rating = document.querySelector('input[name="rating"]:checked');

    if (!comment || !rating) {
        alert("Please provide a comment and select a rating.");
        return;
    }

    connection.invoke("SendReview", username, comment, rating.value)
        .then(() => console.log("Review sent successfully"))
        .catch(function (err) {
            return console.error(err.toString());
        });



    const review = {
        username: username,
        comment: comment,
        rating: rating.value,
        productId: document.getElementById("productId").value
    };

    fetch('/Product/AddReview', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(review)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log('Review submitted to controller:', data.message);
                // Clear input fields
                document.getElementById("Comment").value = '';
                document.querySelector('input[name="rating"]:checked').checked = false;

            } else {
                console.error('Error from controller:', data.message);
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });

});