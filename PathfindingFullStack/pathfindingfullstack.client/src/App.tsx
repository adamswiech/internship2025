import "./App.css";
import Board from "./components/Board";

function App() {
    return (
        <div id="duzyBen">
            <Board height={10} width={10}>
            </Board>
        </div>
    );
}

export default App;