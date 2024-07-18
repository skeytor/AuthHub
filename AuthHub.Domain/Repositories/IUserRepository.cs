﻿using AuthHub.Domain.Entities;

namespace AuthHub.Domain.Repositories;
public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<User>> GetAllAsync();
    Task<User?> GetByUserNameAsync(string userName);
    Task<bool> ExistAsync(string email);
}
