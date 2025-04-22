using AutoGenerator.Repositories.Base;
using LAHJAAPI.Data;
using LAHJAAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace V1.Repositories.Base
{
    /// <summary>
    /// BaseRepository class for ShareRepository.
    /// </summary>
    public sealed class BaseRepository<T> : TBaseRepository<ApplicationUser, IdentityRole, string, T>, IBaseRepository<T> where T : class
    {
        public BaseRepository(DataContext db, ILogger logger) : base(db, logger)
        {
        }

        public async Task<bool> ExecuteTransactionAsync(Func<Task<bool>> operation)
        {
            return await base.ExecuteTransactionAsync(operation);
        }

    }
}