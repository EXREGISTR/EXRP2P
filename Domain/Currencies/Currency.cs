namespace Domain.Currencies;

public class Currency {
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Precision { get; set; }
    public bool IsFiat { get; set; }

}
