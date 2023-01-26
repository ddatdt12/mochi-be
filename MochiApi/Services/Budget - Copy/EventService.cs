using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MochiApi.Dtos;
using MochiApi.Error;
using MochiApi.Helper;
using MochiApi.Hubs;
using MochiApi.Models;
using System.Linq.Expressions;

namespace MochiApi.Services
{
    //public class EventService : IEventService
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly IMapper _mapper;

    //    public DataContext _context { get; set; }

    //    public EventService(IConfiguration configuration, DataContext context, IMapper mapper)
    //    {
    //        _configuration = configuration;
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<Event>> GetEvents(int userId)
    //    {
    //        var @events = await _context.Events.Include(e => e.Wallet).Where(e =>
    //      (e.WalletId != null && e.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted))
    //      || (e.WalletId == null && e.CreatorId == userId)
    //      ).ToListAsync();

    //        return @events;
    //    }
    //    public async Task<IEnumerable<Event>> GetEventsOfWallet(int userId, int walletId)
    //    {
    //        var @events = await _context.Events.AsNoTracking().Include(e => e.Wallet).Where(e =>
    //      (e.WalletId == walletId && e.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted))
    //      || (e.WalletId == null && e.CreatorId == userId)
    //      ).ToListAsync();

    //        return @events;
    //    }
    //    public async Task<Event?> GetEventById(int id, int userId)
    //    {
    //        var @event = await _context.Events.AsNoTracking()
    //        .Where(e => e.Id == id && (e.Wallet!.WalletMembers.Any(wM => wM.UserId == userId && wM.Status == Common.Enum.MemberStatus.Accepted))
    //      || (e.WalletId == null && e.CreatorId == userId)
    //      ).Include(e => e.Wallet).FirstOrDefaultAsync();

    //        return @event;
    //    }
    //    public async Task<IEnumerable<Transaction>> GetTransactionOfEvent(int id)
    //    {
    //        var trans = await _context.Transactions.AsNoTracking().Where(b => b.EventId != id).OrderByDescending(tr => tr.CreatedAt)
    //        .ToListAsync();

    //        return trans;
    //    }


    //    public async Task<Budget> CreateEvent()
    //    {
    //        var cate = await _context.Categories.Where(c => c.Id == createDto.CategoryId).FirstOrDefaultAsync();

    //        if (cate == null || cate.Type == Common.Enum.CategoryType.Income)
    //        {
    //            throw new ApiException("Invalid category", 400);
    //        }

    //        if (await IsBudgetExist(createDto.CategoryId, createDto.Month, createDto.Year))
    //        {
    //            throw new ApiException("Budget for this category in this time duration have already existed!", 400);
    //        }

    //        var budget = _mapper.Map<Budget>(createDto);

    //        var spentInMonth = _context.Transactions.Where(t => t.CategoryId == budget.CategoryId
    //        && t.CreatedAt.Month == createDto.Month && t.CreatedAt.Year == createDto.Year).Sum(t => t.Amount);

    //        budget.CreatorId = userId;
    //        budget.SpentAmount = spentInMonth;
    //        await _context.Budgets.AddAsync(budget);

    //        await _context.SaveChangesAsync();
    //        return budget;
    //    }


      
    //}
}
