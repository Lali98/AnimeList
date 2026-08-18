namespace AnimeList.Dtos.FanSub
{
    public class CreateFanSubDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public LinkDto Links { get; set; } = new();
    }
}
