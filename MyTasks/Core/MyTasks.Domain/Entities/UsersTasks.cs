using MyTasks.Domain.Common;

namespace MyTasks.Domain.Entities
{
    public class UsersTasks : AuditableEntity
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
    }
}
