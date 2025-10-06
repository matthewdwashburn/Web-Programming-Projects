//Author: Matthew Washburn
//Version: Fall 2025

//array to store the last index that was force read
let lastForceReadArray = [-1];
//flag to check if the last action was a force read
let justForceRead = false;
//server url for all fetch requests
const serverUrl = "https://localhost:7091/api";

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
    fetch(serverUrl + "/students/").then((response) => {
        return response.json();

    }).then(responseJson => {
        simpleResponse(responseJson);
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
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
    fetch(serverUrl + "/students/").then((response) => {
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
            return fetch(serverUrl + "/students/" + tempIndex);
        }
    }).then(response => response.json()).then(responseJson => {
        simpleResponse(responseJson);
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
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
        fetch(serverUrl + "/students/" + lastForceReadArray[0] + "/views")
            .then(response => response.json())
            .then(responseJson => {
                simpleResponse2(responseJson);
            }).catch(error => {
                document.getElementById("errorMessage").innerHTML = error.message;
            });
    }
}

//post a new view date for the last force read character
const postViews = () => {
    document.getElementById("errorMessage").innerHTML = "";
    justForceRead = false;
    document.getElementById("randomIndex").innerHTML = "";

    const viewDateValue = document.getElementById("viewDate").value;

    if (!viewDateValue) {
        document.getElementById("errorMessage").innerHTML = "ViewDate cannot be empty";
        return;
    }
    if (lastForceReadArray[0] == -1) {
        //Force has not been read yet, display error message
        document.getElementById("errorMessage").innerHTML = "Please Force Read a Character First";
    } else {
        //Force has been read, clear error message
        document.getElementById("errorMessage").innerHTML = "";
        fetch(serverUrl + "/students/" + lastForceReadArray[0] + "/views", {
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
        }).catch(error => {
            document.getElementById("errorMessage").innerHTML = error.message;
        });
    }
}
//Patch last force read student with new values
const patchStudent = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("randomIndex").innerHTML = "";
    const firstName = document.getElementById("firstName").value;
    const lastName = document.getElementById("lastName").value;
    const favoriteCharacter = document.getElementById("favoriteCharacter").value;

    if (lastForceReadArray[0] === -1) {
        document.getElementById("errorMessage").innerHTML = "Please Force Read a Student First";
        return;
    }

    const studentIndex = lastForceReadArray[0];

    const studentPatchData = {
        FirstName: firstName || null, // Only send if non-empty
        LastName: lastName || null, // Only send if non-empty
        FavoriteCharacter: favoriteCharacter || null // Only send if non-empty
    };

    fetch(serverUrl + "/students/" + studentIndex, {
        method: "PATCH",
        body: JSON.stringify(studentPatchData),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => {
            return response.json();
        })
        .then(responseJson => {
            simpleResponse(responseJson);  // Display updated student data
        })
        .catch(error => {
            document.getElementById("errorMessage").innerHTML = error.message;
        });
}


//get a specific view for a given student and view index
const getSingleView = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("randomIndex").innerHTML = "";

    // Get indices from input fields
    const studentIndex = parseInt(document.getElementById("studentIndex").value);
    const viewIndex = parseInt(document.getElementById("viewIndex").value);

    // Validate indices before sending request
    if (isNaN(studentIndex) || studentIndex < 0) {
        document.getElementById("errorMessage").innerHTML = "Invalid student index.";
        return;
    }
    if (isNaN(viewIndex) || viewIndex < 0) {
        document.getElementById("errorMessage").innerHTML = "Invalid view index.";
        return;
    }

    fetch(serverUrl + "/students/" + studentIndex + "/views/" + viewIndex)
        .then(response => {
            return response.json();
        })
        .then(responseJson => {
            // Display the JSON result 
            simpleResponse2(responseJson);
        }).catch(error => {
            document.getElementById("errorMessage").innerHTML = error.message;
        });
};


//Post user first and last name, as well as favorite character
const runPost = () => {
    document.getElementById("errorMessage").innerHTML = "";
    justForceRead = false;
    document.getElementById("randomIndex").innerHTML = "";

    fetch(serverUrl + "/students/", {
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
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//Delete a random index 
const runDelete = () => {
    document.getElementById("errorMessage").innerHTML = "";
    fetch(serverUrl + "/students/").then((response) => {
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
                fetch(serverUrl + "/students/" + lastForceReadArray[0], {
                    method: "DELETE",
                }).then(() => {
                    //Display entire list again after delete is called
                    fetch(serverUrl + "/students/")
                        .then(response => response.json())
                        .then(responseJson => {
                            simpleResponse(responseJson);
                        }).catch(error => {
                            document.getElementById("errorMessage").innerHTML = error.message;
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
                fetch(serverUrl + "/students/" + tempIndex, {
                    method: "DELETE",
                }).then(() => {
                    //Display entire list again after delete is called
                    fetch(serverUrl + "/students/")
                        .then(response => response.json())
                        .then(responseJson => {
                            simpleResponse(responseJson);
                        }).catch(error => {
                            document.getElementById("errorMessage").innerHTML = error.message;
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
    document.getElementById("patchStudent").onclick = patchStudent;
    document.getElementById("forceInsight").onclick = getViews;
    document.getElementById("watchMovies").onclick = postViews;
    document.getElementById("deleteButton").onclick = runDelete;
    document.getElementById("getSingleView").onclick = getSingleView;
}
