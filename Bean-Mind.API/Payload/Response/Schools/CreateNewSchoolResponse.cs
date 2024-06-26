﻿namespace Bean_Mind.API.Payload.Response.Schools
{
    public class CreateNewSchoolResponse
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public bool? DelFlg {  get; set; } 
    }
}
