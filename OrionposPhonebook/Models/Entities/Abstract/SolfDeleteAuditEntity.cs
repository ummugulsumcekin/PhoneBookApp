namespace OrionposPhonebook.Models.Entities.Abstract;

public abstract class SolfDeleteAuditEntity : AuditEntity, ISoftDeleteEntity
{
    public bool IsDeleted { get; set; }
}
