using CodeQuest.Models;
using CodeQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeQuest.Controllers;

public class HomeController : Controller
{
    private readonly ILeaderboardService _leaderboard;
    private readonly IEmailService _emailService;

    public HomeController(ILeaderboardService leaderboard, IEmailService emailService)
    {
        _leaderboard = leaderboard;
        _emailService = emailService;
    }

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult Index()
    {
        return View();
    }

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 60)]
    public async Task<IActionResult> CodeQuest()
    {
        var top = await _leaderboard.GetTopAsync(10);
        return View(top);
    }

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult Services() => View();

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult About() => View();

    public IActionResult Contact() => View();

    [HttpPost]
    public async Task<IActionResult> SendContactMessage(string Name, string Email, string Message)
    {
        await _emailService.SendContactMessageAsync(Name, Email, Message);
        await _emailService.SendNoReplyEmailAsync(Email, "Thank you for contacting KOSOVAPOS", 
            $"Hi {Name},<br/><br/>We have received your message and will get back to you shortly.<br/><br/>Best regards,<br/>KOSOVAPOS Team");

        TempData["SuccessMessage"] = "Your message has been sent successfully!";
        return RedirectToAction("Contact");
    }

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
