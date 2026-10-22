using System;
using System.Collections.Generic;

namespace AI_Programming_Assistant.Models;

public partial class LearningContent
{
    public int Level { get; set; }

    public int LanguageId { get; set; }

    public string? Exam { get; set; }

    public string? Content { get; set; }

    public virtual ProgrammingLanguage Language { get; set; } = null!;
}
