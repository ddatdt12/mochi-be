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

        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();

        CreateMap<Wallet, WalletDto>();
        CreateMap<CreateWalletDto, Wallet>();
        CreateMap<UpdateWalletDto, Wallet>();

        CreateMap<Transaction, TransactionDto>();
        CreateMap<CreateTransactionDto, Transaction>();
        CreateMap<UpdateTransactionDto, Transaction>();
    }
}