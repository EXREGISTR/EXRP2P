using Application.Services;
using Domain.Common;
using Domain.Currencies;
using Domain.Orders;
using Domain.Wallets;
using MediatR;
using System;

namespace Application.Orders;

public static class CreateSellOrder {
    public record Request(
        Guid SourceCurrencyId,
        Guid TargetCurrencyId,
        decimal Amount,
        decimal Price,
        string[] PaymentBanks)
        : IRequest;

    internal sealed class Handler(
        ICurrenciesRepository currenciesRepository,
        IWalletsRepository walletsRepository,
        IOrdersRepository ordersRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider timeProvider) : IRequestHandler<Request> {
        public async Task Handle(Request request, CancellationToken token) {
            var creatorId = userContext.Id;

            var sourceCurrencyPrecision = await currenciesRepository.GetPrecision(request.SourceCurrencyId, token)
                ?? throw CurrencyExceptions.SourceCurrencyNotFound;

            var targetCurrencyPrecision = await currenciesRepository.GetPrecision(request.TargetCurrencyId, token)
                ?? throw CurrencyExceptions.TargetCurrencyNotFound;

            var wallet = await walletsRepository.Find(creatorId, request.SourceCurrencyId, token)
                ?? throw WalletExceptions.NotEnoughMoney;

            wallet.Lock(request.Amount);

            var order = new Order(
                creatorId,
                amount: Math.Round(request.Amount, sourceCurrencyPrecision, MidpointRounding.AwayFromZero),
                price: Math.Round(request.Price, targetCurrencyPrecision, MidpointRounding.AwayFromZero),
                request.SourceCurrencyId,
                request.TargetCurrencyId,
                request.PaymentBanks,
                OrderType.Sell,
                timeProvider);

            await ordersRepository.Insert(order, token);

            await unitOfWork.SaveChanges(token);
        }
    }
}
