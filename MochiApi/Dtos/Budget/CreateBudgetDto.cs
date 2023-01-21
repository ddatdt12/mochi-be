using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MochiApi.Dtos
{
    public class CreateBudgetDto
    {
        public CreateBudgetDto()
        {
        }
        public int LimitAmount{ get; set; }
        public int Month{ get; set; }
        public int Year{ get; set; }
        public int CategoryId{ get; set; }
        public int WalletId { get; set; }
    }
}
