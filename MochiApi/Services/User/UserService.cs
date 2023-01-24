﻿using Microsoft.EntityFrameworkCore;
using MochiApi.Models;

namespace MochiApi.Services
{
    public class UserService : IUserService
    {
        private DataContext _context { get; set; }
        public UserService(DataContext context)
        {
            _context = context;
        }
        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<IEnumerable<User>> SearchByEmail(string Email)
        {
            return await _context.Users.Where(u => u.Email.StartsWith(Email)).ToListAsync();
        }
    }
}
