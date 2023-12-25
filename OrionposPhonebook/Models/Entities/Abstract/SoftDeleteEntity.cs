namespace OrionposPhonebook.Models.Entities.Abstract;

public abstract class SoftDeleteEntity : Entity, ISoftDeleteEntity
{
    public bool IsDeleted { get; set; }
}
