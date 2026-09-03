document.addEventListener('DOMContentLoaded', () => {
    const columns = document.querySelectorAll('.kanban-column');
    if (!columns.length) return;

    updateAllEmptyStates();

    columns.forEach(column => {
        new Sortable(column, {
            group: 'kanban-board',
            animation: 200,
            ghostClass: 'sortable-ghost',
            chosenClass: 'sortable-chosen',
            dragClass: 'sortable-drag',
            easing: 'cubic-bezier(0.2, 1, 0.1, 1)',
            emptyInsertThreshold: 25,

            onStart: () => {
                document.body.classList.add('is-dragging');
            },

            onEnd: async (evt) => {
                document.body.classList.remove('is-dragging');

                const itemEl = evt.item;
                const targetColumn = evt.to;
                const sourceColumn = evt.from;

                // Exit if dropped at the exact same location
                if (targetColumn === sourceColumn && evt.oldIndex === evt.newIndex) {
                    return;
                }

                const taskId = itemEl.getAttribute('data-id');
                const userId = itemEl.getAttribute('data-user-id');
                const newStatus = targetColumn.getAttribute('data-status');
                const archiveForm = itemEl.querySelector('.archive-form');

                if (!taskId || !newStatus) return;

                // 1. Optimistic UI update
                updateBadgeCount(sourceColumn, -1);
                updateBadgeCount(targetColumn, 1);
                updateAllEmptyStates();

                // Toggle archive icon visibility based on new status
                if (archiveForm) {
                    if (newStatus === 'Completed') {
                        archiveForm.classList.remove('d-none');
                    } else {
                        archiveForm.classList.add('d-none');
                    }
                }

                // 2. Prepare payload compatible with MVC model binding
                const formData = new FormData();
                formData.append('id', taskId);
                formData.append('status', newStatus);
                if (userId) {
                    formData.append('userId', userId);
                }

                const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

                try {
                    const response = await fetch('/ToDo/UpdateStatus', {
                        method: 'POST',
                        headers: {
                            ...(csrfToken ? { 'RequestVerificationToken': csrfToken } : {})
                        },
                        body: formData
                    });

                    const data = await response.json();

                    if (!response.ok) {
                        throw new Error(data.message || `Server returned status ${response.status}`);
                    }
                } catch (error) {
                    console.error('Failed to update task status:', error);

                    // Revert UI on failure
                    if (evt.oldIndex !== undefined) {
                        sourceColumn.insertBefore(itemEl, sourceColumn.children[evt.oldIndex] || null);
                    } else {
                        sourceColumn.appendChild(itemEl);
                    }

                    // Restore previous archive button visibility
                    if (archiveForm) {
                        const oldStatus = sourceColumn.getAttribute('data-status');
                        if (oldStatus === 'Completed') {
                            archiveForm.classList.remove('d-none');
                        } else {
                            archiveForm.classList.add('d-none');
                        }
                    }

                    updateBadgeCount(sourceColumn, 1);
                    updateBadgeCount(targetColumn, -1);
                    updateAllEmptyStates();

                    alert(error.message || 'Failed to update task status.');
                }
            }
        });
    });

    function updateBadgeCount(columnEl, delta) {
        const headerBadge = columnEl.closest('.col-xl-4, .col-lg-4')?.querySelector('.badge');
        if (!headerBadge) return;

        const currentVal = parseInt(headerBadge.innerText.trim(), 10) || 0;
        headerBadge.innerText = Math.max(0, currentVal + delta);
    }

    function updateAllEmptyStates() {
        columns.forEach(col => {
            const cards = col.querySelectorAll('.kanban-card');
            let placeholder = col.querySelector('.kanban-empty-placeholder');

            if (cards.length === 0) {
                if (!placeholder) {
                    placeholder = document.createElement('div');
                    placeholder.className = 'kanban-empty-placeholder text-center text-muted small py-5';
                    placeholder.innerHTML = `
                        <i class="bi bi-inbox d-block fs-3 opacity-25 mb-1"></i>
                        <span>No tasks in this lane</span>
                    `;
                    col.appendChild(placeholder);
                }
            } else if (placeholder) {
                placeholder.remove();
            }
        });
    }
});