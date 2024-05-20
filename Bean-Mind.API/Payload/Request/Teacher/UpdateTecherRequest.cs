﻿namespace Bean_Mind.API.Payload.Request.Teacher
{
    public class UpdateTecherRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
       
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public void TrimString()
        {
            FirstName = FirstName?.Trim();
            
        }
    }
}
