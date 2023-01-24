namespace MochiApi.Dtos
{
    public class BudgetDetailSummary : BudgetSummary
    {
        public double RecommendedDailyExpense { get; set; }
        public double RealDailyExpense { get; set; }
        public double ExpectedExpense { get; set; }
    }
}
