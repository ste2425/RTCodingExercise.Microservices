import { displayToast } from '../toast.js'

export async function toggleReserved(id, data) {
    const url = new URL(`${window.apis.catalog}plates/${encodeURIComponent(id)}`);

    data.reserved = !data.reserved;

    const request = await fetch(url, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!request.ok) {
        displayToast('Error reserving plate', 'Please try again later');
    } else {
        const message = data.reserved ? 'Plate has been reserved' : 'Plate is no longer reserved';
        displayToast('Plate modified', message);
    }

    return data.reserved;
}