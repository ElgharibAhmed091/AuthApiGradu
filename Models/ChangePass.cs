namespace AuthenticationFirst.Models
{
    public class ChangePass
    {
        public int Id { get; set; }
        public string Email  { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
  