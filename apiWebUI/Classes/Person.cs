namespace apiWebUI.Classes;

public class Person : Common
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public List<Car> CarsOwnedId { get; set; } = new List<Car>();

}