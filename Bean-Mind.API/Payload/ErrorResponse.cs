﻿using System.Text.Json;

namespace Bean_Mind.API.Payload
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string Error { get; set; }

        public DateTime TimeStamp { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
