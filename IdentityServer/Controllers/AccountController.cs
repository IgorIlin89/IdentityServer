using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Application.Commands;
using IdentityServer.Application.Handlers.Interfaces;
using IdentityServer.Database.Interfaces;
using IdentityServer.Domain;
using IdentityServer.DtosAndModels;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAddUserCommandHandler _addUserCommandHandler;

    public AccountController(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ICustomerRepository customerRepository,
        IAddUserCommandHandler addUserCommandHandler)
    {
        _customerRepository = customerRepository;
        _interaction = interaction;
        _events = events;
        _addUserCommandHandler = addUserCommandHandler;
    }

    [HttpGet]
    public IActionResult Login([FromQuery] string returnUrl)
    {
        return View(new LoginModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpGet]
    public IActionResult Forbidden()
    {
        return Unauthorized();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn([FromForm] LoginModel model)
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        // the user clicked the "cancel" button
        if (model.LoginCancelation)
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (IsNativeClient(context))
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("~/Views/Login/Redirect.cshtml", new RedirectModel
                    {
                        RedirectUri = model.ReturnUrl
                    });
                }

                return Redirect(model.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if (ModelState.IsValid)
        {
            // validate username/password
            var customers = await _customerRepository.GetUserListAsync(CancellationToken.None);
            var customer = customers.FirstOrDefault(x => x.EMail == model.UserName);

            if (customer != null && customer.Password == model.Password)
            {
                await _events.RaiseAsync(new UserLoginSuccessEvent(customer.EMail, customer.EMail, customer.EMail,
                    clientId: context?.Client.ClientId));

                // only set explicit expiration here if user chooses "remember me". 
                // otherwise we rely upon expiration configured in cookie middleware.
                AuthenticationProperties props = null;
                //if (LoginOptions.AllowRememberLogin && model.RememberLogin)
                //{
                //    props = new AuthenticationProperties
                //    {
                //        IsPersistent = true,
                //        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
                //    };
                //};

                var claims = new[] {
                    new Claim(ClaimTypes.Name, customer.GivenName),
                };

                // issue authentication cookie with subject ID and username
                var isuser = new IdentityServerUser(customer.Id.ToString())
                {
                    DisplayName = customer.EMail,
                    AdditionalClaims = claims
                };

                await HttpContext.SignInAsync(isuser, props);

                if (context != null)
                {
                    if (IsNativeClient(context))
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return View("~/Views/Login/Redirect.cshtml", new RedirectModel
                        {
                            RedirectUri = model.ReturnUrl
                        });
                    }

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(model.ReturnUrl);
                }

                // request for a local page
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect("~/");
                }
                else
                {
                    // user might have clicked on a malicious link - should be logged
                    //TODO throw new Exception("invalid return URL");
                    return View("~/Views/Login/Redirect.cshtml", new RedirectModel
                    {
                        RedirectUri = model.ReturnUrl
                    });
                }
            }

            //await _events.RaiseAsync(new UserLoginFailureEvent(model.UserName, "invalid credentials", clientId: context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, "Invalid username or password");
        }

        await _events.RaiseAsync(new UserLoginFailureEvent(model.UserName, "invalid credentials", clientId: context?.Client.ClientId));

        return View("~/Views/Account/Login.cshtml", model);
    }

    protected async Task SignIn(User customer)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, customer.GivenName),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = true,
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();// CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("https://localhost:7195");
    }

    /// <summary>
    /// Checks if the redirect URI is for a native client.
    /// </summary>
    private bool IsNativeClient(AuthorizationRequest context)
    {
        return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
&& !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationModel model,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            if (model.Password == model.RepeatPassword)
            {
                var userToAdd = new User
                {
                    EMail = model.EMail,
                    Password = model.Password.Trim(),
                    GivenName = model.FirstName,
                    Surname = model.LastName,
                    Age = model.Age,
                    Country = model.Country,
                    City = model.City,
                    Street = model.Street,
                    HouseNumber = model.HouseNumber,
                    PostalCode = model.PostalCode
                };

                var command = new AddUserCommand(model.EMail, model.FirstName, model.LastName, model.Age,
                    model.Country, model.City, model.Street, model.HouseNumber, model.PostalCode, model.Password);

                var userToLogin = await _addUserCommandHandler.HandleAsync(command, cancellationToken);

                await SignIn(userToLogin);

                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("Model", "The repeated Password was not the same");
                return View(model);
            }
        }

        return View(model);
    }
}