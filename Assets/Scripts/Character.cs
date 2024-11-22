public class Character
{
    private static int characterCount = 0;

    public int Id;
    public string Name;
    public Status Status;

    public Character(string name, Status status)
    {
        Id = ++characterCount;
        Name = name;
        Status = status;
    }
}

public class CharacterPresenter
{

}