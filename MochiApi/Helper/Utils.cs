using static MochiApi.Common.Enum;

namespace MochiApi.Helper
{
    public class Utils
    {
        public static readonly List<CategoryType> PrivateCategoryTypes = new List<CategoryType>() {
            CategoryType.Debt,
            CategoryType.Repayment,
            CategoryType.Loan,
            CategoryType.DebtCollection
        };
        public static readonly List<CategoryType> MinusCategoryTypes = new List<CategoryType>() {
            CategoryType.Expense,
            CategoryType.Repayment,
            CategoryType.Loan,
        };
        public static readonly List<CategoryType> PlusCategoryTypes = new List<CategoryType>() {
            CategoryType.Income,
            CategoryType.Debt,
            CategoryType.DebtCollection,
        };
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

    }
}
