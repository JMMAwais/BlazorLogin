using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorLogin.Data.Context.Model
{
    public class StateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sesssion;
       

        public StateProvider(ProtectedSessionStorage sesssion)
        {
                _sesssion = sesssion;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Simulating retrieval from session or any storage
            var userSession = await _sesssion.GetAsync<UserSession>("UserSession");

            if (userSession.Success && userSession.Value != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userSession.Value.UserName), // Example
        };

                var identity = new ClaimsIdentity(claims, "CustomAuth");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }

            // If no user session is found, return unauthenticated state
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task Login(UserSession userSession)
        {
            // Save the user session to ProtectedSessionStorage
            await _sesssion.SetAsync("UserSession", userSession);
           // await _sesssion.SetAsync("UserSession", new UserSession { UserName = "admin@example.com" });

            // Notify Blazor that the authentication state has changed
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
