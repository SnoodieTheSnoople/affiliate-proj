﻿namespace affiliate_proj.Core.RequestModels;

public class UserRequest
{
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}