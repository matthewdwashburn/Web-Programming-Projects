//Author: Matthew Washburn
//Version: Fall 2025

//server url for all fetch requests
const serverUrl = "https://localhost:7225/api";

let isMarioMoving = false;

//random int generator between a range of two integers
function randomIntInRange(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}

//Move mario to the right by the provided percentage
function moveMarioRight(percent) {
    const marioDiv = document.getElementById("marioDiv");
    let currentLeft = parseFloat(window.getComputedStyle(marioDiv).left);
    let percentToPixels = window.innerWidth * (0.01 * percent);
    marioDiv.style.left = (currentLeft + percentToPixels) + "px"; // ✅ Only inline style needed
}

const marioMoving = async () => {
    while (isMarioMoving) {
        await runGet();
    }
}

//get and display the full list of favorite characters
const runGet = async () => {
    //Check if mario already died
    if (!isMarioMoving) {
        return;
    }
    var marioMovementInt = randomIntInRange(1, 4);

    switch (marioMovementInt) {
        case (1):
            var marioMovementString = "walk";
            break;
        case (2):
            var marioMovementString = "jump";
            break;
        case (3):
            var marioMovementString = "wait";
            break;
        case (4):
            var marioMovementString = "run";
            break;
    }

    const response = await fetch(serverUrl + "/mario/" + marioMovementString);
    const data = await response.json();
    //Display the result of the request
    document.getElementById("results").innerHTML = await data.message;
    console.log(data.message);

    // Check if mario died again after the fetch completes
    if (!isMarioMoving) {
        return;
    }

    const mario = document.getElementById("mario");
    const marioDiv = document.getElementById("marioDiv");

    switch (data.message) {
        case ("Mario died."):
            // Disable transitions for instant change
            mario.classList.add("no-transition");
            marioDiv.classList.add("no-transition");

            // Change to dead state
            mario.src = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/e496e8d8-0665-4149-868b-6c492f70006a/dgzoabp-399c4fc4-6283-4a35-a2d3-2fce8f1f8a37.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiIvZi9lNDk2ZThkOC0wNjY1LTQxNDktODY4Yi02YzQ5MmY3MDAwNmEvZGd6b2FicC0zOTljNGZjNC02MjgzLTRhMzUtYTJkMy0yZmNlOGYxZjhhMzcucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.gZXch6TDN8QiNX7LGg1iP6Hwa4xEWUQDGT6omx2dqWg";
            mario.classList.remove("mario-alive");
            mario.classList.add("mario-dead");
            marioDiv.classList.remove("marioDiv-alive");
            marioDiv.classList.add("marioDiv-dead");

            // Re-enable transitions
            setTimeout(() => {
                mario.classList.remove("no-transition");
                marioDiv.classList.remove("no-transition");
            }, 50);

            isMarioMoving = false;
            break;
        case ("Mario walked super well!"):
            moveMarioRight(5);
            break;
        case ("Mario waited patiently!"):
            moveMarioRight(0);
            break;
        case ("Mario made the jump!"):
            moveMarioRight(5);
            break;
        case ("Mario ran fast!"):
            moveMarioRight(10);
            break;
    }

    let currentMarioLeftPixels = parseFloat(window.getComputedStyle(marioDiv).left);
    let currentPoleLeftPixels = parseFloat(window.getComputedStyle(goalPole).left);

    console.log("Mario position:", currentMarioLeftPixels);
    console.log("Pole position:", currentPoleLeftPixels);

    // Check if mario crossed the flag
    if (currentMarioLeftPixels >= currentPoleLeftPixels-100) {
        isMarioMoving = false;
        document.getElementById("results").innerHTML = "Mario completed the level!";
    }
}

//Reset and start mario movement on button click
window.onload = function () {
    document.getElementById("startButton").onclick = function () {
        const mario = document.getElementById("mario");
        const marioDiv = document.getElementById("marioDiv");

        // Reset to alive state
        mario.src = "https://supermariorun.com/assets/img/hero/hero_chara_mario_pc.png";
        mario.classList.remove("mario-dead");
        mario.classList.add("mario-alive");
        marioDiv.classList.remove("marioDiv-dead");
        marioDiv.classList.add("marioDiv-alive", "marioDiv-start");

        // Clear all inline styles so css classes can work
        marioDiv.style.left = "";
        marioDiv.style.top = "";
        mario.style.width = "";
        mario.style.height = "";

        // Start game and wait for mario to slide back if needed
        setTimeout(() => {
            isMarioMoving = true;
            marioMoving();
        }, 600);
    }
}