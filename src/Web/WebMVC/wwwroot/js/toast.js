export function displayToast(title, message) {
    const toastEl = document.querySelector('#toast'),
        header = toastEl.querySelector('.toast-header strong'),
        body = toastEl.querySelector('.toast-body');

    header.textContent = title;
    body.textContent = message;

    new bootstrap.Toast(toastEl).show();
}