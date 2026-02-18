//Author: Matthew Washburn
//Version: Fall 2025

//Server url for all fetch requests
const serverUrl = "https://localhost:7155/api";

//Helper function to display results in the "results" div
const simpleResponse = (responseJson) => {
    document.getElementById("results").innerHTML = responseJson;
}


//Attempt to log in with inputted username and password
const logIn = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    fetch(serverUrl + "/login/", {
        method: "POST",
        body: JSON.stringify({
            UserName: document.getElementById("userName").value || null,
            Password: document.getElementById("password").value || null,
        }),
        headers: {
            "Content-Type": "application/json",
        }
    }).then(async (response) => {
        const text = await response.text();  // Plain text

        if (!response.ok) {
            throw new Error(text || "Login failed");
        }

        return text;
    }).then(responseJson => {
        localStorage.setItem("authToken", responseJson)
        simpleResponse("Log In Success!");
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//Log out by removing local browser storage of authentication token
const logOut = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("results").innerHTML = "";
    localStorage.removeItem("authToken");
    document.getElementById("results").innerHTML = "Log Out Success!";
}

//Get a random quote from glados
const getQuote = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    //Get login token
    var authToken = localStorage.getItem("authToken")

    //Will only return a quote if the user has a valid token
    fetch(serverUrl + "/glados/", {
        headers: {
            "Authorization": `Bearer ${authToken}`,
            "Content-Type": "application/json",
        }
    }).then(async (response) => {
        const text = await response.text();  // Plain text

        if (!response.ok) {
            throw new Error("Unauthorized. Please Log In.");
        }

        return text;
    }).then(responseJson => {
        document.getElementById("results").innerHTML = responseJson;
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//Hook up all buttons to their corresponding functions when the page loads
window.onload = function () {
    document.getElementById("logIn").onclick = logIn;
    document.getElementById("logOut").onclick = logOut;
    document.getElementById("getQuote").onclick = getQuote;
}