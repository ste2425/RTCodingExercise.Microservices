// Script for the index page. Perform plate search functionality.
// The searc functionality should be extracted out into a re-useable module to allow any page to perform plates searches

document.addEventListener('DOMContentLoaded', () => {
    const textBox = document.querySelector('#plateSearch'),
        searchBtn = document.querySelector('#plateSearchBtn'),
        previousBtn = document.querySelector('#previousBtn'),
        currentPage = document.querySelector('#currentPage'),
        nextBtn = document.querySelector('#nextBtn'),
        sortBy = document.querySelector('#sort');

    previousBtn.addEventListener('click', () => {
        const page = currentPage.dataset.currentPage;
        performSearch(textBox.value, sortBy.value, parseInt(page) - 1);
    });

    nextBtn.addEventListener('click', () => {
        const page = currentPage.dataset.currentPage;
        performSearch(textBox.value, sortBy.value, parseInt(page) + 1);
    });

    sortBy.addEventListener('change', () => {
        const page = currentPage.dataset.currentPage;
        performSearch(textBox.value, sortBy.value, parseInt(page));
    });

    searchBtn.addEventListener('click', () => performSearch(textBox.value, sortBy.value, 0));

    performSearch('', sortBy.value, 0);
});

async function performSearch(filter, sort, page) {
    try {
        const template = document.querySelector("#plateTemplate"),
            url = new URL(`${window.apis.catalog}plates`),
            resultsContainer = document.querySelector('#searchResults'),
            currencyFormatter = Intl.NumberFormat('en-GB', {
                style: 'currency',
                currency: 'GBP',
            });
    
        url.searchParams.append('filter', filter);
        url.searchParams.append('page', page);
        url.searchParams.append('sort', sort);
    
        const resp = await fetch(url),
            data = await resp.json();
    
        currentPage.textContent = `${data.currentPage + 1} / ${data.totalPages}`;
        currentPage.dataset.currentPage = data.currentPage;
    
        if (data.currentPage > 0)
            previousBtn.removeAttribute('disabled');
        else
            previousBtn.setAttribute('disabled', 'disabled');
    
        if (data.currentPage < (data.totalPages - 1))
            nextBtn.removeAttribute('disabled');
        else
            nextBtn.setAttribute('disabled', 'disabled');
    
        // clear container. Doesn't remove event handlers but thats not an issue for now
        resultsContainer.innerHTML = '';
    
        data.data.forEach((record) => {
            const clone = template.content.cloneNode(true);
    
            clone.querySelector('.reg').textContent = record.registration;
            clone.querySelector('.salePrice').textContent = currencyFormatter.format(record.salePrice);
    
            resultsContainer.appendChild(clone);
        });
    } catch (e) {
        const toastEl = document.querySelector('#errorToast');
        new bootstrap.Toast(toastEl).show();

        console.error('Error loading plates', e);
    } finally {

    }
}