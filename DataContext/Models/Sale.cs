using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataContext.Models;

public sealed class Sale : ApplicationEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public Estate Estate { get; set; } = null!;
    public DateOnly SaleDate { get; set; }
    public Realtor Realtor { get; set; } = null!;
    public decimal Price { get; set; }
}
