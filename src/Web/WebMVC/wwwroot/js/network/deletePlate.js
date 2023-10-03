import { displayToast } from '../toast.js'

export async function deletePlate(id) {
    const url = new URL(`${window.apis.catalog}plates/${encodeURIComponent(id)}`);

    const request = await fetch(url, {
        method: 'DELETE'
    });

    if (!request.ok) {
        displayToast('Error deleting plate', 'Please try again later');
    } else {
        displayToast('Plate deleted', 'Plate has been successfully deleted');
    }
}
