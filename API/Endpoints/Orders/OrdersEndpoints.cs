using API.Utils;
using Application.Old.Requests;
using Application.Orders.ConfirmPayments;
using Application.Orders.Requests;
using Domain.Common;
using Domain.Orders;
using Domain.Users;
using Domain.Wallets;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.Orders;

public static class OrdersEndpoints {
    public static void MapOrdersEndpoints(this IEndpointRouteBuilder builder) {
        var group = builder.MapGroup("/orders")
            .RequireAuthorization(x => x.RequireRole(UserRoles.VerifiedUser));

        group.MapPost("/sell", Sell);

        group.MapPost("/buy", Buy);

        group.MapPost("/reserve-buying", ReserveBuying);

        group.MapPost("/reserve-selling", ReserveSelling);

        group.MapPost("/finish-deal", FinishDeal);
    }

    public static async Task<IResult> LoadPaymentProofUrl(
        [FromBody] LoadPaymentProofRequest request, 
        ApplicationDbContext appDbContext,
        UserContextAccessor userContext,
        CancellationToken token) {
        var senderId = userContext.Id;

        var deal = await appDbContext.Deals.FirstOrDefaultAsync(x => x.Id == request.OrderId, token);

        if (deal == null || deal.BuyerId != senderId) {
            return Results.NotFound("You have not booked a purchase order");
        }

        deal.LoadPaymentProofUrl(request.PaymentProofUrl);

        await appDbContext.SaveChangesAsync(token);

        return Results.Accepted();
    }

    public static async Task<IResult> FinishDeal(
        [FromBody] ConfirmPaymentRequest request,
        ApplicationDbContext appDbContext,
        UserContextAccessor userContext,
        IDateTimeProvider timeProvider,
        CancellationToken token) {
        var senderId = userContext.Id;

        var order = await appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, token);

        if (order == null) return Results.NotFound("Order not found");

        var deal = await appDbContext.Deals.FirstOrDefaultAsync(x => x.Id == request.OrderId, token);

        if (deal == null) {
            return Results.NotFound("You didn't make a deal");
        }

        if (deal.SellerId != senderId) {
            return Results.Conflict("You aren't seller");
        }

        var sellerWallet = await appDbContext.Wallets
            .Where(x => x.UserId == deal.SellerId && x.CurrencyId == order.FromCurrencyId)
            .FirstAsync(token);

        sellerWallet.Remove(order.Amount);

        var buyerWallet = await appDbContext.Wallets
            .Where(x => x.UserId == deal.BuyerId && x.CurrencyId == order.TargetCurrencyId)
            .FirstOrDefaultAsync(token);

        if (buyerWallet == null) {
            buyerWallet = new Wallet(
                userId: deal.BuyerId,
                currencyId: order.TargetCurrencyId,
                initialBalance: order.Amount * order.Price);
            appDbContext.Wallets.Add(buyerWallet);
        } else {
            buyerWallet.Add(order.Amount * order.Price);
        }

        deal.Close(timeProvider);
        order.ConfirmPayment();

        await appDbContext.SaveChangesAsync(token);

        return Results.Accepted();
    }

    private static async Task<IResult> ReserveBuying() {
        // create deal


        throw new InvalidOperationException("You can't dispute order because order is not active");
    }

    private static async Task<IResult> ReserveSelling() {
        // create deal

        throw new InvalidOperationException("You can't dispute order because order is not active");
    }

    public static async Task<IResult> Sell(
        [FromBody] CreateSellOrderRequest request,
        ApplicationDbContext appDbContext,
        UserContextAccessor userContext,
        IDateTimeProvider timeProvider,
        CancellationToken token) {
        var creatorId = userContext.Id;

        var fromCurrency = await appDbContext.Currencies
            .AsNoTracking()
            .Where(x => x.Id == request.FromCurrencyId)
            .Select(x => new { x.Precision })
            .FirstOrDefaultAsync(token);

        if (fromCurrency == null) {
            return Results.NotFound("From currency not found");
        }

        var targetCurrency = await appDbContext.Currencies
            .AsNoTracking()
            .Where(x => x.Id == request.FromCurrencyId)
            .Select(x => new { x.Precision })
            .FirstOrDefaultAsync(token);

        if (targetCurrency == null) {
            return Results.NotFound("Target currency not found");
        }

        var wallet = await appDbContext.Wallets
            .Where(x => x.UserId == creatorId && x.CurrencyId == request.FromCurrencyId)
            .FirstOrDefaultAsync(token);

        if (wallet == null) {
            return Results.NotFound($"Wallet is not created for from currency");
        }

        wallet.Lock(request.Amount);

        var order = new Order(
            creatorId,
            amount: Math.Round(request.Amount, fromCurrency.Precision, MidpointRounding.AwayFromZero),
            price: Math.Round(request.Price, targetCurrency.Precision, MidpointRounding.AwayFromZero),
            request.FromCurrencyId,
            request.TargetCurrencyId,
            request.PaymentBanks,
            OrderType.Sell,
            timeProvider);


        appDbContext.Orders.Add(order);

        await appDbContext.SaveChangesAsync(token);

        return Results.Accepted();
    }

    public static async Task<IResult> Buy(
        [FromBody] CreateBuyOrderRequest request,
        ApplicationDbContext appDbContext,
        UserContextAccessor userContext,
        IDateTimeProvider timeProvider,
        CancellationToken token) {
        var creatorId = userContext.Id;

        var fromCurrency = await appDbContext.Currencies
            .AsNoTracking()
            .Where(x => x.Id == request.FromCurrencyId)
            .Select(x => new { x.Precision })
            .FirstOrDefaultAsync(token);

        if (fromCurrency == null) {
            return Results.NotFound("From currency not found");
        }

        var targetCurrency = await appDbContext.Currencies
            .AsNoTracking()
            .Where(x => x.Id == request.FromCurrencyId)
            .Select(x => new { x.Precision })
            .FirstOrDefaultAsync(token);

        if (targetCurrency == null) {
            return Results.NotFound("Target currency not found");
        }

        var order = new Order(
            creatorId,
            amount: Math.Round(request.Amount, fromCurrency.Precision, MidpointRounding.AwayFromZero),
            price: Math.Round(request.Price, targetCurrency.Precision, MidpointRounding.AwayFromZero),
            request.FromCurrencyId,
            request.TargetCurrencyId,
            paymentBanks: [],
            OrderType.Buy,
            timeProvider);

        appDbContext.Orders.Add(order);

        await appDbContext.SaveChangesAsync(token);

        return Results.Accepted();
    }
}
