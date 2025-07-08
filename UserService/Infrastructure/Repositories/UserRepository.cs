using Microsoft.EntityFrameworkCore;
using System.Threading;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> CreateUser(User user, CancellationToken cancellationToken)
        { 
            var entity = await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Entity;
        }

        public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            return user;
        }

        public async Task<User?> GetUserById(int id, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(id, cancellationToken);
            return user;
        }

        public async Task<bool> CheckUserExists(string email, CancellationToken cancellationToken)
        {
            var userExists = await _dbContext.Users.AnyAsync(x => x.Email.Equals(email),cancellationToken);
            return userExists;
        }
    }
}
