namespace KlasseLib;

public class User_Role
{
    public int UserId { get; set; }
    public int RoleId { get; set; }


    public User_Role(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public override string ToString()
    {
        return $"{nameof(UserId)}: {UserId}, {nameof(RoleId)}: {RoleId}";
    }
}