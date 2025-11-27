import { useState, useEffect, useCallback } from "react";
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
    const [checked, setChecked] = useState(false);
    const handleResize = useCallback(() => {
        setWidth(Math.floor(innerWidth / 50));
        setHeight(Math.floor(innerHeight / 50) - 2);
    }, [setWidth, setHeight]);
    //function handleResize() {
    //    setWidth(Math.floor(innerWidth / 50))
    //    setHeight(Math.floor(innerHeight / 50)-2)
    //    console.log(innerHeight)
    //}
    useEffect(() => {
        if (checked) {
            window.addEventListener("resize", handleResize);
            setWidth(Math.floor(innerWidth / 50))
            setHeight(Math.floor(innerHeight / 50)-1)
            
        } else {
            window.removeEventListener("resize", handleResize);
            setWidth(10)
            setHeight(10)
        }
    }, [checked]);
    
return (
    <>
        <div id="responsywne">
            <label htmlFor="responsive">responsywne wymiary</label>
            <input type="checkbox" id="responsive" checked={checked} onChange={(e) => setChecked(e.target.checked)}></input>
        </div>
        
        <div id="wymiary">wymiary
            
        <input type="number"
                min={10}
                placeholder="y"
              //value={height}
              onBlur={(e) => checkHeight(Number(e.target.value))}/>
        <input type="number"
              min={10}
                max={30}
                placeholder="x"
            //value={width}
              onBlur={(e) => checkWidth(Number(e.target.value))} />
        </div>
        <div id="duzyBen" style={{ width: `${width * 50}px`, height: `${height * 50}px` }}>
        <Board height={height} width={width}>
        </Board>
      </div>
    </>
  );
}

export default App;