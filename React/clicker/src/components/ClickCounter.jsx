import "./ClickCounter.css";
import { Button } from "react-bootstrap";

/*Author: Matthew Washburn
Version: Fall 2025*/

//Take in all necessary params
const ClickCounter = ({ elementName, scoreMultiplyChange, scoreAddChange, onRockCountChange, rockCount }) => {
    let isDisabled = false;
    let styleClass = "";

    // Air, Fire, and water disabled at rock count 0
    if((elementName === "Air" || elementName === "Water" || elementName === "Fire") && rockCount === 0) {
        isDisabled = true;
    }
    
    // Set Button styles
    if(elementName === "Earth 1" || elementName === "Earth 2" || elementName === "Earth 3") {
        styleClass = "earth-button";
    }

    if(elementName === "Water") {
        styleClass = "water-button";
    }

    if(elementName === "Fire") {
        styleClass = "fire-button";
    }

    if(elementName === "Air") {
        styleClass = "air-button";
    }

    //Ground and water disabled at rock count 1000
    if((elementName === "Earth 1" || elementName === "Earth 2" || elementName === "Earth 3" || elementName === "Water") && rockCount >= 1000) {
        isDisabled = true;
    }
    return (
        //When clicked, calculate new rock cound and send it back over
        <div className="clickCounterContainer">
            <div className="buttonContainer">
                <Button onClick={() => onRockCountChange(parseInt(rockCount  * scoreMultiplyChange + scoreAddChange))}
                    disabled={isDisabled}
                    className={styleClass}
                    >{elementName}</Button>
            </div>
        </div>
    );
};

export default ClickCounter;