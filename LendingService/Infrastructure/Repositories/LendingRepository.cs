using LendingService.Application.Interfaces;
using LendingService.Domain.Entities;
using LendingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Repositories
{
    public class LendingRepository: ILendingRepository
    {
        private readonly ApplicationDbContext _context;

        public LendingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Lending> CreateAsync(Lending lending, CancellationToken cancellationToken)
        {
            var lend = await _context.Lendings.AddAsync(lending, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return lend.Entity;
        }

        public async Task<IEnumerable<Lending>> GetAllLendingsAsync(CancellationToken cancellationToken)
        {
            var lendings = await _context.Lendings.AsNoTracking().ToListAsync(cancellationToken);
            return lendings;
        }

        public async Task<Lending?> GetLendingAsync(int id, CancellationToken cancellationToken)
        {
            var lending = await _context.Lendings.FindAsync(id, cancellationToken);
            return lending;
        }

        public async Task<Lending?> GetLendingByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var lending = await _context.Lendings.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            return lending;
        }
    }
}
