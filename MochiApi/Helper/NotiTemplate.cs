namespace MochiApi.Helper
{
    public class NotiTemplate
    {
        public static string GetRemindBudgetExceedLimit(string budgetName, int month, int year)
        {
            return $"Ngân sách {budgetName} đã vượt quá giá trị tối đa của tháng {month}-{year}";
        }
    }
}
