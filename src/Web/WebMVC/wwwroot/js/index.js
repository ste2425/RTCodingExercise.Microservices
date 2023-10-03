import { deletePlate, createPlate, toggleReserved, performSearch } from './network/index.js';

// Script for the index page. Perform plate search functionality.
// The searc functionality should be extracted out into a re-useable module to allow any page to perform plates searches

document.addEventListener('DOMContentLoaded', () => {
    const textBox = document.querySelector('#plateSearch'),
        searchBtn = document.querySelector('#plateSearchBtn'),
        previousBtn = document.querySelector('#previousBtn'),
        currentPage = document.querySelector('#currentPage'),
        nextBtn = document.querySelector('#nextBtn'),
        sortBy = document.querySelector('#sort');

    // Event delegation. Events buddle so regiser one click and check if the
    // element is a delete button. Saves lots of even handlers for each individual delete button
    document.addEventListener('click', async (e) => {
        if (e.target.matches('.delete')) {
            const plateId = e.target.dataset.plateId;

            if (!plateId)
                return;

            e.target.setAttribute('disabled', 'disabled');
            await deletePlate(plateId);

            e.target.closest('.row').remove();
        } else if (e.target.matches('.reserve')) {
            const row = e.target.closest('.row'),
                buyBtn = row.querySelector('.buy');

            const plate = row.dataset;

            const reserved = await toggleReserved(plate.id, {
                ...plate,
                reserved: plate.reserved === 'true',
                forSale: plate.forSale === 'true'
            });

            row.dataset.reserved = reserved;

            if (reserved)
                buyBtn.setAttribute('disabled', 'disabled');
            else
                buyBtn.removeAttribute('disabled');
        }
    });

    const createPlateBtn = document.querySelector('#createPlate'),
        createPlateModal = new bootstrap.Modal('#createPlateModal'),
        createPlateForm = document.querySelector('#createPlateModal form'),
        submitBtn = createPlateForm.querySelector('[type=submit]');

    if (createPlateBtn)
        createPlateBtn.addEventListener('click', () => createPlateModal.show());

    createPlateForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        try {
            submitBtn.setAttribute('disabled', 'disabled');
            const data = new FormData(e.target);
            
            await createPlate({
                registration: data.get('reg'),
                purchasePrice: parseInt(data.get('pp')),
                salePrice: parseInt(data.get('sp')),
                reserved: false,
                forSale: true
            });
        } catch(e) {
            console.error('Error saving plate', e);
        } finally {
            submitBtn.removeAttribute('disabled');
            createPlateModal.hide();
        }
    });

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
