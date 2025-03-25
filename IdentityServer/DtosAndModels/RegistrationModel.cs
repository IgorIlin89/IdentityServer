namespace IdentityServer.DtosAndModels;

public class RegistrationModel
{
    public string EMail { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public int PostalCode { get; set; }
}