﻿namespace AuthenticationFirst.Models
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
