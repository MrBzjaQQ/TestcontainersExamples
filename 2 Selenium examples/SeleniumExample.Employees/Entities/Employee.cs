﻿namespace SeleniumExample.Users.Entities;

public sealed record Employee(string Phone, string UserName, string Email, string Position)
{
    public Guid Id { get; init; }
}