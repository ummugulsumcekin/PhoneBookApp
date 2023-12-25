using OrionposPhonebook.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrionposPhonebook.Models.Entities;

public class User : Entity
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    public string? Token { get; set; } = null;

    [InverseProperty("CreatedBy")]
    public ICollection<Contact> Creations { get; set; }

    [InverseProperty("UpdatedBy")]
    public ICollection<Contact> Updates { get; set; }

    [InverseProperty("DeletedBy")]
    public ICollection<Contact> Deletions { get; set; }

    public static implicit operator User?(string? v)
    {
        throw new NotImplementedException();
    }
}
