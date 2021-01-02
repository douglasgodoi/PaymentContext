using System;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class BoletoPayment: Payment
    {
        public BoletoPayment(string barcode, string boletoNumber, DateTime paidDate, DateTime expireDate, decimal total, decimal totalPaid, string payer, Document document, Address address, Email email) 
        : base(boletoNumber, paidDate, expireDate, total, totalPaid, address, document, payer, email)
        {
        }

        public string BarCode { get; private set; }
        public string BoletoNumber { get; private set; }
    }
}