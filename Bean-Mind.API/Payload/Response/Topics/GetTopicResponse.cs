﻿namespace Bean_Mind.API.Payload.Response.Topics
{
    public class GetTopicResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public GetTopicResponse(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }   
    }
}
