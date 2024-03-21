using Task2.HttpModels.Data;

namespace Task2.HttpModels;

public sealed class CreateRequest
{
    public List<DistrictData> Districts { get; set; } = new();
    public List<EstateTypeData> EstateTypes { get; set; } = new();
    public List<MaterialData> Materials { get; set; } = new();
    public List<ScoreCriteriaData> Criterias { get; set; } = new();
    public List<EstateData> Estates { get; set; } = new();
    public List<RealtorData> Realtors { get; set; } = new();
}
