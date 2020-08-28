using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TestingTutor.UI.Hubs
{
    public class SubmissionHub : Hub
    {
        public class User
        {
            public string Name { get; set; }
            public HashSet<string> ConnectionIds { get; set; }
        }

        public static ConcurrentDictionary<string, User> Users
            = new ConcurrentDictionary<string, User>();

        public override Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(username, _ => new User()
            {
                Name = username,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;

            var status = Users.TryGetValue(username, out var user);

            if (status)
            {
                lock (Users)
                {
                    lock (Users[username].ConnectionIds)
                    {
                        Users[username].ConnectionIds.Remove(connectionId);
                    }

                    if (!Users[username].ConnectionIds.Any())
                    {
                        Users.Remove(username, out _);
                    }

                }


            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
