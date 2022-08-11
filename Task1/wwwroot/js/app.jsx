import FilesPage from "./pages/filesPage.jsx";
import NavBar from "./components/navBar.jsx";

class App extends React.Component {
    render() {
        return <>
            <NavBar />
            <FilesPage />
        </>;
    }
}
ReactDOM.render(
    <App />,
    document.getElementById("content")
);