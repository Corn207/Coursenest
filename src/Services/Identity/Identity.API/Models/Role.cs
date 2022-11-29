namespace Identity.API.Models;

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
