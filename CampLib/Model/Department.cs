namespace KlasseLib;

public class Department
{
    public int ID { get; set; }
    public string Name { get; set; }

    public Department(int id, string name)
    {
        ID = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"{nameof(ID)}: {ID}, {nameof(Name)}: {Name}";
    }
}