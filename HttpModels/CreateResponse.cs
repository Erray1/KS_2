namespace Task2.HttpModels;

public sealed class CreateResponse
{
    public bool IsSuccesful { get; set; } = true;
    public string? Error { get; set; }
}
