//Author: Matthew Washburn
//Version: Fall 2025

//array to store the last index that was force read
let lastForceReadArray = [-1];
//flag to check if the last action was a force read
let justForceRead = false;
//server url for all fetch requests
const serverUrl = "https://bethelwebrequestsserver.azurewebsites.net";

//helper function to display results in the "results" div
const simpleResponse = (responseJson) => {
    document.getElementById("results").innerHTML = JSON.stringify(responseJson);
}

//helper function to display results in the "results2" div
const simpleResponse2 = (responseJson) => {
    document.getElementById("results2").innerHTML = JSON.stringify(responseJson);
}

//get and display the full list of favorite characters
const runGet = () => {
    document.getElementById("errorMessage").innerHTML = "";
    justForceRead = false;
    document.getElementById("randomIndex").innerHTML = "";
    fetch(serverUrl + "/api/favoritecharacters/").then((response) => {
        return response.json();

    }).then(responseJson => {
        simpleResponse(responseJson);
    });
}

//random int generator between a range of two integers
function randomIntInRange(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}

//get and display one random favorite character
const runGetOne = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("randomIndex").innerHTML = "";
    justForceRead = true;
    fetch(serverUrl + "/api/favoritecharacters").then((response) => {
        return response.json();
    }).then(responseJson => {
        if (responseJson.favoriteCharacters.length == 0) {
            document.getElementById("errorMessage").innerHTML = "Unable to Read. List is empty.";
            //If there are objects left, continue on
        } else {
            let tempIndex = randomIntInRange(0, responseJson.favoriteCharacters.length - 1);
            document.getElementById("randomIndex").innerHTML = "Index: " + tempIndex;
            //Save the last force read index
            lastForceReadArray = [tempIndex];
            return fetch(serverUrl + "/api/favoritecharacters/" + tempIndex);
        }
    }).then(response => response.json()).then(responseJson => {
        simpleResponse(responseJson);
    });
}

//get and display views for the last force read character
const getViews = () => {
    document.getElementById("errorMessage").innerHTML = "";
    justForceRead = false;
    document.getElementById("randomIndex").innerHTML = "";
    if (lastForceReadArray[0] == -1) {
        //Force has not been read yet, display error message
        document.getElementById("errorMessage").innerHTML = "Please Force Read a Character First";
    } else {
        //Force has been read, clear error message
        document.getElementById("errorMessage").innerHTML = "";
        fetch(serverUrl + "/api/favoritecharacters/" + lastForceReadArray[0] + "/views")
            .then(response => response.json())
            .then(responseJson => {
                simpleResponse2(responseJson);
            });
    }
}

//post a new view date for the last force read character
const postViews = () => {
    document.getElementById("errorMessage").innerHTML = "";
    justForceRead = false;
    document.getElementById("randomIndex").innerHTML = "";
    if (lastForceReadArray[0] == -1) {
        //Force has not been read yet, display error message
        document.getElementById("errorMessage").innerHTML = "Please Force Read a Character First";
    } else {
        //Force has been read, clear error message
        document.getElementById("errorMessage").innerHTML = "";
        fetch(serverUrl + "/api/favoritecharacters/" + lastForceReadArray[0] + "/views", {
            method: "POST",
            body: JSON.stringify({
                ViewDate: document.getElementById("viewDate").value,
            }),
            headers: {
                "Content-Type": "application/json",
            }
        }).then((response) => {
            return response.json();
        }).then(responseJson => {
            simpleResponse2(responseJson);
        });
    }
}

//Post user first and last name, as well as favorite character
const runPost = () => {
    document.getElementById("errorMessage").innerHTML = "";
    justForceRead = false;
    document.getElementById("randomIndex").innerHTML = "";
    fetch(serverUrl + "/api/favoritecharacters", {
        method: "POST",
        body: JSON.stringify({
            firstName: document.getElementById("firstName").value,
            lastName: document.getElementById("lastName").value,
            favoriteCharacter: document.getElementById("favoriteCharacter").value,
        }),
        headers: {
            "Content-Type": "application/json",
        }
    }).then((response) => {
        return response.json();
    }).then(responseJson => {
        simpleResponse(responseJson);
    });
}

//Delete a random index 
const runDelete = () => {
    document.getElementById("errorMessage").innerHTML = "";
    fetch(serverUrl + "/api/favoritecharacters").then((response) => {
        return response.json();
    }).then(responseJson => {
        //Check and make sure there are still objects left to delete
        if (responseJson.favoriteCharacters.length == 0) {
            document.getElementById("errorMessage").innerHTML = "Unable to Delete. List is empty.";
            //If there are objects left, continue on
        } else {
            //reset error message
            document.getElementById("errorMessage").innerHTML = "";
            //If we just force read, delete the index that was just read
            if (justForceRead) {
                //delete read object
                fetch(serverUrl + "/api/favoritecharacters/" + lastForceReadArray[0], {
                    method: "DELETE",
                }).then(() => {
                    //Display entire list again after delete is called
                    fetch(serverUrl + "/api/favoritecharacters/")
                        .then(response => response.json())
                        .then(responseJson => {
                            simpleResponse(responseJson);
                        });
                });
                //Display index of object you are about to delete
                document.getElementById("randomIndex").innerHTML = "Index: " + lastForceReadArray[0];
                //Otherwise delete a random one
            } else {
                let tempIndex = randomIntInRange(0, responseJson.favoriteCharacters.length - 1);
                //Display index of object you are about to delete
                document.getElementById("randomIndex").innerHTML = "Index: " + tempIndex;
                //Delete that object
                fetch(serverUrl + "/api/favoritecharacters/" + tempIndex, {
                    method: "DELETE",
                }).then(() => {
                    //Display entire list again after delete is called
                    fetch(serverUrl + "/api/favoritecharacters/")
                        .then(response => response.json())
                        .then(responseJson => {
                            simpleResponse(responseJson);
                        });
                });
            }
        }
    })
}

//hook up all the buttons to their corresponding functions when the page loads
window.onload = function () {
    document.getElementById("forcePull").onclick = runGet;
    document.getElementById("forceRead").onclick = runGetOne;
    document.getElementById("forcePush").onclick = runPost;
    document.getElementById("forceInsight").onclick = getViews;
    document.getElementById("watchMovies").onclick = postViews;
    document.getElementById("deleteButton").onclick = runDelete;
}
