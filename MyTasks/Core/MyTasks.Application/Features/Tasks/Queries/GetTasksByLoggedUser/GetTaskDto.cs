namespace MyTasks.Application.Features.Tasks.Queries.GetTasksByLoggedUser
{
    public class GetTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? Duration { get; set; }
        public string OwnerName { get; set; }
    }
}
