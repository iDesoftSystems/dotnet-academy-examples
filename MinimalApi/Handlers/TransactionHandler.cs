using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Context;
using MinimalApi.Dto;
using MinimalApi.Models;

namespace MinimalApi.Handlers;

class TransactionHandler
{

    public static async Task<IResult> GetAllTransactions([FromQuery(Name = "page")] int page, RepositoryContext context)
    {
        return TypedResults.Ok(await context.Transactions
            .Select(t => TransactionItemDto.From(t))
            .ToListAsync());
    }

    public static async Task<IResult> GetIncomeTransactions(RepositoryContext context)
    {
        return TypedResults.Ok(await context.Transactions
            .Where(t => t.TransactionType == TransactionType.Income)
            .Select(t => TransactionItemDto.From(t))
            .ToListAsync());
    }

    public static async Task<IResult> GetOutcomeTransactions(RepositoryContext context)
    {
        return TypedResults.Ok(await context.Transactions
            .Where(t => t.TransactionType == TransactionType.Outcome)
            .Select(t => TransactionItemDto.From(t))
            .ToListAsync());
    }

    public static async Task<Results<Ok<TransactionItemDto>, NotFound>> GetTransaction(int id, RepositoryContext context)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction is not null)
        {
            return TypedResults.Ok(TransactionItemDto.From(transaction));
        }

        return TypedResults.NotFound();
    }

    public static async Task<IResult> CreateTransaction(CreateTransactionDto transactionDto, RepositoryContext context)
    {
        var transactionItem = new Models.Transaction
        {
            Summary = transactionDto.Summary,
            Amount = transactionDto.Amount,
            TransactionType = transactionDto.TransactionTypeId
        };

        context.Transactions.Add(transactionItem);
        await context.SaveChangesAsync();

        var location = $"/transactions/{transactionItem.Id}";
        return TypedResults.Created(location, transactionItem);
    }

    public static async Task<IResult> UpdateTransaction(int id, UpdateTransactionDto updateTransactionDto, RepositoryContext context)
    {
        var transaction = await context.Transactions.FindAsync(id);
        if (transaction is null)
        {
            return TypedResults.NotFound();
        }

        transaction.Summary = updateTransactionDto.Summary;

        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }


    public static async Task<IResult> DeleteTransaction(int id, RepositoryContext context)
    {
        if (await context.Transactions.FindAsync(id) is Transaction transaction)
        {
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

}