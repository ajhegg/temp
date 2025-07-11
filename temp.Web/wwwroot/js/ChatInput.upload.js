// ChatInput.upload.js: Handles file upload to Azure Blob Storage using SAS

export async function uploadFiles(inputElem, getSasApiUrl, onProgress, onComplete, onError) {
    const files = inputElem.files;
    for (const file of files) {
        try {
            // Request SAS URL from backend
            const sasRes = await fetch(getSasApiUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ fileName: file.name })
            });
            if (!sasRes.ok) throw new Error('Failed to get SAS URL');
            const { sasUrl } = await sasRes.json();

            // Upload file to blob
            const xhr = new XMLHttpRequest();
            xhr.open('PUT', sasUrl, true);
            xhr.setRequestHeader('x-ms-blob-type', 'BlockBlob');
            xhr.upload.onprogress = (e) => {
                if (onProgress) onProgress(file, e.loaded, e.total);
            };
            xhr.onload = () => {
                if (xhr.status === 201 || xhr.status === 200) {
                    // After upload, trigger indexer
                    import('/js/RunIndexer.js').then(m => m.runIndexer && m.runIndexer());
                    if (typeof window.ChatInputUploadSuccess === 'function') {
                        window.ChatInputUploadSuccess(file.name);
                    }
                    if (onComplete) onComplete(file);
                } else {
                    if (onError) onError(file, xhr.statusText);
                }
            };
            xhr.onerror = () => {
                if (onError) onError(file, xhr.statusText);
            };
            xhr.send(file);
        } catch (err) {
            if (onError) onError(file, err.message);
        }
    }
}
