namespace MyTasks.Common.Exceptions
{
    [Serializable]
    public class UserIsNotOwnerException : Exception
    {
        public UserIsNotOwnerException() : base("Only owner can do this operation")
        {

        }
    }
}
