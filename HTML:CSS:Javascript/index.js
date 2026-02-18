//Author: Matthew Washburn, Steven Yackel
//Project: Intro Website Design
//Version: Fall 2025
const parse = () => {
    let errorDiv = document.getElementById("error");
    errorDiv.innerText = "";

    let userInput = document.getElementById("userInput").value;

    try {
        let userinputObject = JSON.parse(userInput);
        if (userinputObject.color) {
            document.getElementById("title").style.color = userinputObject.color;
            document.getElementById("body").style.color = userinputObject.color;
            document.getElementById("linkText").style.color = userinputObject.color;
            document.getElementById("fixed-corner-div-0").style.color = userinputObject.color;
            document.getElementById("textBoxInfo").style.color = userinputObject.color;
        }
        if (userinputObject.backgroundColor) {
            document.body.style.backgroundColor = userinputObject.backgroundColor;
        }
        if (userinputObject.href) {
            document.getElementById("link").href = userinputObject.href;
        } if (userinputObject.displayText) {
            document.getElementById("link").textContent = userinputObject.displayText;
        }
    } catch {
        errorDiv.innerText = "Invalid JSON";
    }
}
window.onload = function () {
    document.getElementById("parseButton").onclick = parse;
    parse();
};