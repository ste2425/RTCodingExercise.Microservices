import { displayToast } from '../toast.js'

export async function createPlate(data) {
    const url = new URL(`${window.apis.catalog}plates`);

    const request = await fetch(url, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!request.ok) {
        displayToast('Error saving plate', 'Please try again later');
    } else {
        displayToast('Plate created', `Plate '${data.registration}' created`);
    }
}
