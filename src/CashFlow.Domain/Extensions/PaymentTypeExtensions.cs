using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourcePaymentTypeMessages.CASH,
            PaymentType.CreditCard => ResourcePaymentTypeMessages.CREDITCARD,
            PaymentType.DebidCard => ResourcePaymentTypeMessages.DEBITCARD,
            PaymentType.EletronicTransfer => ResourcePaymentTypeMessages.ELETRONICTRANSFER,
            _ => string.Empty
        };
    }
}