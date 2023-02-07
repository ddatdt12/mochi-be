namespace MochiApi.Helper
{
    public class NotiTemplate
    {
        public static string GetRemindBudgetExceedLimit(string budgetName, int month, int year)
        {
            return $"Ngân sách {budgetName} đã vượt quá giá trị tối đa của tháng {month}-{year}";
        }
        public static string GetRemindBudgetExceedLimitInDay(string budgetName)
        {
            return $"Ngân sách {budgetName} đã vượt quá giá trị tối đa của nên dùng trong ngày {DateTime.Now.ToString("dd/MM/yyyy")}";
        }
        public static string GetInvitationContent(string walletName)
        {
            return $"Bạn vừa được mời tham gia vào ví {walletName}";
        }
    }
}
