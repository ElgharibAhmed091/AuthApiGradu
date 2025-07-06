namespace AuthenticationFirst.Models
{
    public class ChangePasswordModel
    {
             public int id { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
    }
 }

 