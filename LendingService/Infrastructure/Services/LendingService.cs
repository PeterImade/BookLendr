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

        public async Task<bool> CheckBookAvailability(int bookId, CancellationToken cancellationToken)
        {
           var response = await _client.GetResponse<CheckBookAvailabilityResponse>
           (
               new CheckBookAvailabilityRequest
               {
                   BookId = bookId
               }
           );
            return response.Message.IsAvailable;
        }

        public async Task<Result<LendingResponse>> LendAsync(LendRequest lendRequest, int userId, string userEmail, CancellationToken cancellationToken)
        {
            var isAvailable = await CheckBookAvailability(lendRequest.BookId, cancellationToken);

            if (!isAvailable)
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
                UserEmail = userEmail,
                DueDate = DateTime.Now.AddDays(7)
            });

            var response = Mapper.MapToDto(result);

            return Result<LendingResponse>.Success(response);
        }
    }
}