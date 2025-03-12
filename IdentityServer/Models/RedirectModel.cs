using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models;

public class RedirectModel
{
    [Required]
    public string RedirectUri { get; set; }
}