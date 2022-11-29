namespace Authentication.API.Models;

public class Credential
{
    public Credential(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public int CredentialId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    // Relationship
    public int UserId { get; set; }

    public List<Role> Roles { get; set; } = new();
}
