using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserId id,  CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

        void Add(User user);
    }
}
