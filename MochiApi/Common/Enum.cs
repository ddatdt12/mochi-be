using System.Text.Json.Serialization;

namespace MochiApi.Common
{
    public static class Enum
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CategoryType
        {
            Income,
            Expense
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum CategoryGroup
        {
            Income,
            RequiredExpense,
            NecessaryExpense,
            Entertainment,
            InvestingOrDebt,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum WalletType
        {
            Personal,
            Group
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum MemberRole
        {
            Admin,
            Member
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Mode
        {
            Light,
            Dark
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum NotificationType
        {
            BudgetExceed,
            Reminder,
            JoinWalletInvitation
        }
    }
}
