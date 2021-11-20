﻿using Domain;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class UserHub : Hub
    { 
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext applicationDbContext;
        public UserHub(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }
        public override async Task OnConnectedAsync()
        {
            ApplicationUser appUser = await userManager.FindByIdAsync(Context.User.FindFirst("sub")?.Value);
            if (appUser.IsOnline is false)
            {
                appUser.IsOnline = true;
                appUser.TabsOpen = 1;
                await applicationDbContext.SaveChangesAsync();
                await Clients.All.SendAsync("Update");
                return;
            }
            if (appUser.IsOnline)
            {
                appUser.TabsOpen++;
                await applicationDbContext.SaveChangesAsync();
            }
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            ApplicationUser appUser = await userManager.FindByIdAsync(Context.User.FindFirst("sub")?.Value);
            if (appUser.TabsOpen > 0)
            {
                appUser.TabsOpen--;
                await applicationDbContext.SaveChangesAsync();
            }
            if (appUser.TabsOpen == 0)
            {
                appUser.IsOnline = false;
                await applicationDbContext.SaveChangesAsync();
                await Clients.All.SendAsync("Update");
            }
        }
    }
}
