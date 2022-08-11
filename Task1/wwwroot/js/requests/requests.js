export async function getFiles(callback) {
    const response = await fetch("/api/files");

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body (the method explained below)
        const responseData = await response.json();
        console.log(responseData);

        callback(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}

export async function generateFiles(numberOfFiles, linesInFile, callback) {
    const response = await fetch("/api/files/create", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({
            numberOfFiles,
            linesInFile
        })
    });

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body (the method explained below)
        const responseData = await response.json();
        callback(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}

export async function getFile(fileName, callback) {
    const response = await fetch(`/api/files/${fileName}`);

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body (the method explained below)
        const responseData = await response.json();
        console.log(responseData);

        callback(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}

export async function getProcessedAmount(callback) {
    const response = await fetch("/api/files/processed");

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body (the method explained below)
        const responseData = await response.json();
        console.log(responseData);

        callback(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}

export async function getRemovedLinesAmount(callback) {
    const response = await fetch("/api/files/removed");

    if (response.ok) { // if HTTP-status is 200-299
        // get the response body (the method explained below)
        const responseData = await response.json();
        console.log(responseData);

        callback(responseData);
    } else {
        alert("HTTP-Error: " + response.status);
    }
}