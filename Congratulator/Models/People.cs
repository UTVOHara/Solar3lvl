namespace WonnaKnow.Models
{
    public class People
    {
        public int Id { get; set; }
        public byte[]? Photo { get; set; } 
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
