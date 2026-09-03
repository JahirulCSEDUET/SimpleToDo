using Microsoft.AspNetCore.SignalR;

namespace SimpleToDo.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }
    }
}