import ClickCounter from './components/ClickCounter'
import { useCallback, useState } from "react";
import { Button } from "react-bootstrap";

/*Author: Matthew Washburn
Version: Fall 2025*/

import './App.css'

const App = () => {
  const [teamScores, setTeamScores] = useState({"Earth 1": 0, "Earth 2": 0, 
    "Earth 3": 0, "Water": 0, "Air": 0, "Fire": 0});
  //Set a changeable variable for the number of rocks
  const [rockCount, setRockCount] = useState(0);
  //Set a changeable variable for the size of each rock
  const [rockSizes, setRockSizes] = useState([]);

  const onRockCountChange = useCallback((newCount) => {
    const currentCount = Math.min(1000, rockCount);
    // Cap rock count at 1000
    const cappedRockCount = Math.min(1000, newCount);
    setRockCount(cappedRockCount);
    
    // If adding rocks, generate new sizes for the new rocks
    if (newCount > currentCount) {
      const newSizes = [...rockSizes];
      for (let i = currentCount; i < newCount; i++) {
        //Random size from 50-150 px
        newSizes.push(Math.floor(Math.random() * 100) + 50);
      }
      setRockSizes(newSizes);
    } else if (newCount < currentCount) {
      // If removing rocks, trim the sizes array
      setRockSizes(rockSizes.slice(0, newCount));
    }
    
    return newCount;
    //Get a new function every time rock count or rock sizes changes
  }, [rockCount, rockSizes]);

  const onScoreChange = useCallback((elementName, score) => {
    setTeamScores({ ...teamScores, [elementName]: score });
  },
    //Get a new function every time teamScores changes
    [teamScores]);

  //Store the math calculation values each element button does to the rock count
  const buttonChangeValues = [ 
    {multiply: 1, add: 1},
    {multiply: 1, add: 2},
    {multiply: 1, add: 3},
    {multiply: 2, add: 0},
    {multiply: 0.5, add: 0},
    {multiply: 0, add: 0}
  ];
  //Change the passed in rock's size
  const changeRockSize = (size, index) => {
    const rockImg = document.getElementById(`rock-${index}`)
    let newSize;
    do {
     newSize = (Math.random() * 100) + 50;
    } while(Math.abs(size - newSize) <= 20);
    rockImg.style.setProperty('--rock-size', `${newSize}px`);
  }
  //Set up containters for the rock count, element buttons, and rock images
  return (
    <div className="ApplicationContainer">
      <div className={"rockCount"}>Rock Count: {rockCount}</div>
      <div className={"elementContainer"}>
      {Object.keys(teamScores).map((elementName, index) => (
        //Set up a click counter class to duplicate for each element button
      <ClickCounter
        key={elementName}
        elementName={elementName}
        scoreValue={teamScores[elementName] ?? 0}
        onScoreChange={onScoreChange}
        rockCount={rockCount}
        onRockCountChange={onRockCountChange}
        scoreAddChange={buttonChangeValues[index].add}
        scoreMultiplyChange={buttonChangeValues[index].multiply}
      ></ClickCounter>
    ))}
      </div>
      <div className={"rockContainer"}>
      {rockSizes.map((size, index) => (
        //Map each rock size to each rock button image tag
        <Button className="rockButton" key={index} onClick={() => changeRockSize(size, index)}>
          <img 
            id={`rock-${index}`}
            className="rockImage" 
            src="https://png.pngtree.com/png-vector/20250125/ourmid/pngtree-large-gray-rock-png-image_15333471.png" 
            alt="rock"
            style={{'--rock-size': `${size}px`}}
          ></img>
          </Button>
        
        ))}
      </div>
  </div>
  );
};

export default App;
