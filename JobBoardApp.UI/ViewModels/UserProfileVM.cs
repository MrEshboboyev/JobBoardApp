namespace JobBoardApp.UI.ViewModels
{
    public class UserProfileVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public string? CompanyName { get; set; }
        public string? Website { get; set; }
        public string? Bio { get; set; }

        public string? OwnerName { get; set; }

        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
    }
}
