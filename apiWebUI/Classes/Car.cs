namespace apiWebUI.Classes;

public class Car : Common
{
    public string? Vin { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Type { get; set; }
    public string? Fuel { get; set; }
    public int OwnerId { get; set; }
    public Person Owner { get; set; }
    public string carImage { get; set; }
}