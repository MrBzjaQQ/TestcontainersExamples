namespace SeleniumExample.Contract;

public interface ICreateEmployeeRequest
{
    public string Phone { get; }
    public string UserName { get; }
    public string Email { get; }
    public string Position { get; }
}