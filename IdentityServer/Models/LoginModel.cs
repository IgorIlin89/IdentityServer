using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models;

public class LoginModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string ReturnUrl { get; set; }

    public bool LoginCancelation { get; set; }
}