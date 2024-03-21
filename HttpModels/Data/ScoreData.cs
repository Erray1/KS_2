namespace Task2.HttpModels.Data;

public sealed class ScoreData
{
    public required string Criteria { get; set; }
    public required int Score { get; set; }
    public required DateOnly CreationDate { get; set; }
}
