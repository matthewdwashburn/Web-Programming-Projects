//Author: Matthew Washburn
//Version: Fall 2025

//Server url for all fetch requests
const serverUrl = "https://localhost:7282/api";

//Helper function to display results in the "results" div
const simpleResponse = (responseJson) => {
    document.getElementById("results").innerHTML = JSON.stringify(responseJson);
}

//Store the last ETag globally for If-None-Match requests
let lastETag = null;

//GET all gargoyles from the database
const runGet = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("eTag").innerHTML = "";
    document.getElementById("results").innerHTML = "";
    fetch(serverUrl + "/gargoyles/").then((response) => {

        return response.json();
    }).then(responseJson => {
        simpleResponse(responseJson);
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//GET a specific gargoyle by name with If-None-Match support
const runGetOne = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("eTag").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    const nameValue = document.getElementById("name").value;
    if (!nameValue || nameValue.trim() === "") {
        document.getElementById("errorMessage").innerHTML = "Please enter a name";
        return;
    }

    const headers = {
        "Content-Type": "application/json"
    };

    // Add If-None-Match header if we have a stored ETag
    if (lastETag) {
        headers["If-None-Match"] = lastETag;
    }

    fetch(serverUrl + "/gargoyles/" + nameValue, { headers }).then((response) => {
        // Check for 304 Not Modified status
        if (response.status === 304) {
            throw new Error("Data has not been modified since the last GET request");
        }
        // Store the new ETag for future requests
        lastETag = response.headers.get("ETag");
        document.getElementById("eTag").innerHTML = lastETag;
        return response.json();
    }).then(response => {
        simpleResponse(response);
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//PATCH a gargoyle with ETag validation
const runPatch = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("eTag").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    const nameValue = document.getElementById("name").value;
    if (!nameValue || nameValue.trim() === "") {
        document.getElementById("errorMessage").innerHTML = "Please enter a name";
        return;
    }

    // First GET to retrieve the current ETag
    fetch(serverUrl + "/gargoyles/" + document.getElementById("name").value).then(response => {
        return response.headers.get("ETag");
    }).then(eTag => {
        // Then PATCH with the retrieved ETag
        fetch(serverUrl + "/gargoyles/" + nameValue, {
            method: "PATCH",
            body: JSON.stringify({
                Name: document.getElementById("name").value || null,
                Color: document.getElementById("color").value || null,
                Size: document.getElementById("size").value || null,
                Gender: document.getElementById("gender").value || null,
            }),
            headers: {
                "Content-Type": "application/json",
                "if-match": eTag
            }
        }).then(response => {
            return response.json();
        })
            .then(responseJson => {
                simpleResponse(responseJson);
            })
            .catch(error => {
                document.getElementById("errorMessage").innerHTML = error.message;
            });
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//PATCH a gargoyle with wildcard ETag override
const runPatchOverride = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("eTag").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    const nameValue = document.getElementById("name").value;
    if (!nameValue || nameValue.trim() === "") {
        document.getElementById("errorMessage").innerHTML = "Please enter a name";
        return;
    }

    // PATCH with wildcard to bypass ETag validation
    fetch(serverUrl + "/gargoyles/" + nameValue, {
        method: "PATCH",
        body: JSON.stringify({
            Name: document.getElementById("name").value || null,
            Color: document.getElementById("color").value || null,
            Size: document.getElementById("size").value || null,
            Gender: document.getElementById("gender").value || null,
        }),
        headers: {
            "Content-Type": "application/json",
            "if-match": "*"
        }
    }).then(response => {
        return response.json();
    })
        .then(responseJson => {
            simpleResponse(responseJson);
        })
        .catch(error => {
            document.getElementById("errorMessage").innerHTML = error.message;
        });
}

//POST a new gargoyle
const runPost = () => {
    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("eTag").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    fetch(serverUrl + "/gargoyles/", {
        method: "POST",
        body: JSON.stringify({
            Name: document.getElementById("name").value || null,
            Color: document.getElementById("color").value || null,
            Size: document.getElementById("size").value || null,
            Gender: document.getElementById("gender").value || null,
        }),
        headers: {
            "Content-Type": "application/json",
        }
    }).then((response) => {
        // Check for 409 Conflict (duplicate name)
        if (response.status === 409) {
            document.getElementById("errorMessage").innerHTML = "A gargoyle with that name already exists";
        }
        return response.json();
    }).then(responseJson => {
        simpleResponse(responseJson);
    }).catch(error => {
        document.getElementById("errorMessage").innerHTML = error.message;
    });
}

//PUT to replace an entire gargoyle
const runPut = () => {

    document.getElementById("errorMessage").innerHTML = "";
    document.getElementById("eTag").innerHTML = "";
    document.getElementById("results").innerHTML = "";

    const nameValue = document.getElementById("name").value;
    if (!nameValue || nameValue.trim() === "") {
        document.getElementById("errorMessage").innerHTML = "Please enter a name";
        return;
    }

    fetch(serverUrl + "/gargoyles/" + nameValue, {
        method: "PUT",
        body: JSON.stringify({
            Name: document.getElementById("name").value || null,
            Color: document.getElementById("color").value || null,
            Size: document.getElementById("size").value || null,
            Gender: document.getElementById("gender").value || null,
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

//Hook up all buttons to their corresponding functions when the page loads
window.onload = function () {
    document.getElementById("getAll").onclick = runGet;
    document.getElementById("getOne").onclick = runGetOne;
    document.getElementById("post").onclick = runPost;
    document.getElementById("patch").onclick = runPatch;
    document.getElementById("patchOverride").onclick = runPatchOverride;
    document.getElementById("put").onclick = runPut;
}