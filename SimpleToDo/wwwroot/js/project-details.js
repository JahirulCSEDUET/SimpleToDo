document.addEventListener('DOMContentLoaded', function () {
    const lanes = ['Pending', 'Processing', 'Completed'];

    // 1. Initialize Drag-and-Drop Lifecycle Interfaces
    lanes.forEach(status => {
        const laneEl = document.getElementById(`lane-${status}`);
        if (laneEl) {
            new Sortable(laneEl, {
                group: 'project-kanban-board',
                animation: 180,
                ghostClass: 'sortable-ghost',
                fallbackTolerance: 3,

                onEnd: function (evt) {
                    if (evt.from === evt.to) return;

                    const cardElement = evt.item;
                    const taskId = cardElement.getAttribute('data-task-id');
                    const targetStatus = evt.to.getAttribute('data-status');
                    const todoUserId = cardElement.getAttribute('data-user-id');
                    const originalLane = evt.from;

                    executeDragStatusUpdate(taskId, targetStatus, todoUserId, originalLane, cardElement);
                }
            });
        }
    });

    // 2. Client-Side Realtime Assignee Query Filtering
    const boardFilter = document.getElementById('member-board-filter');
    if (boardFilter) {
        boardFilter.addEventListener('change', function () {
            const selectedValue = this.value;
            const allCards = document.querySelectorAll('.project-task-card');

            allCards.forEach(card => {
                const cardUserId = card.getAttribute('data-user-id');

                if (selectedValue === 'all') {
                    card.classList.remove('d-none');
                }
                else if (selectedValue === '0') {
                    if (!cardUserId || cardUserId === '0' || cardUserId.trim() === '') {
                        card.classList.remove('d-none');
                    }
                    else {
                        card.classList.add('d-none');
                    }
                }
                else {
                    if (cardUserId === selectedValue) {
                        card.classList.remove('d-none');
                    }
                    else {
                        card.classList.add('d-none');
                    }
                }
            });
        });
    }

    // 3. Database Sync Pipeline 
    function executeDragStatusUpdate(taskId, newStatus, todoUserId, fallbackLane, cardElement) {
        let formData = new FormData();
        formData.append('id', taskId);
        formData.append('status', newStatus);
        formData.append('userId', todoUserId);

        fetch('/ToDo/UpdateStatus', {
            method: 'POST',
            body: formData
        })
            .then(async response => {
                if (response.ok) {
                    return response.json();
                }
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || 'An unexpected pipeline error occurred.');
            })
            .then(data => {
                showLiveAlert(data.message || 'Task status updated!', 'success');

                // Dynamic theme shift tracking tokens
                cardElement.classList.remove('card-status-todo', 'card-status-progress', 'card-status-done');
                if (newStatus === 'Pending') {
                    cardElement.classList.add('card-status-todo');
                } else if (newStatus === 'Processing') {
                    cardElement.classList.add('card-status-progress');
                } else if (newStatus === 'Completed') {
                    cardElement.classList.add('card-status-done');
                }
            })
            .catch(error => {
                showLiveAlert(error.message, 'danger');
                fallbackLane.appendChild(cardElement);
            });
    }

    // 4. Premium In-App Notification Alerts Frame Component
    function showLiveAlert(message, type) {
        const container = document.getElementById('kanban-live-alert');
        if (!container) return;

        const icon = type === 'success' ? 'bi-check-circle-fill' : 'bi-exclamation-triangle-fill';

        container.innerHTML = `
            <div class="alert alert-${type} shadow-lg border-0 rounded-4 p-3 d-flex align-items-center gap-2 animate-fade-in">
                <i class="bi ${icon} fs-5 text-${type}"></i>
                <div class="fw-medium text-dark small flex-grow-1">${message}</div>
                <button type="button" class="btn-close small shadow-none ms-2" onclick="this.parentElement.parentElement.classList.add('d-none')" aria-label="Close"></button>
            </div>
        `;

        container.classList.remove('d-none');

        setTimeout(() => {
            container.classList.add('d-none');
        }, 3500);
    }
});