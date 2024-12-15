using BlazorLogin.Data.Context.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;



namespace BlazorLogin.Data.Context
{
    public class Service
    {
        
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public Service(SignInManager<IdentityUser> signInManage, UserManager<IdentityUser> userManager)
        {
                _signInManager= signInManage;
            _userManager = userManager;
        }

        public async Task<string> Login(Logint model)
        {
            //model.Email = "admin@example.com";
            //model.Password = "Admin@123";

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return "User not found.";
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (isPasswordValid)
            {
                // Simulate authentication state
                return "Login successful!";
            }

            return "Invalid email or password.";


        }
    }
}
