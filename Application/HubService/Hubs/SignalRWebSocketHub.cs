using Common.Dto;
using Microsoft.AspNetCore.SignalR;

namespace HubService.Hubs
{
    public class SignalRWebSocketHub : Hub
    {
        public async Task AddToRestaurantGroup(string groupName)
        {
            // Get the connection id
            var cId = Context.ConnectionId;
            AddCidToGroup(cId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            // Get the connection id
            var cId = Context.ConnectionId;
            // Check weather its a existing group name..
            RemoveCidFromGroup(cId, groupName);
        }

        public async Task SendErrorResponseToClient(string jsonGenericErrorResponse)
        {
            await Clients.All.SendAsync("ReceiveError", jsonGenericErrorResponse);
        }

        public async Task SendNewOrderToRestaurant(string jsonNewOrder)
        {
            await Clients.Groups("Restaurant").SendAsync("ReceiveNewOrder", jsonNewOrder);
        }

        /// <summary>
        /// Method for adding a connection id to group
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="groupName"></param>
        public async void AddCidToGroup(string connectionId, string groupName)
        {
            HashSet<string> connections;

            if (!Cache.Cache.Groups.TryGetValue(groupName, out _))
            {
                connections = new HashSet<string>();
                Cache.Cache.Groups.TryAdd(groupName, connections);
            }

            // Only add to group if unique 
            var groupConnections = Cache.Cache.Groups[groupName];
            var isExist = groupConnections.TryGetValue(connectionId, out _);

            if (!isExist)
            {
                // Add to the particular group
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                // Add to the dict managing the connections
                groupConnections.Add(connectionId);
            }
        }

        /// <summary>
        /// Method for removing a connection id from group
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="groupName"></param>
        public async void RemoveCidFromGroup(string connectionId, string groupName)
        {
            HashSet<string> connections;

            if (!Cache.Cache.Groups.TryGetValue(groupName, out _))
            {
                return;
            }

            // Only remove if exists....
            var groupConnections = Cache.Cache.Groups[groupName];
            var isExist = groupConnections.TryGetValue(connectionId, out _);

            if (isExist)
            {
                // Remove from the particular group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                // Add to the dict managing connections
                groupConnections.Remove(connectionId);
            }
        }
        /// <summary>
        /// Method for finding all connections by groupName
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public IEnumerable<string> GetConnectionsByGroupName(string groupName)
        {
            HashSet<string> connections;

            if (Cache.Cache.Groups.TryGetValue(groupName, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

    }
}
