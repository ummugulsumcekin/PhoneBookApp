using OrionposPhonebook.Utils.Abstract;

namespace OrionposPhonebook.Utils;

public class CurrentUserService : ICurrentUserService
{
    public int? UserId { get; set; }
}
