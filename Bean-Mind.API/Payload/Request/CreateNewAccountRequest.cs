﻿using Bean_Mind_Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request
{
    public class CreateNewAccountRequest
    {
        [Required(ErrorMessage = "Username is missing")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is missing")]
        public RoleEnum Role { get; set; }

        public CreateNewAccountRequest()
        {

        }
    }
}
