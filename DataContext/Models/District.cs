using System.ComponentModel.DataAnnotations.Schema;

namespace Task2.DataContext.Models;

public sealed class District : ApplicationEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string Name { get; set; }
    public ICollection<Estate> EstatesOfDistrict { get; set; } = new List<Estate>();
}
