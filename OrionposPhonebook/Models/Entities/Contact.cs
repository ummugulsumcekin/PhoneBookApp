using OrionposPhonebook.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace OrionposPhonebook.Models.Entities;

public class Contact : SolfDeleteAuditEntity
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    public string? LastName { get; set; } = null;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
}
