namespace OrionposPhonebook.Models.Entities.Abstract;

public interface ISoftDeleteEntity : IEntity
{
    public bool IsDeleted { get; set; }
}
