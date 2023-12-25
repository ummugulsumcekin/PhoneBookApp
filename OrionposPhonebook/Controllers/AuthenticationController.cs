using Microsoft.AspNetCore.Mvc;
using OrionposPhonebook.Models.AuthenticationModel;
using OrionposPhonebook.Models.Contexts;
using System.Security.Cryptography;
using System.Text;

namespace OrionposPhonebook.Controllers;

public class AuthenticationController : Controller
{
    private readonly OrionposPhonebookContext _context;

    public AuthenticationController(OrionposPhonebookContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["Error"] = false;
        return View();
    }

    [HttpPost]
    public IActionResult LoginForm(LoginModel model)
    {
        var user = _context.Users
            .FirstOrDefault(user => (user.Username == model.UsernameOrEmail || user.Email == model.UsernameOrEmail) && user.Password == model.Password);

        if (user is null)
        {
            ViewData["Error"] = true;
            return View("Index");
        }

        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
        var token = user.Username + user.Email + user.Password + timestamp;
        var tokenHash = GetHashString(token);

        user.Token = tokenHash;
        _context.SaveChanges();

        var cookieOption = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(7)
        };
        Response.Cookies.Append("UserId", user.Id.ToString(), cookieOption);
        Response.Cookies.Append("Token", tokenHash, cookieOption);
        Response.Cookies.Append("RememberMe", model.RememberMe.ToString(), cookieOption);
        
        HttpContext.Session.SetString("Alive", "True");

        //var contacts = _context.Contacts.ToList();
        //ViewData["Contacts"] = contacts;

        return RedirectToAction("Index", "Contact");

    }

    private string GetHashString(string text)
    {
        using var hashAlgorithm = SHA256.Create();
        var tokenHash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text));

        var sb = new StringBuilder();
        foreach (var b in tokenHash)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}