using CodeQuest.Models;
using CodeQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeQuest.Controllers;

public class HomeController : Controller
{
    private readonly ILeaderboardService _leaderboard;

    public HomeController(ILeaderboardService leaderboard) =>
        _leaderboard = leaderboard;

    public async Task<IActionResult> Index()
    {
        var top = await _leaderboard.GetTopAsync(10);
        return View(top);
    }

    [HttpPost]
    public IActionResult Start(StartGameViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.PlayerName))
            return RedirectToAction("Index");

        TempData["PlayerName"] = vm.PlayerName.Trim();
        TempData["Language"] = vm.Language;
        return RedirectToAction("Begin", "Game");
    }

    public IActionResult Error() => View();
}
