using LendingService.Application.Interfaces;
using LendingService.Domain.Entities;
using LendingService.Infrastructure.Data;

namespace LendingService.Infrastructure.Repositories
{
    public class LendingRepository: ILendingRepository
    {
        private readonly ApplicationDbContext _context;

        public LendingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Lending lending, CancellationToken cancellationToken)
        {
            await _context.Lendings.AddAsync(lending, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
