namespace Blog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PostId { get; set; }
    }
}