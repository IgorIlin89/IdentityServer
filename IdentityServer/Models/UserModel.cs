namespace IdentityServer.Models;

public class UserModel
{
    public int? UserId { get; set; }
    public string EMail { get; set; }
    public string GivenName { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public int PostalCode { get; set; }
    public string? Password { get; set; }
}
