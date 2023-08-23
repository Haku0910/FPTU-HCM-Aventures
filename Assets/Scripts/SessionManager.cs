using System.Collections.Generic;
public class SessionManager
{
    private Dictionary<string, string> userSessions = new Dictionary<string, string>(); // L?u th�ng tin phi�n ??ng nh?p

    public bool IsUserLoggedIn(string email, string passcode)
    {
        if (userSessions.ContainsKey(email))
        {
            return userSessions[email] == passcode;
        }
        return false;
    }

    public void UserLoggedIn(string email, string passcode)
    {
        userSessions[email] = passcode;
    }

    public void UserLoggedOut(string email)
    {
        if (userSessions.ContainsKey(email))
        {
            userSessions.Remove(email);
        }
    }
}
