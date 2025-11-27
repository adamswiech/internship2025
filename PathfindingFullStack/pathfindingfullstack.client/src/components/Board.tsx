import { useEffect, useState } from "react";
import "../App.css";

interface Field {
    id: number;
    className: string;
    type: "Blank" | "Path" | "Obstacle" | "Dijkstra" | "Filippa";
    z: number;
    previous?: {
        type: Field["type"];
        className: string;
    };
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
    const [fields, setFields] = useState<Field[]>(()=>generateBoard());
    const [disabled, setDisabled] = useState(true);
    const [path, setPath] = useState<Point[]>([]);


    function generateBoard(): Field[] {
        const arr: Field[] = [];
        const size = height * width;

        for (let i = 0; i < size; i++) {
            arr.push({ id: i, className: "pole", z: 0, type:"Blank" });
        }
        let posDijkstra = Math.floor(Math.random() * size);
        let posFilippa = Math.floor(Math.random() * size);
        while (posFilippa === posDijkstra) {
        posFilippa = Math.floor(Math.random() * size);
        }

        arr[posDijkstra].className += " dijkstra";
        arr[posFilippa].className += " filippa";
        arr[posDijkstra].type = "Dijkstra";
        arr[posFilippa].type = "Filippa";

        return arr;
    }

    function randomObs() {
        const posD = fields.findIndex(f => f.type.includes("Dijkstra"));
        const posF = fields.findIndex(f => f.type.includes("Filippa"));
        const newArr = [...fields];

        for (let i = 0; i < 5; i++) {
            const id = Math.floor(Math.random() * (width * height));

            if (id !== posD && id !== posF) {
                newArr[id].className = "przeszkoda";
                newArr[id].type = "Obstacle";
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
            setFields(prev =>
            prev.map(field => {
            if (field.id === id && (field.type === "Dijkstra" || field.type === "Filippa")) return field;
            if (field.id !== id) return field;

        if (field.type !== "Obstacle") {
            return {
            ...field,
            previous: {
                type: field.type,
                className: field.className
            },
            type: "Obstacle",
            className: "przeszkoda"
            };
        }

        if (field.previous) {
            return {
            ...field,
            type: field.previous.type,
            className: field.previous.className,
            previous: undefined
            };
        }

        return { 
            ...field, 
            type: "Blank", 
            className: "pole" 
        };
        })
    );
    }

    async function sendToBackend() {
        const posDijkstra = fields.find(f => f.type.includes("Dijkstra"))!;
        const posFilippa = fields.find(f => f.type.includes("Filippa"))!;

        const allfields = fields.map(f => ({
                XPosition: Math.floor(f.id / width),
                YPosition: f.id % width,
                value: 0
            }));

        
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
            allfields,
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
        const board = generateBoard();
        setFields(board);
    }, [height, width]);

    useEffect(() => {
        drawPath()
    },[path]);

    return (
        <div>
            <div
                className="board"
                style={{
                    /*
                    display: "grid",
                    gridTemplateColumns: `repeat(${width}, 1fr)`,
                    gridTemplateRows: `repeat(${height}, 1fr)`,
                    */
                    display: "flex",
                    flexWrap: "wrap",
                    height: "fit-content",
                    width:"fit-content",
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
