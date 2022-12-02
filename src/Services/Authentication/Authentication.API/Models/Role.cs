using System.ComponentModel.DataAnnotations.Schema;
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
