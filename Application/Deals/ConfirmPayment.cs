using Application.Services;
using Domain.Common;
using Domain.Deals;
using Domain.Orders;
using Domain.Wallets;
using MediatR;

namespace Application.Deals;

public static class ConfirmPayment {
    public record Request(Guid OrderId) : IRequest;

    internal sealed class Handler(
        IOrdersRepository ordersRepository,
        IDealsRepository dealsRepository, 
        IWalletsRepository walletsRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        IUserContext userContext) 
        : IRequestHandler<Request> {
        public async Task Handle(Request request, CancellationToken token) {
            var senderId = userContext.Id;

            var order = await ordersRepository.Find(request.OrderId, token) 
                ?? throw OrderExceptions.NotFound;

            var deal = await dealsRepository.Find(request.OrderId, token)
                ?? throw DealExceptions.NotFound;

            if (deal.SellerId != senderId) {
                throw DealExceptions.OnlySellerCanConfirmPayment;
            }

            var sellerWallet = await walletsRepository.FindForced(deal.SellerId, order.FromCurrencyId, token);
            sellerWallet.Withdraw(order.Amount);

            var buyerWallet = await walletsRepository.Find(deal.BuyerId, order.TargetCurrencyId, token);

            if (buyerWallet == null) {
                buyerWallet = new Wallet(
                    userId: deal.BuyerId,
                    currencyId: order.TargetCurrencyId,
                    initialBalance: order.Amount * order.Price);

                await walletsRepository.Insert(buyerWallet, token);
            } else {
                buyerWallet.Replenish(order.Amount * order.Price);
            }

            deal.Close(dateTimeProvider);
            order.ConfirmPayment();

            await unitOfWork.SaveChanges(token);
        }
    }
}