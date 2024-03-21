namespace Task2.DataContext.Utilities;

public static class EstateScoreMapper
{
    public static string ToStringRepresentation(int scorePercents)
    {
        if (scorePercents <= 100 && scorePercents > 90) return "превосходно";
        else if (scorePercents <= 90 && scorePercents > 80) return "очень хорошо";
        else if (scorePercents <= 80 && scorePercents > 70) return "хорошо";
        else if (scorePercents <= 70 && scorePercents > 60) return "удовлетворительно";
        else return "неудовлетворительно";
    }
}
