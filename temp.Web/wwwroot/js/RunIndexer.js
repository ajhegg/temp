export async function runIndexer() {
    await fetch('/api/blob-index', { method: 'POST' });
}
