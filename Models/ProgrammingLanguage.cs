using System;
using System.Collections.Generic;

namespace AI_Programming_Assistant.Models;

public partial class ProgrammingLanguage
{
    public int LanguageId { get; set; }

    public string? LanguageName { get; set; }

    public virtual ICollection<LearningContent> LearningContents { get; set; } = new List<LearningContent>();

    public virtual ICollection<UsersProgrammingLanguage> UsersProgrammingLanguages { get; set; } = new List<UsersProgrammingLanguage>();
}
