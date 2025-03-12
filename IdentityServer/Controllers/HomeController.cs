using ApiUser.Domain.Interfaces.Handlers;
using IdentityServer.Dtos;
using IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IdentityServer.Controllers;

public class HomeController(
    IGetUserListCommandHandler getUserListCommandHandler,
    ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userList = await getUserListCommandHandler.HandleAsync(CancellationToken.None);

        return View(userList.MapToModelList());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
