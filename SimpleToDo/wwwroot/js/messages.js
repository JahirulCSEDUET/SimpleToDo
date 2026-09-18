document.addEventListener('DOMContentLoaded', function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .withAutomaticReconnect()
        .build();

    connection.start().catch(err => console.error("SignalR Connection Error: ", err));

    connection.on("ReceiveNewMessage", function (chatId) {
        const chatBox = document.getElementById(`fb-chat-box-${chatId}`);
        if (chatBox) {
            fetchChatMessages(chatId);
        }
    });
    connection.on("UpdateMessageBadge", function (chatId) {
        if (typeof loadUserChats === "function") {
            loadUserChats();
        }

        const openChatBody = document.getElementById(`fb-chat-body-${chatId}`);
        if (openChatBody && typeof fetchChatMessages === "function") {
            fetchChatMessages(chatId);
        }
    });

    const messageBadge = document.getElementById('message-badge');
    const messageHeaderCount = document.getElementById('message-header-count');
    const messageItemsList = document.getElementById('message-items-list');
    const dropdownToggle = document.getElementById('fbMessageDropdown');

    function loadUserChats() {
        fetch('/Message/GetUserChats')
            .then(res => {
                if (!res.ok) throw new Error(`HTTP Error: ${res.status}`);
                return res.json();
            })
            .then(data => {
                const totalUnread = data.totalUnread ?? data.TotalUnread ?? 0;
                const chats = data.chats ?? data.Chats ?? [];

                if (messageBadge) {
                    if (totalUnread > 0) {
                        messageBadge.innerText = totalUnread > 99 ? '99+' : totalUnread;
                        messageBadge.classList.remove('d-none');
                    } else {
                        messageBadge.classList.add('d-none');
                    }
                }

                if (messageHeaderCount) {
                    messageHeaderCount.innerText = `${totalUnread} Unread`;
                }

                if (!chats || chats.length === 0) {
                    messageItemsList.innerHTML = `
                        <div class="text-center py-4 text-muted small">
                            <i class="bi bi-chat-dots mb-2 fs-3 d-block opacity-50"></i>
                            No active project chats found.
                        </div>`;
                    return;
                }

                let html = '';
                chats.forEach(chat => {
                    const chatId = chat.chatId ?? chat.ChatId;
                    const chatName = chat.chatName ?? chat.ChatName ?? 'Project Chat';
                    const unreadCount = chat.unreadCount ?? chat.UnreadCount ?? 0;
                    const lastMsg = chat.lastMessage ?? chat.LastMessage ?? 'No messages yet...';
                    const time = chat.lastMessageTime ?? chat.LastMessageTime ?? '';

                    const badgeHtml = unreadCount > 0
                        ? `<span class="badge rounded-pill px-2 py-1" style="background:var(--brand); color:#fff;">${unreadCount}</span>`
                        : '';

                    html += `
                        <div class="fb-dropdown-item p-3 border-bottom" onclick="openFloatingChat(${chatId}, '${escapeHtml(chatName)}')">
                            <div class="d-flex align-items-center justify-content-between gap-2">
                                <div class="d-flex align-items-center gap-2.5 text-truncate">
                                    <div class="avatar-circle-sm rounded-circle d-flex align-items-center justify-content-center fw-bold mx-2" style="background:var(--brand-soft); color:var(--brand);">
                                        <i class="bi bi-people-fill"></i>
                                    </div>
                                    <div class="text-truncate">
                                        <h6 class="mb-0 fw-bold text-dark text-truncate" style="font-size: 0.85rem;">${escapeHtml(chatName)}</h6>
                                        <div class="text-muted small text-truncate line-clamp-1" style="font-size: 0.76rem;">${escapeHtml(lastMsg)}</div>
                                    </div>
                                </div>
                                <div class="d-flex flex-column align-items-end gap-1 flex-shrink-0">
                                    <span class="text-muted-xs">${time}</span>
                                    ${badgeHtml}
                                </div>
                            </div>
                        </div>`;
                });

                messageItemsList.innerHTML = html;
            })
            .catch(err => {
                console.error('Failed to load user chats:', err);
                if (messageItemsList) {
                    messageItemsList.innerHTML = `
                        <div class="text-center py-4 text-danger small">
                            <i class="bi bi-exclamation-circle mb-1 d-block fs-4"></i>
                            Unable to load chats. Check server logs.
                        </div>`;
                }
            });
    }

    loadUserChats();
    if (dropdownToggle) {
        dropdownToggle.addEventListener('show.bs.dropdown', loadUserChats);
    }
});

