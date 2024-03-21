using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataContext.Models;

public sealed class Realtor : ApplicationEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string Surname { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
