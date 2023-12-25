using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrionposPhonebook.Models.ContactModels;
using OrionposPhonebook.Models.Contexts;
using OrionposPhonebook.Models.Entities;
using System.Net;
using System.Text.Json;

namespace OrionposPhonebook.Controllers;

public class ContactController : Controller
{
    private readonly OrionposPhonebookContext _context;

    public ContactController(OrionposPhonebookContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        var result = ValidateAndRedirect("Index");

        if (!result.Item2)
        {
            return result.Item1;
        }

        var contacts = _context.Contacts.AsEnumerable();
        var mappedContacts = contacts.Select(contact => new
        {
            contact.Id,
            contact.FirstName,
            contact.LastName,
            contact.PhoneNumber
        });

        ViewData["Contacts"] = JsonSerializer.Serialize(mappedContacts);

        return result.Item1;
    }

    public IActionResult AddContactView()
    {
        var result = ValidateAndRedirect("AddContactView");
        return result.Item1;
    }

    [HttpGet("{controller}/{action}/{contactId:int}")]
    public IActionResult UpdateContactView(int contactId)
    {
        var result = ValidateAndRedirect("UpdateContactView");

        if (!result.Item2)
        {
            return result.Item1;
        }

        var contact = _context.Contacts.FirstOrDefault(contact => contact.Id == contactId);

        if (contact == null)
        {
            ViewData["Error"] = "Kayıt bulunamadı.";
        }
        else
        {
            ViewData["Contact"] = JsonSerializer.Serialize(new
            {
                contact.Id,
                contact.FirstName,
                contact.LastName,
                contact.PhoneNumber,
            });
        }

        return result.Item1;
    }

    [HttpPost]
    public IActionResult AddContactForm(AddContactModel model)
    {
        var validated = ValidateRequest();

        if (!validated)
        {
            return RedirectToAction("Index", "Authentication");
        }

        var phoneNumberExists = _context.Contacts.Any(contact => contact.PhoneNumber == model.PhoneNumber);

        if (phoneNumberExists)
        {
            ViewData["Error"] = "Bu telefon numarasına başka kişi kayıtlı";
            return View("AddContactView");
        }

        var contact = new Contact
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };

        _context.Contacts.Add(contact);
        _context.SaveChanges();

        
        return RedirectToAction("Index");
    }

    [HttpPost("{controller}/{action}/{contactId:int}")]
    public IActionResult UpdateContactForm(int contactId, UpdateContactModel model)
    {
        var validated = ValidateRequest();

        if (!validated)
        {
            return RedirectToAction("Index", "Authentication");
        }

        var contact = _context.Contacts.FirstOrDefault(contact => contact.Id == contactId);

        if (contact is null)
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Contact bulunamadı" });
        }

        var phoneNumberExists = _context.Contacts.Any(contact => contact.PhoneNumber == model.PhoneNumber && contact.Id != contactId);

        if (phoneNumberExists)
        {
            ViewData["Error"] = "Bu telefon numarasına başka kişi kayıtlı";
            return View("UpdateContactView");
        }

        contact.FirstName = model.FirstName;
        contact.LastName = model.LastName;
        contact.PhoneNumber = model.PhoneNumber;
        _context.SaveChanges();

       

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult RemoveContacts([FromBody] RemoveContactModel model)
    {
        var validated = ValidateRequest();

        if (!validated)
        {
            return StatusCode((int)HttpStatusCode.Unauthorized);
        }

        var contacts = _context.Contacts
            .Where(contact => model.ContactIds.Contains(contact.Id))
            .AsEnumerable();

        _context.Contacts.RemoveRange(contacts);
        _context.SaveChanges();

        return StatusCode((int)HttpStatusCode.NoContent);
    }

    private Tuple<IActionResult, bool> ValidateAndRedirect(string view)
    {
        var isAlive = bool.Parse(HttpContext.Session.GetString("Alive") ?? "False");

        if (isAlive)
        {
            return new Tuple<IActionResult, bool>(View(view), true);
        }

        if (Request.Cookies.TryGetValue("RememberMe", out var rememberMe))
        {
            if (!bool.Parse(rememberMe))
            {
                Response.Cookies.Delete("UserId");
                Response.Cookies.Delete("Token");
                Response.Cookies.Delete("RememberMe");
                return new Tuple<IActionResult, bool>(RedirectToAction("Index", "Authentication"), false);
            }

            HttpContext.Session.SetString("Alive", "True");

            return new Tuple<IActionResult, bool>(View(view), true);
        }

        return new Tuple<IActionResult, bool>(RedirectToAction("Index", "Authentication"), false);
    }

    private bool ValidateRequest()
    {
        var validated = true;

        if (Request.Cookies.TryGetValue("Token", out var token))
        {
            var user = _context.Users.FirstOrDefault(user => user.Token == token);

            if (user is null)
            {
                validated = false;
            }

            if (Request.Cookies.TryGetValue("UserId", out var userId))
            {
                if (int.Parse(userId) != user.Id)
                {
                    validated = false;
                }
            }
            else
            {
                validated = false;
            }
        }
        else
        {
            validated = false;
        }

        if (!validated)
        {
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("RememberMe");
        }

        return validated;
    }
    #region API CALLS
    [HttpGet]
    public IActionResult GetAllContacts()
    {
        var contacts = _context.Contacts.ToList(); // Retrieve all contacts from the database
        var mappedContacts = contacts.Select(contact => new
        {
            contact.Id,
            contact.FirstName,
            contact.LastName,
            contact.PhoneNumber
        });

        return Json(new { data = mappedContacts });
    }
    #endregion

}
