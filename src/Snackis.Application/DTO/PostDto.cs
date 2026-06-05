namespace Snackis.Application.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // är empty för att undvika null-värden när man går till azure
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
