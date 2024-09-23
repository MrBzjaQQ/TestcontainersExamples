namespace TestIsolationStrategies;

public record User
{
    public long Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public void SetName(string name)
    {
        Name = name;
    }

    protected User()
    {

    }

    public User(string name)
    {
        Name = name;
    }
}