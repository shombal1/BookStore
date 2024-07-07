namespace BookStore.APi.Models;

public class CreateAuthor
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Patronymic { get; set; } = "";
    public string City { get; set; } = "";
}