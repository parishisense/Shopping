﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(AddUserViewModel model)
        {
            User user = new()
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = model.ImageId,
                PhoneNumber = model.PhoneNumber,
                City = await _context.Cities.FindAsync(model.CityId),
                UserName = model.UserName,
                UserType = model.UserType,
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if ((result != IdentityResult.Success))
            {
                return null;
            }

            User newUser = await GetUserAsync(model.UserName);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;

        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }



        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users
                .Include(u => u.City)
                .ThenInclude(c => c.State)
                .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _context.Users
        .Include(u => u.City)
         .ThenInclude(c => c.State)
        .ThenInclude(s => s.Country)
        .FirstOrDefaultAsync(u => u.Id == userId.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }



        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }
    }
}
