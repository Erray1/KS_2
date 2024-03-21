using Task2.DataContext.Models;

namespace Task2.DataContext.Utilities;

public static class PriceCalculator
{
    public static decimal GetPricePerSquareMeter(Estate estate)
    {
        return estate.Price / (decimal)estate.Area;
    }
    public static decimal GetAveragePricePerSquareMeter(IEnumerable<Estate> estates)
    {
        return estates.Select(x => GetPricePerSquareMeter(x)).Average();
    }
}