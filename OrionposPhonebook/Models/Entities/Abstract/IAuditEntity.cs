using System.ComponentModel.DataAnnotations.Schema;

namespace OrionposPhonebook.Models.Entities.Abstract;

public interface IAuditEntity : IEntity
{
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public int? UpdatedById { get; set; }
    public User UpdatedBy { get; set; }
    public int? DeletedById { get; set; }
    public User DeletedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
