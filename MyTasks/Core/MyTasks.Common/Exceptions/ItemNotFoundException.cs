namespace MyTasks.Common.Exceptions
{
    [Serializable]
    public class ItemNotFoundException : Exception
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ItemNotFoundException(int id, string name) : base($"{name} with given id ({id}) was not found")
        {
            Id = id;
            Name = name; 
        }
    }
}
