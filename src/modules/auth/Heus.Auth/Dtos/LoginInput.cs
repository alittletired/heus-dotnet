namespace Heus.Auth.Dtos;

public class LoginInput
{
    public LoginInput(string account, string password, bool rememberMe)
    {
        Account = account;
        Password = password;
        RememberMe = rememberMe;
    }

    public string Account { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    
   
}