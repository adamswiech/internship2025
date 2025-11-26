import { useState } from "react";
import "./App.css";
import Board from "./components/Board";

function App() {
  const [height, setHeight] = useState(10);
  const [width, setWidth] = useState(10);
return (
    <>
      <div>wymiary
        <input type="number"
              min={10}
              value={height}
              onChange={(e) => setHeight(Number(e.target.value))}/>
        <input type="number"
              min={10}
              value={width}
              onChange={(e) => setWidth(Number(e.target.value))} />
        </div>
        <div id="duzyBen" style={{ width: `${width*50}px` }}>
        <Board height={height} width={width}>
        </Board>
      </div>
    </>
  );
}

export default App;