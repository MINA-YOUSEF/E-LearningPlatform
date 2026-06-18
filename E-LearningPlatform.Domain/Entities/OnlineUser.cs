// OnlineUser.cs

namespace E_LearningPlatform.Domain.Entities
{
    public class OnlineUser : BaseEntity
    {
        private OnlineUser () { }

     public OnlineUser (
        int userId,
        string connectionId)
        {
            UserId = userId;

            ConnectionId = connectionId;

            ConnectedAtUtc = DateTime.UtcNow;

            IsOnline = true;
        }

        public int UserId { get; private set; }

        public string ConnectionId { get; private set; } = null!;

        public bool IsOnline { get; private set; }

        public DateTime ConnectedAtUtc { get; private set; }

        public DateTime? LastSeenUtc { get; private set; }

        public void Disconnect ()
        {
            IsOnline = false;

            LastSeenUtc = DateTime.UtcNow;
        }

        public void Reconnect (string connectionId)
        {
            ConnectionId = connectionId;

            IsOnline = true;

            ConnectedAtUtc = DateTime.UtcNow;

            LastSeenUtc = null;
        }
    }
 }
