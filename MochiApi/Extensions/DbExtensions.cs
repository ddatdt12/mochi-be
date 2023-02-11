using Microsoft.EntityFrameworkCore;
using MochiApi.Models;
using static MochiApi.Common.Enum;

namespace MochiApi.Extensions
{
    public static class DbExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>
            {
                new User{ Id = 1, Email = "test@gmail.com", Password = "123123123" },
                new User{ Id = 2, Email = "test2@gmail.com", Password = "123123123"},
                new User{ Id = 3, Email = "test3@gmail.com", Password = "123123123"},
                new User{ Id = 4, Email = "test4@gmail.com", Password = "123123123"},
            });

            var date = new DateTime(2022, 12, 29);
            var wallets = new List<Wallet>
            {
                new Wallet{ Id = 1, Type = Common.Enum.WalletType.Personal, IsDefault = true, Balance = 100000, Name = "Ví"},
                new Wallet{ Id = 2, Type = Common.Enum.WalletType.Personal, IsDefault = true, Balance = 200000, Name = "Ví"},
                new Wallet{ Id = 3, Type = Common.Enum.WalletType.Personal, IsDefault = true, Balance = 300000, Name = "Ví"},
                new Wallet{ Id = 4, Type = Common.Enum.WalletType.Personal, IsDefault = true, Balance = 400000, Name = "Ví"},
            };
            modelBuilder.Entity<Wallet>().HasData(wallets);

            modelBuilder.Entity<WalletMember>().HasData(
                new List<WalletMember>
                {
                    new WalletMember
                    {
                        Role = Common.Enum.MemberRole.Admin,
                        JoinAt  = date,
                        UserId = 1,
                        WalletId = 1
                    },
                    new WalletMember
                    {
                        Role = Common.Enum.MemberRole.Admin,
                        JoinAt  = date,
                        UserId = 2,
                        WalletId = 2
                    },
                    new WalletMember
                    {
                        Role = Common.Enum.MemberRole.Admin,
                        JoinAt  = date,
                        UserId = 3,
                        WalletId = 3
                    },
                    new WalletMember
                    {
                        Role = Common.Enum.MemberRole.Admin,
                        JoinAt  = date,
                        UserId = 4,
                        WalletId = 4
                    },
                }
                );

            var baseListCateogry = new List<Category> {
                new Category {
                    Id = 1,
                    Name = "Ăn uống",
                    Type = CategoryType.Expense,
                    Group = CategoryGroup.RequiredExpense,
                    Icon = "1",
                },
                new Category {
                    Id = 2,
                    Name = "Di chuyển",
                    Icon = "2",
                    Group = CategoryGroup.RequiredExpense,
                    Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 3,
                    Name = "Thuê nhà",
                    Icon = "3",
                    Group = CategoryGroup.RequiredExpense,
                    Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 4,
                    Name = "Hóa đơn điện thoại",
                    Icon = "4",
                    Group = CategoryGroup.RequiredExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 5,
                    Name = "Hóa đơn internet",
                    Icon = "5",
                    Group = CategoryGroup.RequiredExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 6,
                    Name = "Hóa đơn tiện ích khác",
                    Icon = "6",
                    Group = CategoryGroup.RequiredExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 7,
                    Name = "Sửa & trang trí khác",
                    Icon = "7",
                    Group = CategoryGroup.NecessaryExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 8,
                    Name = "Bảo dưỡng xe",
                    Icon = "8",
                    Group = CategoryGroup.RequiredExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 9,
                    Name = "Khám sức khỏe",
                    Icon = "1",
                    Group = CategoryGroup.NecessaryExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 10,
                    Name = "Thể dục thể thao",
                    Icon = "1",
                    Group = CategoryGroup.RequiredExpense,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 11,
                    Name = "Lương",
                    Icon = "1",
                    Group = CategoryGroup.Income,
                     Type = Common.Enum.CategoryType.Income,
                },
                new Category {
                    Id = 12,
                    Name = "Tiền ăn vặt",
                    Icon = "1",
                    Group = CategoryGroup.Income,
                     Type = Common.Enum.CategoryType.Income,
                },
                new Category {
                    Id = 13,
                    Name = "Thu nhập khác",
                    Icon = "1",
                    Group = CategoryGroup.Income,
                     Type = Common.Enum.CategoryType.Income,
                },
                new Category {
                    Id = 14,
                    Name = "Đầu tư",
                    Icon = "1",
                    Group = CategoryGroup.InvestingOrDebt,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 15,
                    Name = "Đi vay",
                    Icon = "1",
                    Group = CategoryGroup.InvestingOrDebt,
                     Type = Common.Enum.CategoryType.Debt,
                },
                new Category {
                    Id = 16,
                    Name = "Cho vay",
                    Icon = "1",
                    Group = CategoryGroup.InvestingOrDebt,
                     Type = Common.Enum.CategoryType.Loan,
                },
                new Category {
                    Id = 17,
                    Name = "Trả Nợ",
                    Icon = "1",
                    Group = CategoryGroup.InvestingOrDebt,
                     Type = Common.Enum.CategoryType.Repayment,
                },
                new Category {
                    Id = 18,
                    Name = "Thu Nợ",
                    Icon = "1",
                    Group = CategoryGroup.InvestingOrDebt,
                     Type = Common.Enum.CategoryType.DebtCollection,
                },
                new Category {
                    Id = 19,
                    Name = "Làm đẹp",
                    Icon = "3",
                    Group = CategoryGroup.Entertainment,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 20,
                    Name = "Quà tặng và quyên góp",
                    Icon = "4",
                    Group = CategoryGroup.Entertainment,
                     Type = Common.Enum.CategoryType.Expense,
                },
                new Category {
                    Id = 21,
                    Name = "Dịch vụ trực tuyến",
                    Icon = "4",
                    Group = CategoryGroup.Entertainment,
                     Type = Common.Enum.CategoryType.Expense,
                },
            };

            var listCategory = new List<Category>();
            var index = 22;

            foreach (var w in wallets)
            {
                foreach (var c in baseListCateogry)
                {
                    listCategory.Add(new Category
                    {
                        Id = index++,
                        WalletId = w.Id,
                        Name = c.Name,
                        Icon = c.Icon,
                        Group = c.Group,
                        Type = c.Type,
                    }); ;
                }
            }
            

            modelBuilder.Entity<Category>().HasData(baseListCateogry);
            modelBuilder.Entity<Category>().HasData(listCategory);
        }
    }
}
