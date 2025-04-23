﻿using SQLite;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } 
}
