﻿using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IUserService
    {
        Task<User?> GetById(int id);
    }
}
