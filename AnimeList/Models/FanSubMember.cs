namespace AnimeList.Models
{
    public class FanSubMember
    {
        public int FanSubId { get; set; }
        public FanSub FanSub { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public FanSubRole Role { get; set; }
    }

    public enum FanSubRole
    {
        Owner,
        Admin,
        Editor,
        Translator
    }
}
