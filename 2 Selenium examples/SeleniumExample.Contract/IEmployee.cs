namespace SeleniumExample.Contract;

public interface IEmployee
{
    public Guid Id { get; }
    public string Phone { get; }
    public string UserName { get; }
    public string Email { get; }
    public string Position { get; }
}