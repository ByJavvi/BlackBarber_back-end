namespace BlackBarberAPI.DTOs
{
    public class EmailDTO
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
