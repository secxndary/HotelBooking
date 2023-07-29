﻿using Entities.Models;
namespace Repository.Extentsions;

public static class UserRepositoryExtensions
{
    public static IQueryable<User> Search(this IQueryable<User> users, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return users;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        var usersWithSearchTermInLastName = users
            .Where(u => u.LastName.ToLower().Contains(lowerCaseTerm))
            .Select(u => new { User = u, Priority = 1 });

        var usersWithSearchTermInFirstName= users
            .Where(u =>
                !u.LastName.ToLower().Contains(lowerCaseTerm) &&
                u.FirstName.ToLower().Contains(lowerCaseTerm))
            .Select(u => new { User = u, Priority = 2 });

        var usersWithSearchTermInEmail = users
            .Where(u =>
                !u.LastName.ToLower().Contains(lowerCaseTerm) &&
                !u.FirstName.ToLower().Contains(lowerCaseTerm) &&
                u.Email.ToLower().Contains(lowerCaseTerm))
            .Select(u => new { User = u, Priority = 3 });

        var result = usersWithSearchTermInLastName
            .Union(usersWithSearchTermInFirstName)
            .Union(usersWithSearchTermInEmail)
            .OrderBy(u => u.Priority)
            .Select(u => u.User);

        return result;
    }
}
