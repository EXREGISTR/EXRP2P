using Application.Services;
using Domain.Deals;
using MediatR;

namespace Application.Deals;

public static class LoadPaymentProofUrl {
    public record Request(
        Guid OrderId,
        string PaymentProofUrl)
        : IRequest;

    internal sealed class Handler(
        IDealsRepository dealsRepository, 
        IUnitOfWork unitOfWork,
        IUserContext userContext) 
        : IRequestHandler<Request> {
        public async Task Handle(Request request, CancellationToken token) {
            var senderId = userContext.Id;

            var deal = await dealsRepository.Find(request.OrderId, token);

            if (deal == null || deal.BuyerId != senderId) {
                throw DealExceptions.NotFound;
            }

            deal.LoadPaymentProofUrl(request.PaymentProofUrl);

            await unitOfWork.SaveChanges(token);
        }
    }
}
