﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataContext.Models;

public sealed class ScoreCriteria : ApplicationEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string Name { get; set; }
}
