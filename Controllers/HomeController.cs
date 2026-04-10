using CodeQuest.Models;
using CodeQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeQuest.Controllers;

public class HomeController : Controller
{
    private readonly ILeaderboardService _leaderboard;

    public HomeController(ILeaderboardService leaderboard) =>
        _leaderboard = leaderboard;

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> CodeQuest()
    {
        var top = await _leaderboard.GetTopAsync(10);
        return View(top);
    }

    public IActionResult Services() => View();
    public IActionResult About() => View();
    public IActionResult Contact() => View();

    [HttpPost]
    public IActionResult Start(StartGameViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.PlayerName))
            return RedirectToAction("CodeQuest");

        TempData["PlayerName"] = vm.PlayerName.Trim();
        TempData["Language"] = vm.Language;
        return RedirectToAction("Begin", "Game");
    }

    public IActionResult Error() => View();
}
