using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class BoletoPayment: Payment
    {
        public BoletoPayment(string number, DateTime paidDate, DateTime expireDate, decimal total, decimal totalPaid, Address address, Document document, string payer, Email email) 
        : base(number, paidDate, expireDate, total, totalPaid, address, document, payer, email)
        {
        }

        public string BarCode { get; private set; }
        public string BoletoNumber { get; private set; }
    }
}