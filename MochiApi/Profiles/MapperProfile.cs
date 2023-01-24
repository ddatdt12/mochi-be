using AutoMapper;
using MochiApi.Dtos;
using MochiApi.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<RegisterUserDto, User>();
        CreateMap<LoginUserDto, User>();
        CreateMap<User, BasicUserDto>();

        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); ;
        CreateMap<Category, CategoryDto>();

        CreateMap<Wallet, WalletDto>();
        CreateMap<CreateWalletDto, Wallet>();
        CreateMap<UpdateWalletDto, Wallet>();
        CreateMap<WalletMember, WalletMemberDto>();

        CreateMap<Transaction, TransactionDto>();
        CreateMap<CreateTransactionDto, Transaction>();
        CreateMap<UpdateTransactionDto, Transaction>();

        CreateMap<Budget, BudgetDto>();
        CreateMap<CreateBudgetDto, Budget>();
        CreateMap<UpdateBudgetDto, Budget>();

        CreateMap<CreateNotificationDto, Notification>();
        CreateMap<Notification, NotificationDto>();

        CreateMap<Invitation, InvitationDto>();

    }
}