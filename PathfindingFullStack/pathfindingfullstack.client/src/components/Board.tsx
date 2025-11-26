import { useEffect, useState } from "react";
import "../App.css";

interface Field {
    id: number;
    className: string;
    type: string;
    z: number;
}

interface BoardSize {
    height: number;
    width: number;
}

interface Point {
    xPosition: number;
    yPosition: number;
    value: number;
}

export default function Board({ height, width }: BoardSize) {
    const [fields, setFields] = useState<Field[]>([]);
    const [disabled, setDisabled] = useState(true);
    const [path, setPath] = useState<Point[]>([]);

    const size = height * width;

    function generateBoard(): Field[] {
        const arr: Field[] = [];

        for (let i = 0; i < size; i++) {
            arr.push({ id: i, className: "pole", z: 0, type:"" });
        }

        const posDijkstra = Math.floor(Math.random() * size);
        const posFilippa = Math.floor(Math.random() * size);

        arr[posDijkstra].className += " dijkstra";
        arr[posFilippa].className += " filippa";

        return arr;
    }

    function randomObs() {
        const posD = fields.findIndex(f => f.className.includes("dijkstra"));
        const posF = fields.findIndex(f => f.className.includes("filippa"));
        const newArr = [...fields];

        for (let i = 0; i < 5; i++) {
            const id = Math.floor(Math.random() * size);

            if (id !== posD && id !== posF && !newArr[id].className.includes("sciezka")) {
                newArr[id].className = "przeszkoda";
            }
        }

        setFields(newArr);
    }

    function resetBoard() {
        const board = generateBoard();
        setFields(board);
        setPath([]);
    }

    function Choosing() {
        setDisabled(!disabled);
    }

    function setObs(id: number) {
        const newArr = [...fields];
        if (newArr[id].className === "pole")
            newArr[id].className = "przeszkoda";
        else if (newArr[id].className === "przeszkoda")
            newArr[id].className = "pole";

        setFields(newArr);
    }

    async function sendToBackend() {
        const posDijkstra = fields.find(f => f.className.includes("dijkstra"))!;
        const posFilippa = fields.find(f => f.className.includes("filippa"))!;

        const obstacles = fields
            .filter(f => f.className.includes("przeszkoda"))
            .map(f => ({
                XPosition: Math.floor(f.id / width),
                YPosition: f.id % width,
                value: 0
            }));

        const payload = {
            width,
            height,
            start: {
                XPosition: Math.floor(posDijkstra.id / width),
                YPosition: posDijkstra.id % width,
            },
            end: {
                XPosition: Math.floor(posFilippa.id / width),
                YPosition: posFilippa.id % width,
            },
            obstacles
        };

        const res = await fetch("http://localhost:5251/api/path", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload),
        });

        const path: { board: Point[] } = await res.json();
        const found = path.board.filter((p: Point) => p.value === 2);
        setPath(found);
        console.log("Received path:", path.board);
        path.board.forEach((p, i) => {
        console.log(`Point ${i}:`, p, "X:", p.xPosition, "Y:", p.yPosition);
        drawPath();
        });
    }
    function drawPath() {
           const newArr = fields.map(f => {
        if (f.className.includes("sciezka")) {
            return { ...f, className: "pole" };
        }
        return { ...f };
    })

        path.forEach(p => {
            const id = p.xPosition * width + p.yPosition;
            if (!newArr[id].className.includes("dijkstra") &&
                !newArr[id].className.includes("filippa")) {
                newArr[id].className += " sciezka";
            }
        });

        setFields(newArr);
    }

    useEffect(() => {
        setPath([]);
        const board = generateBoard();
        setFields(board);
    }, [height, width]);

    useEffect(() => {
    if (path.length === 0) return;

    setFields(prevFields => {
        const newArr = [...prevFields];

        path.forEach(p => {
            const id = p.xPosition * width + p.yPosition;

            if (!newArr[id]) {
                console.log("Warning: field undefined for id", id);
                return;
            }

            if (!newArr[id].className.includes("dijkstra") &&
                !newArr[id].className.includes("filippa")) {
                newArr[id].className += " sciezka";
            }
        });

        console.log("Path drawn on board:", newArr);
        return newArr;
    });
    }, [path, width, height]);

    return (
        <div>
            <div
                className="board"
                style={{
                    display: "grid",
                    gridTemplateColumns: `repeat(${width}, 1fr)`,
                    gridTemplateRows: `repeat(${height}, 1fr)`,
                }}
            >
                {fields.map((f) => (
                    <div
                        key={f.id}
                        className={f.className}
                        onClick={disabled ? undefined : () => setObs(f.id)}
                    ></div>
                ))}
            </div>

            <button onClick={randomObs} style={{ marginTop: "10px" }}>
                Dodaj przeszkody
            </button>

            <button onClick={resetBoard} style={{ marginTop: "10px" }}>
                Resetuj plansze
            </button>

            <button onClick={Choosing} style={{ marginTop: "10px" }}>
                {disabled ? "Zacznij stawianie" : "Zatrzymaj stawianie"}
            </button>

            <button onClick={sendToBackend} style={{ marginTop: "10px" }}>
                Wyznacz ścieżkę (A*)
            </button>

        </div>
    );
}
