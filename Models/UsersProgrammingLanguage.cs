using System;
using System.Collections.Generic;

namespace AI_Programming_Assistant.Models;

public partial class UsersProgrammingLanguage
{
    public int UserId { get; set; }

    public int LanguageId { get; set; }

    public int? UserLevel { get; set; }

    public virtual ProgrammingLanguage Language { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
