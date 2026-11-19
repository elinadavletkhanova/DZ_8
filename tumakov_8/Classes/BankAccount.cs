using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using tumakov_8.Enums;

namespace tumakov_8.Classes
{
    public class BankAccount
    {
        private long _accountNumber;
        private decimal _balance;
        private AccountType _accountType;

        private static long _nextNumber = 4000000000000;

        private Queue _transactions = new Queue();

        public BankAccount()
        {
            _accountNumber = NextNumber();
            Balance = 0;
            AccountType = AccountType.Current;
        }

        public BankAccount(decimal balance)
        {
            _accountNumber = NextNumber();
            Balance = balance;
            AccountType = AccountType.Current;
        }

        public BankAccount(AccountType accountType)
        {
            _accountNumber = NextNumber();
            Balance = 0;
            AccountType = accountType;
        }

        public BankAccount(decimal balance, AccountType accountType)
        {
            _accountNumber = NextNumber();
            Balance = balance;
            AccountType = accountType;
        }

        /// <summary>
        /// Генерирует следующий уникальный номер банковского счёта с помощью статического счётчика
        /// </summary>
        /// <returns>Уникальный номер банковского счёта</returns>
        public static long NextNumber()
        {
            return _nextNumber++;
        }

        public long AccountNumber
        {
            get { return _accountNumber; }
        }

        public decimal Balance
        {
            get { return _balance; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Баланс не может быть отрицательным");
                }

                _balance = value;
            }
        }

        public AccountType AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }

        /// <summary>
        /// Пополняет баланс банковского счёта на указанную сумму
        /// </summary>
        /// <param name="amount">Сумма пополнения</param>
        /// <exception cref="ArgumentException">Вызывается, если сумма пополнения меньше или равна нулю</exception>
        public void DepositMoney(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма пополнения должна быть больше нуля");
            }

            Balance += amount;

            _transactions.Enqueue(new BankTransaction(amount));

            Console.WriteLine($"Счёт: {AccountNumber} пополнен на {amount} руб. Текущий баланс счёта - {Balance} руб.");
        }

        /// <summary>
        /// Снимает указанную сумму с банковского счёта
        /// </summary>
        /// <param name="amount">Сумма снятия</param>
        /// <exception cref="ArgumentException">Вызывается, если сумма снятия меньше или равна нулю</exception>
        /// <exception cref="InvalidOperationException">Вызывается, если на счёте недостаточно средств для списания</exception>
        public void WithdrawMoney(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Сумма снятия должна быть больше нуля");
            }

            if (Balance < amount)
            {
                throw new InvalidOperationException($"Невозможно снять со счёта: {AccountNumber}, {amount} руб. На счёте недостаточно средств, его текущий баланс - {Balance} руб.");
            }

            Balance -= amount;

            _transactions.Enqueue(new BankTransaction(-amount));

            Console.WriteLine($"С счёта: {AccountNumber} снято {amount} руб. Текущий баланс счёта - {Balance} руб.");
        }

        /// <summary>
        /// Переводит указанную сумму с указанного счёта
        /// </summary>
        /// <param name="fromBankAccount">Счёт, с которого будет переводиться указанная сумма</param>
        /// <param name="amount">Сумма перевода</param>
        public void TransferMoney(BankAccount fromBankAccount, decimal amount)
        {
            fromBankAccount.WithdrawMoney(amount);

            DepositMoney(amount);

            Console.WriteLine($"Перевод {amount} рублей с счёта: {fromBankAccount.AccountNumber} на счёт: {AccountNumber} успешно выполнен.");
        }

        /// <summary>
        /// Считает количество транзакций очереди
        /// </summary>
        /// <returns>Количество транзакций</returns>
        public int TransactionsCount()
        {
            return _transactions.Count;
        }

        /// <summary>
        /// Выводит список транзакций в формате: дата, сумма
        /// </summary>
        public void PrintTransactions()
        {
            Console.WriteLine($"\nСписок транзакций счёта {AccountNumber}: ");

            if (_transactions.Count == 0)
            {
                Console.WriteLine("Транзакций нет");
                return;
            }

            foreach (var trans in _transactions)
            {
                BankTransaction transaction = trans as BankTransaction;
                Console.WriteLine(transaction.GetInfo());
            }
        }

        /// <summary>
        /// Сохраняет историю транзакций текущего счёта в текстовый файл
        /// </summary>
        public void Dispose()
        {
            List<string> transactionsHistory = new List<string>();

            transactionsHistory.Add($"История транзакций счёта с номером {AccountNumber}");

            while (_transactions.Count > 0)
            {
                BankTransaction transaction = (BankTransaction)_transactions.Dequeue();
                transactionsHistory.Add(transaction.GetInfo());
            }

            string filePath = $"..\\..\\Files\\Transactions_{AccountNumber}.txt";
            File.WriteAllLines(filePath, transactionsHistory);

            GC.SuppressFinalize(this);

            Console.WriteLine($"\nИстория счёта {AccountNumber} сохранена в файл");
        }

        /// <summary>
        /// Выводит всю информацию о банковском счёте
        /// </summary>
        public void Print()
        {
            Console.WriteLine($"Номер счёта - {AccountNumber}\nБаланс - {Balance} руб.\nТип счёта - {AccountType}\n");
        }
    }
}