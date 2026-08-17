namespace NetProject.Models
{
    public class User
    {

        public Guid id { get; set; } = Guid.NewGuid();

        public String Email { get; set; } = String.Empty;

        public string? GoogleSubjectId { get; set; }

        public string? PasswordHash { get; set; }

        public string AuthProvider { get; set; } = "Google";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
