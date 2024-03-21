using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataContext.Models;

public sealed class Score : ApplicationEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public DateOnly ScoreDate { get; set; }
    public ScoreCriteria Criteria { get; set; } = null!;
    public int EstateID { get; set; }
    public int Value { get; set; }
}
