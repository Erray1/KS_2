namespace Task2.HttpModels.Data;

public sealed class EstateData
{
    public required string District { get; set; }
    public required string Address { get; set; }
    public required int Floor { get; set; }
    public required int RoomsNumber { get; set; }
    public required string Type { get; set; }
    public required float Area { get; set; }
    public SaleDataForEstate? Sale { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public required List<string> Materials { get; set; }
    public required List<ScoreData> Scores { get; set; }
    public required DateOnly DatePosted { get; set; }
}
