using System;

namespace LNP.Core.Entities
{
    public class PaymentEf
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public bool IsSuccessful { get; set; }
        
        public virtual OrderEf Order { get; set; }
    }
}