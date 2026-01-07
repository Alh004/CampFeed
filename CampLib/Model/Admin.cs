namespace CampLib.Model;

public class Admin
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }

    public Admin(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }
    
}