using System.ComponentModel.DataAnnotations;
using static Common.Constants.Validations.TransactionLimits;

namespace RestAPI.Dtos.Payments
{
    public class DepositRequestDto
    {
        [Range(typeof(decimal), DepositMinSum, DepositMaxSum)]
        public decimal Amount { get; set; }


    }
}