const chatPollingIntervals = {};

function openFloatingChat(chatId, chatTitle) {
    const dock = document.getElementById('fb-chat-dock');
    if (!dock) return;

    const existingBox = document.getElementById(`fb-chat-box-${chatId}`);
    if (existingBox) {
        existingBox.querySelector('input')?.focus();
        return;
    }

    const chatBox = document.createElement('div');
    chatBox.className = 'fb-chat-box';
    chatBox.id = `fb-chat-box-${chatId}`;
    chatBox.innerHTML = `
        <div class="fb-chat-header d-flex align-items-center justify-content-between">
            <div class="d-flex align-items-center gap-2 text-truncate">
                <span class="d-inline-block bg-success rounded-circle" style="width: 8px; height: 8px;"></span>
                <span class="fw-bold small text-truncate" style="max-width: 190px;">${escapeHtml(chatTitle)}</span>
            </div>
            <button type="button" class="btn-close btn-close-white btn-sm shadow-none" onclick="closeFloatingChat(${chatId})"></button>
        </div>
        <div class="fb-chat-body" id="fb-chat-body-${chatId}">
            <div class="text-center py-4 text-muted small">
                <div class="spinner-border spinner-border-sm text-primary"></div>
            </div>
        </div>
        <div class="fb-chat-footer">
            <form onsubmit="sendFloatingMessage(event, ${chatId})" class="d-flex align-items-center gap-1.5 m-0">
                <input type="text" id="fb-input-${chatId}" class="form-control form-control-sm rounded-pill shadow-none px-3" placeholder="Type a message..." autocomplete="off" required />
                <button type="submit" class="btn btn-sm btn-primary rounded-circle p-0 d-flex align-items-center justify-content-center" style="width: 32px; height: 32px;">
                    <i class="bi bi-send-fill" style="font-size: 0.75rem;"></i>
                </button>
            </form>
        </div>
    `;

    dock.appendChild(chatBox);
    fetchChatMessages(chatId);

    if (!chatPollingIntervals[chatId]) {
        chatPollingIntervals[chatId] = setInterval(() => {
            fetchChatMessages(chatId);
        }, 3500);
    }
}

function fetchChatMessages(chatId) {
    const bodyEl = document.getElementById(`fb-chat-body-${chatId}`);
    if (!bodyEl) return;

    fetch(`/Message/GetChatFeed?chatId=${chatId}`)
        .then(async res => {
            if (!res.ok) {
                const errorText = await res.text();
                throw new Error(errorText);
            }
            return res.text();
        })
        .then(html => {
            bodyEl.innerHTML = html;
            bodyEl.scrollTop = bodyEl.scrollHeight;
        })
        .catch(err => {
            console.error('Chat feed error:', err);
            bodyEl.innerHTML = `
                <div class="text-center py-4 text-danger small px-3">
                    <i class="bi bi-exclamation-triangle mb-2 d-block fs-3"></i>
                    <strong>Error Details:</strong><br/>
                    ${err.message}
                </div>`;
        });
}

function sendFloatingMessage(event, chatId) {
    event.preventDefault();
    const input = document.getElementById(`fb-input-${chatId}`);
    const messageText = input.value.trim();

    if (!messageText) return;

    fetch('/Message/SendChatMessage', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ chatId: chatId, body: messageText })
    })
        .then(res => {
            if (!res.ok) throw new Error('Failed to send message');
            return res.json();
        })
        .then(() => {
            input.value = '';
            fetchChatMessages(chatId);
        })
        .catch(err => console.error('Error sending message:', err));
}

function closeFloatingChat(chatId) {
    if (chatPollingIntervals[chatId]) {
        clearInterval(chatPollingIntervals[chatId]);
        delete chatPollingIntervals[chatId];
    }

    const box = document.getElementById(`fb-chat-box-${chatId}`);
    if (box) box.remove();
}

function escapeHtml(str) {
    if (!str) return '';
    return str
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}