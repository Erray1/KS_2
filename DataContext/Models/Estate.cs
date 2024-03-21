using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataContext.Models;

public sealed class Estate : ApplicationEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public District District { get; set; } = null!;
    public string Address { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int RoomsNumber { get; set; }
    public EstateType Type { get; set; } = null!;
    public int Status { get; set; }
    public Sale? Sale { get; set; }
    public int? SaleID { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ICollection<Material> Materials { get; set; } = new List<Material>();
    public ICollection<Score> Scores { get; set; } = new List<Score>();
    public float Area { get; set; }
    public DateOnly DatePosted { get; set; }
}