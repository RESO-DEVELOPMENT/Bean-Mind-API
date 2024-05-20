﻿namespace Bean_Mind.API.Payload.Response.Teacher
{
    public class CreateNewTeacherResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Message { get; set; }
    }
}
