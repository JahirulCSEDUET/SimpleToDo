
namespace SimpleToDo.Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public Project Project { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
