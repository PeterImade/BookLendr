using Contracts.Events;
using Contracts.Results;
using LendingService.Application;
using LendingService.Application.Dtos;
using LendingService.Application.Interfaces;
using LendingService.Domain.Entities;
using MassTransit;

namespace LendingService.Infrastructure.Services
{
    public class LendingBLService: ILendingService
    {
        private readonly ILendingRepository _lendingRepository;
        private readonly IRequestClient<CheckBookAvailabilityRequest> _client;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly BookServiceClient _bookServiceClient;

        public LendingBLService(ILendingRepository lendingRepository, IRequestClient<CheckBookAvailabilityRequest> client, IPublishEndpoint publishEndpoint, BookServiceClient bookServiceClient)
        {
            _lendingRepository = lendingRepository;
            _client = client;
            _publishEndpoint = publishEndpoint;
            _bookServiceClient = bookServiceClient;
        }

        public async Task<(bool, string?)> CheckBookAvailability(int bookId, CancellationToken cancellationToken)
        {
           var response = await _client.GetResponse<CheckBookAvailabilityResponse>
           (
               new CheckBookAvailabilityRequest
               {
                   BookId = bookId
               }
           );
            return (response.Message.IsAvailable, response.Message.BookTitle);
        }

        public async Task<Result<LendingResponse>> GetLendingAsync(int id, CancellationToken cancellationToken)
        { 
            var lending = await _lendingRepository.GetLendingAsync(id, cancellationToken);

            if (lending is null)
                return Result<LendingResponse>.Failed($"Lending with the id:{id} not found");

            var response = Mapper.MapToDto(lending);

            return Result<LendingResponse>.Success(response);
        }

        public async Task<Result<LendingResponse>> GetLendingByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var lending = await _lendingRepository.GetLendingByUserIdAsync(userId, cancellationToken);

            if (lending is null)
                return Result<LendingResponse>.Failed($"Lending with the user id:{userId} not found");

            var response = Mapper.MapToDto(lending);

            return Result<LendingResponse>.Success(response);
        }

        public async Task<Result<IEnumerable<LendingResponse>>> GetLendingsAsync(CancellationToken cancellationToken)
        {
            var lendings = await _lendingRepository.GetAllLendingsAsync(cancellationToken);
            var response = lendings.Select(x => Mapper.MapToDto(x));
            return Result<IEnumerable<LendingResponse>>.Success(response);
        }

        public async Task<Result<LendingResponse>> LendAsync(LendRequest lendRequest, int userId, string userEmail, CancellationToken cancellationToken)
        {
            var bookAvailability = await CheckBookAvailability(lendRequest.BookId, cancellationToken);

            if (!bookAvailability.Item1)
            {
                return Result<LendingResponse>.Failed("Book is not available");
            }

            var lending = new Lending
            {
                BookId = lendRequest.BookId,
                UserId = userId,
                DueDate = DateTime.Now.AddDays(7),
                Status = Status.Lent,
            };

            var result = await _lendingRepository.CreateAsync(lending, cancellationToken);

            var success = await _bookServiceClient.DecreaseBookQuantityAsync(lendRequest.BookId);

            if (!success)
            {
                return Result<LendingResponse>.Failed("Book not available");
            }

            await _publishEndpoint.Publish(new BookLentEvent
            {
                BookId = lendRequest.BookId,
                UserId = userId,
                BookTitle = bookAvailability.Item2,
                UserEmail = userEmail,
                DueDate = DateTime.Now.AddDays(7)
            });

            var response = Mapper.MapToDto(result);

            return Result<LendingResponse>.Success(response);
        }
    }
}