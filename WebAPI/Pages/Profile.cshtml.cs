using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAPI.Pages
{
    public class ProfileModel : PageModel
    {
        private UserManager<ApplicationUser> userManager;
        public ProfileModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public string Motto { get; set; }
        public bool PrivateProfile { get; set; }
        public async Task OnGetAsync()
        {
            ApplicationUser applicationUser = await userManager.GetUserAsync(HttpContext.User);
            Motto = applicationUser.Motto;
            
        }

    }
}
