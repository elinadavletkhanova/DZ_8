using System;

namespace tumakov_8.Classes
{
    public class BankTransaction
    {
        private readonly DateTime _transactionDate;
        private readonly decimal _amount;

        public BankTransaction(decimal amount)
        {
            _transactionDate = DateTime.Now;
            _amount = amount;
        }

        public string GetInfo()
        {
            return $"Дата: {_transactionDate}, сумма: {_amount} руб.";
        }
    }
}