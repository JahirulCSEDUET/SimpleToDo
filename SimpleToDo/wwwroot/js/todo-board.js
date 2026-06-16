document.addEventListener('DOMContentLoaded', function () {
    const columns = ['Pending', 'Processing', 'Completed'];

    // 1. Initialize Sortable columns
    columns.forEach(colId => {
        const el = document.getElementById(colId);
        if (el) {
            new Sortable(el, {
                group: 'todo-board',
                animation: 180,
                ghostClass: 'sortable-ghost',
                handle: '.drag-handle', 
                fallbackTolerance: 3,

                onEnd: function (evt) {
                    if (evt.from === evt.to) return;
                    const itemEl = evt.item;
                    const taskId = itemEl.getAttribute('data-id');
                    const newStatus = evt.to.getAttribute('data-status');
                    const oldColumn = evt.from;

                    updateTaskStatus(taskId, newStatus, oldColumn, itemEl);
                }
            });
        }
    });

    // 2. Post status transition updates asynchronously to the backend controller
    function updateTaskStatus(taskId, newStatus, oldColumn, itemElement) {
        let formData = new FormData();
        formData.append('id', taskId);
        formData.append('status', newStatus);

        fetch('/ToDo/UpdateStatus', {
            method: 'POST',
            body: formData
        })
        .then(response => {
            if (response.ok || response.redirected) {
                // Dynamically toggle the visibility of the archive button based on the target column status
                const archiveForm = itemElement.querySelector('.archive-form');
                if (archiveForm) {
                    if (newStatus === 'Completed') {
                        archiveForm.classList.remove('d-none');
                    } else {
                        archiveForm.classList.add('d-none');
                    }
                }
            } else {
                alert('Error syncing pipeline status modification back to server logs.');
                oldColumn.appendChild(itemElement);
            }
        })
        .catch(error => {
            console.error('Network Error:', error);
            oldColumn.appendChild(itemElement);
        });
    }
});