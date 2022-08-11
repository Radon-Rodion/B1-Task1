class CombineFilesForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = { fileName: "", substring: "" };
        this.onFileNameChange = this.onFileNameChange.bind(this);
        this.onSubstringChange = this.onSubstringChange.bind(this);
    }
    onFileNameChange(e) {
        this.setState({ fileName: e.target.value });
    }
    onSubstringChange(e) {
        this.setState({ substring: e.target.value });
    }

    onClick = () => {
        const combinedFileName = document.getElementById("combinedFileName").Value;
        const substringForDeletion = document.getElementById("stringForDeletion").Value;
    }

    render() {
        return <form className="combineFilesForm">
            <div className="form-group">
                <label htmlFor="combinedFileName">Combined file:</label>
                <input className="form-control" id="combinedFileName" placeholder="combinedFileName.txt" value={this.state.fileName} onChange={this.onFileNameChange} />
            </div>
            <div className="form-group">
                <label htmlFor="stringForDeletion">String for deletion:</label>
                <input className="form-control" id="stringForDeletion" placeholder="stringForDeletion" value={this.state.substring} onChange={this.onSubstringChange} />
            </div>

            <a className="btn btn-primary" href={`/api/files/combine?combinedFileName=${this.state.fileName}&substringForDeletion=${this.state.substring}`}>Combine</a>
        </form>;
    }
}

export default CombineFilesForm;