using OrionposPhonebook.Models.Entities;

namespace OrionposPhonebook.Models.ContactModels;

public class RemoveContactModel
{
    public IEnumerable<int> ContactIds { get; set; }
  
}
