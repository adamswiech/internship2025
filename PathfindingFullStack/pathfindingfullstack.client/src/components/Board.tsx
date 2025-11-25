import { useEffect, useState } from "react";
import "../App.css";

interface Field {
    id: number;
    className: string;
}

interface BoardSize {
    height: number;
    width: number;
}

export default function Board({ height, width }: BoardSize) {
    const [fields, setFields] = useState<Field[]>([]);
    const [disabled, setDisabled] = useState(true);
    const size = height * width;

    function generateBoard(): Field[] {
        const arr: Field[] = [];

        for (let i = 0; i < size; i++) {
            arr.push({ id: i, className: "pole" });
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

            if (id !== posD && id !== posF) {
                newArr[id].className = "przeszkoda";
            }
        }

        setFields(newArr);
    }
    function resetBoard() {
        const board = generateBoard();
        setFields(board);
    }

    function Choosing() {
        if (disabled == true)
            setDisabled(false);
        else
            setDisabled(true);
    }

    function setObs(id: number) {
        const newArr = [...fields];
        if (newArr[id].className == "pole")
            newArr[id].className = "przeszkoda";
        else if (newArr[id].className == "przeszkoda")
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
                value:0
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

        console.log("Sending:", payload);

        const res = await fetch("http://localhost:5251/api/path", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload),
        });

        const path = await res.json();
        console.log("received path:", path);
    }

    useEffect(() => {
        const board = generateBoard();
        setFields(board);
    }, [height, width]);

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
                    <div key={f.id} className={f.className} onClick={disabled ? undefined : () => setObs(f.id)} ></div>
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
                Wyznacz œcie¿kê (A*)
            </button>
        </div>
    );
}