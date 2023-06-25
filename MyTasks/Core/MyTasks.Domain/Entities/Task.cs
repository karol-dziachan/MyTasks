using MyTasks.Domain.Common;

namespace MyTasks.Domain.Entities
{
    public class Task : AuditableEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? Duration { get; set; }
        public string OwnerName { get; set; }
    }
}
