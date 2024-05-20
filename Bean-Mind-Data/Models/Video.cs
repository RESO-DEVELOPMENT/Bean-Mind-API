using System;
using System.Collections.Generic;

namespace Bean_Mind_Data.Models;

public partial class Video
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? VideoUrl { get; set; }

    public DateTime? UploadDate { get; set; }
}
