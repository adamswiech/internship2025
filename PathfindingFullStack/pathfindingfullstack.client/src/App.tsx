import { useState } from "react";
import "./App.css";
import Board from "./components/Board";

function App() {
    function checkHeight(inputHeight: number) {
        if (inputHeight < 10) {
            inputHeight = 10;
        }
        setHeight(inputHeight);
    }
    function checkWidth(inputWidth: number) {
        if (inputWidth < 10) {
            inputWidth = 10;
        } else if (inputWidth > 30) {
            inputWidth = 30;
        }
        setWidth(inputWidth);
    }
  const [height, setHeight] = useState(10);
  const [width, setWidth] = useState(10);
return (
    <>
      <div id="wymiary">wymiary
        <input type="number"
              min={10}
              value={height}
              onChange={(e) => checkHeight(Number(e.target.value))}/>
        <input type="number"
              min={10}
              max={30}
              value={width}
              onChange={(e) => checkWidth(Number(e.target.value))} />
        </div>
        <div id="duzyBen" style={{ width: `${width*50}px` }}>
        <Board height={height} width={width}>
        </Board>
      </div>
    </>
  );
}

export default App;