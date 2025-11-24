import { useEffect, useState } from 'react'
import './App.css'

function App() {
    const [matrix, setMatrix] = useState<number[][]>([]);

    useEffect(() => {
        fetch("https://localhost:7287/api/board")
            .then(res => res.json())
            .then((data: { xPosition: number, yPosition: number, value: number }[]) => {

                const size = 10; // your board is 10x10
                const newMatrix = Array.from({ length: size }, () =>
                    Array.from({ length: size }, () => 0)
                );

                // Fill the matrix based on points from backend
                data.forEach(point => {
                    newMatrix[point.yPosition][point.xPosition] = point.value;
                });

                setMatrix(newMatrix);
            })
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
    );
}

export default App;
