﻿using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Entities.Master;

public class File : Entity
{
    [MaxLength(255)] public string UntrustedName { get; set; } = null!;

    [MaxLength(1024)] public string TrustedName { get; set; } = null!;

    [MaxLength(255)] public string ContentType { get; set; } = null!;

    public long Size { get; set; }
}