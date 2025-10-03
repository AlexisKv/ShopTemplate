using Microsoft.AspNetCore.SignalR;

namespace ShopTemplate.Hubs;

public class CartHub : Hub
{
    public async Task JoinCartGroup(Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
    }

    public async Task LeaveCartGroup(Guid userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
    }
}