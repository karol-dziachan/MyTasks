using MyTasks.Domain.Common;

namespace MyTasks.Domain.Entities
{
    public class UsersTasks : AuditableEntity
    {
        public string UserName { get; set; }
        public string TaskId { get; set; }
    }
}
