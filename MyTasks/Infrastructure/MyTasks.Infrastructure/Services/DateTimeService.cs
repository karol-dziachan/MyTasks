using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
