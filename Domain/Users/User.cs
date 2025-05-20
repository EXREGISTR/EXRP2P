﻿namespace Domain.Users;

public class User {
    public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
