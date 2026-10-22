using System;
using System.Collections.Generic;

namespace AI_Programming_Assistant.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<UsersProgrammingLanguage> UsersProgrammingLanguages { get; set; } = new List<UsersProgrammingLanguage>();
}
