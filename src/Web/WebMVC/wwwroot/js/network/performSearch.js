import { displayToast } from '../toast.js'

export async function performSearch(filter, sort, page) {
    try {
        const previousBtn = document.querySelector('#previousBtn'),
            currentPage = document.querySelector('#currentPage'),
            nextBtn = document.querySelector('#nextBtn');

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
            const clone = template.content.cloneNode(true),
                deleteBTN = clone.querySelector('.delete'),
                buyBtn = clone.querySelector('.buy'),
                row = clone.querySelector('.row');

            Object.assign(row.dataset, record);
    
            clone.querySelector('.reg').textContent = record.registration;
            clone.querySelector('.salePrice').textContent = currencyFormatter.format(record.salePrice);

            if  (deleteBTN)
                deleteBTN.dataset.plateId = record.id;

            if (record.reserved)
                buyBtn.setAttribute('disabled', 'disabled');
    
            resultsContainer.appendChild(clone);
        });
    } catch (e) {
        displayToast('Error loading plates', 'Please try again later');

        console.error('Error loading plates', e);
    }
}