using System.ComponentModel.DataAnnotations.Schema;
<<<<<<< HEAD

namespace Authentication.API.Models;
=======
>>>>>>> 8f2d456107893510f74a5d3eedbdad6da5b6fe3d

namespace Authentication.API.Models;

[Table("Roles")]
public class Role
{
    public int RoleId { get; set; }
    public RoleType Type { get; set; }
    public DateTime Expiry { get; set; }
}

public enum RoleType
{
    Student, Instructor, Publisher, Admin
}
