using System.Text;
using Task2.DataContext.Models;

namespace Task2.DataContext.Utilities;

public static class FCsCreator
{
    public static string GetFCsJoined(Realtor realtor)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} {1} {2}", realtor.Surname, realtor.Firstname, realtor.Lastname);
        return sb.ToString();
    }
    public static string GetFCsJoined(string surname, string firstname, string lastname)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} {1} {2}", surname, firstname, lastname) ;
        return sb.ToString();
    }
        
 }
