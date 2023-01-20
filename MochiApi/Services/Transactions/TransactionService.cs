﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Models;

namespace MochiApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DataContext _context { get; set; }

        public TransactionService(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(int walletId, TransactionFilterDto filter)
        {
            var transQuery = _context.Transactions.Where(t => t.WalletId == walletId)
            .Include(t => t.Category)
            .OrderByDescending(t=> t.CreatedAt)
            .AsNoTracking();

            if (filter.StartDate.HasValue)
            {
                var startDate = filter.StartDate?.Date;
                transQuery = transQuery.Where(t => t.CreatedAt >= startDate);
            }

            if (filter.EndDate.HasValue)
            {
                var EndDate = filter.EndDate?.Date.AddDays(1).AddSeconds(-1);
                transQuery = transQuery.Where(t => t.CreatedAt <= EndDate);
            }


            var trans = await transQuery.ToListAsync();
            return trans;
        }

        public async Task<Transaction> CreateTransaction(int userId, int walletId, CreateTransactionDto transDto)
        {
                var trans = _mapper.Map<Transaction>(transDto);

                trans.CreatorId = userId;
                trans.WalletId = walletId;

                await _context.Transactions.AddAsync(trans);
                await _context.SaveChangesAsync();

                return trans;
        }

        public async Task UpdateTransaction(int transactionId, int walletId,UpdateTransactionDto updateTransDto)
        {
            var trans = await _context.Transactions.Where(t => t.Id == transactionId && t.WalletId == walletId).FirstOrDefaultAsync();
            if (trans == null)
            {
                throw new ApiException("Transaction not found!",400);
            }

            _mapper.Map(updateTransDto, trans);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransaction(int transactionId)
        {
            var trans = await _context.Transactions.Where(t => t.Id == transactionId).FirstOrDefaultAsync();
            if (trans == null)
            {
                throw new ApiException("Transaction not found!", 400);
            }
            _context.Transactions.Remove(trans);
            await _context.SaveChangesAsync();
        }
    }
}
