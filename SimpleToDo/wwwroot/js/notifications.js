document.addEventListener('DOMContentLoaded', function () {
    const badge = document.getElementById('notification-badge');
    const headerCount = document.getElementById('notification-header-count');
    const itemsList = document.getElementById('notification-items-list');
    const dropdownElement = document.getElementById('fbNotificationDropdown');
    const markAllReadBtn = document.getElementById('mark-all-read-btn');

    if (!dropdownElement) return;

    function fetchNotifications() {
        fetch('/Notification/GetFeed')
            .then(res => res.ok ? res.text() : Promise.reject())
            .then(htmlContent => {
                itemsList.innerHTML = htmlContent;

                const metaBridge = document.getElementById('notification-meta-bridge');
                if (metaBridge) {
                    const count = parseInt(metaBridge.getAttribute('data-unread-count')) || 0;
                    updateBadgeCount(count);
                }
            })
            .catch(err => console.error("Error loading server-side notification snippet:", err));
    }

    function updateBadgeCount(count) {
        if (count > 0) {
            badge.innerText = count;
            badge.classList.remove('d-none');
            headerCount.innerText = `${count} New`;
        } else {
            badge.classList.add('d-none');
            headerCount.innerText = '0 New';
        }
    }

    fetchNotifications();

    if (markAllReadBtn) {
        markAllReadBtn.addEventListener('click', function (e) {
            e.stopPropagation(); 

            fetch('/Notification/MarkAllAsRead', { method: 'POST' })
                .then(res => {
                    if (res.ok) {
                        updateBadgeCount(0);
                        
                        document.querySelectorAll('.fb-unread-dot').forEach(dot => dot.remove());
                        document.querySelectorAll('.fb-notification-item').forEach(item => {
                            item.classList.remove('bg-primary-subtle', 'bg-opacity-25');
                            item.style.backgroundColor = '#fff';
                        });
                    }
                })
                .catch(err => console.error("Error updating all records context:", err));
        });
    }

    
    if (itemsList) {
        itemsList.addEventListener('click', function (e) {
            const anchorShortcut = e.target.closest('.btn-redirect-shortcut');
            if (anchorShortcut) {
                e.preventDefault(); 

                const notiId = anchorShortcut.getAttribute('data-id');
                const destinationUrl = anchorShortcut.getAttribute('href');

                
                fetch(`/Notification/MarkSingleAsRead/${notiId}`, { method: 'POST' })
                    .then(() => {
                        
                        window.location.href = destinationUrl;
                    })
                    .catch(() => {
                        
                        window.location.href = destinationUrl;
                    });
            }
        });
    }
});