//Author: Matthew Washburn
//Version: Fall 2025

//server url for all fetch requests
const serverUrl = "https://localhost:7210/api";

//get and display the full list of favorite characters
const startWorkPost = async () => {
    //Get the work count
    let workCount = parseInt(document.getElementById("workCount").value);

    const response = await fetch(serverUrl + "/overseer/", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(workCount)
    });

    if (response.ok) {
        const data = await response.json();
        document.getElementById("results").innerHTML = await data.message;
    } else {
        const data = await response.json();
        document.getElementById("results").innerHTML = "Error: Invalid Request!";
    }
}

//Reset and start mario movement on button click
window.onload = function () {
    document.getElementById("startButton").onclick = function () {
        startWorkPost();
    }
}