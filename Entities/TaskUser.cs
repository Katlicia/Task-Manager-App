namespace TodoApp.Entities
{
    public class TaskUser
    {
        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public UserAccount User { get; set; }
        public bool CanEdit { get; internal set; }
    }
}
