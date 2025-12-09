namespace San3a.Core.DTOs.Job
{
    public class CreateJobWithDirectRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Budget { get; set; }
        public string ServiceId { get; set; } = string.Empty;
        public string? DirectCraftsmanId { get; set; }
        public bool IsPublic { get; set; } = true;
    }
}
