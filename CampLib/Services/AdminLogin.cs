using CampLib.Model;
using Microsoft.AspNetCore.Identity;

namespace KlasseLib;a

public class AdminLogin
{
    public static Admin AdminUser { get; } =
        new Admin(
            "admin@edu.zealand.dk",
            new PasswordHasher<Admin>()
                .HashPassword(null, "saadersej")
        );
}