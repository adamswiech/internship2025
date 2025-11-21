import { useEffect, useState } from 'react'
import './App.css'

function App() {
    const [matrix, setMatrix] = useState<number[][]>([]);
    useEffect(() => {
        fetch("https://localhost:7287/api/board", { credentials: "omit" })
            .then(res => res.json())
            .then(data => setMatrix(data))
            .catch(err => console.error("Fetch error:", err));
    }, []);
    


  return (
    <>
          <div>


              {matrix.map((row, rowIndex) => (
                  <div key={rowIndex}>
                      {row.map((value, colIndex) => (
                          <span key={colIndex} style={{ marginRight: "10px" }}>
                              {value}
                          </span>
                      ))}
                  </div>
              ))}
          </div>
    </>
  )
}

export default App
