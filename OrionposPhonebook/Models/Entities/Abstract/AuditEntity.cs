using System.ComponentModel.DataAnnotations.Schema;

namespace OrionposPhonebook.Models.Entities.Abstract;

public abstract class AuditEntity : Entity, IAuditEntity
{
    [ForeignKey("CreatedBy")]
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }

    [ForeignKey("UpdatedBy")]
    public int? UpdatedById { get; set; }
    public User UpdatedBy { get; set; }

    [ForeignKey("DeletedBy")]
    public int? DeletedById { get; set; }
    public User DeletedBy { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? DeletedAt { get; set; } = null;
}
