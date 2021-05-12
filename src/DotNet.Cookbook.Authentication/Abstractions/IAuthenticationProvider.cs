using System.Collections.Generic;

namespace DotNet.Cookbook.Authentication.Abstractions
{
    /// <summary>
    /// Authorization provider contract. Extaracts roles and user's email./>
    /// </summary>
    public interface IAuthenticationProvider
    {
        string CurrentUserEmail();

        IList<string> Groups();
    }
}
