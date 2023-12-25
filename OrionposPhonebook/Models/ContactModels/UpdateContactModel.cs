namespace OrionposPhonebook.Models.ContactModels;

public class UpdateContactModel
{
    public int ContactId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string PhoneNumber { get; set; }
}
