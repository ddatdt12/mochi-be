namespace MochiApi.Dtos
{
    public class BudgetDetailSummary : BudgetSummary
    {
        public long RecommendedDailyExpense { get; set; }
        public long RealDailyExpense { get; set; }
        public long ExpectedExpense { get; set; }
    }
}
