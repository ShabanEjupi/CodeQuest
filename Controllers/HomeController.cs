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
    public IActionResult Features() => View();

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult Careers() => View();

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult About() => View();

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult Faq() => View();

    [Microsoft.AspNetCore.OutputCaching.OutputCache(Duration = 3600)]
    public IActionResult Pricing() => View();

    public IActionResult Contact() => View();

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
            Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue(new Microsoft.AspNetCore.Localization.RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        return LocalRedirect(returnUrl ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> SendContactMessage(string Name, string Email, string Message)
    {
        var isSq = System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "sq";

        await _emailService.SendContactMessageAsync(Name, Email, Message, isSq);

        string subject = isSq ? "Faleminderit që kontaktuat KOSOVAPOS" : "Thank you for contacting KOSOVAPOS";
        string body = isSq 
            ? $"Përshëndetje {Name},<br/><br/>Kemi pranuar mesazhin tuaj dhe do t'ju përgjigjemi së shpejti.<br/><br/>Të falat,<br/>Ekipi i KOSOVAPOS" 
            : $"Hi {Name},<br/><br/>We have received your message and will get back to you shortly.<br/><br/>Best regards,<br/>KOSOVAPOS Team";

        await _emailService.SendNoReplyEmailAsync(Email, subject, body, isSq);

        TempData["SuccessMessage"] = isSq ? "Mesazhi juaj u dërgua me sukses!" : "Your message has been sent successfully!";
        return RedirectToAction("Contact");
    }

    [HttpPost]
    public IActionResult Start(StartGameViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.PlayerName))
            return RedirectToAction("CodeQuest");

        TempData["PlayerName"] = vm.PlayerName.Trim();
        TempData["Language"] = vm.Language;
        TempData["GameType"] = vm.GameType;
        return RedirectToAction("Begin", "Game");
    }

    public IActionResult Error() => View();
}
