import { getFiles } from "../requests/requests.js";
import CombineFilesForm from "../components/combineFilesForm.jsx";

class FilesPage extends React.Component {

    constructor(props) {
        super(props);
        this.state = { files: undefined };
        getFiles((data) => this.setState({ files: data }));
    }
    render() {
        if (!this.state.files)
            return (<div className="loading">Loading...</div>);
        return <>
            <div className="title">Loaded files:</div>
            <table className="list">
                <thead>
                    <tr>
                        <th>File</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        this.state.files.map((file) => {
                            return <tr>
                                <td><a className="listElement" href={`api/files/${file}`} > {file} </a></td>
                                <td><a className={`listElement btn btn-secondary`} href={`api/data/import?importedFileName=${file}`}> Load into Db </a></td>
                            </tr>;
                        }
                        )
                    }
                </tbody>
            </table>
            <a className="btn btn-secondary" href="/api/files/create?numberOfFiles=100&linesInFile=5000">Create files</a>
            <CombineFilesForm />
        </>;
    }
}

export default FilesPage;