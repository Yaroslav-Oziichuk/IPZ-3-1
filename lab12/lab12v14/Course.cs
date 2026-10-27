namespace lab12v14
{
    public class Course : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId { get; set; }
    }
}
