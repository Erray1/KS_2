namespace Task2.HttpModels.Data;

public sealed class SaleDataForEstate
{
    public required string RealtorFCs { get; set; }
    public required DateOnly Date { get; set; }
    public required decimal Price { get; set; }
}