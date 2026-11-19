using System;
using System.Collections.Generic;
using tumakov_8.Enums;
using tumakov_8.Classes;

namespace tumakov_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Exercises9_123();
            Homework9_1();
        }

        #region Упражнения 9.1, 9.2, 9.3

        public static void Exercises9_123()
        {
            Console.WriteLine("Упражнения 9.1, 9.2 и 9.3");
            Console.WriteLine("Упражнение 9.1 - Демонстрация 4 разных конструкторов в банковском классе");
            Console.WriteLine("Введите цифру от 1 до 4 для проверки конструктора:\n1. Пустой конструктор\n2. Только баланс\n3. Только тип\n4. Тип и баланс");

            BankAccount account = null;

            switch (Console.ReadLine())
            {
                case "1":
                    account = new BankAccount();
                    break;

                case "2":
                    Console.WriteLine("Введите баланс счёта (через запятую): ");

                    if (decimal.TryParse(Console.ReadLine(), out var balance))
                        account = new BankAccount(balance);
                    else
                        Console.WriteLine("Ошибка: введен неверный формат. Счёт не создан");
                        
                    break;

                case "3":
                    Console.WriteLine("Введите тип банковского счёта (current/deposit): ");

                    if (Enum.TryParse(Console.ReadLine(), true, out AccountType type))
                        account = new BankAccount(type);
                    else
                        Console.WriteLine("Ошибка: введен неверный формат. Счёт не создан");

                    break;

                case "4":
                    account = NewAccount();

                    break;

                default:
                    Console.WriteLine("Ошибка: неверный выбор. Счёт не создан");

                    break;
            }

            if (account != null)
            {
                Console.WriteLine("Создан банковский аккаунт: ");
                account.Print();
            }

            Console.WriteLine("Упражнение 9.2 - Создается класс транзакций и очередь для его объектов");

            BankAccount firstAccount = NewAccount();
            if (firstAccount == null)
                return;
            firstAccount.Print();

            BankAccount secondAccount = NewAccount();
            if (secondAccount == null)
                return;
            secondAccount.Print();

            Console.WriteLine("Введите одну из цифр в зависимости от нужной операции:\n1 - Пополнить первый счёт\n2 - Снять деньги с " +
                "первого счёта\n3 - Перевести деньги с первого счёта на второй");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("Введите сумму для пополнения (через запятую): ");

                        if (!decimal.TryParse(Console.ReadLine(), out var depAmount))
                        {
                            Console.WriteLine("Ошибка: введен неверный формат числа");
                            break;
                        }

                        firstAccount.DepositMoney(depAmount);
                        break;

                    case "2":
                        Console.WriteLine("Введите сумму для снятия (через запятую): ");

                        if (!decimal.TryParse(Console.ReadLine(), out var withAmount))
                        {
                            Console.WriteLine("Ошибка: введен неверный формат числа");
                            break;
                        }

                        firstAccount.WithdrawMoney(withAmount);
                        break;

                    case "3":
                        Console.WriteLine("Введите сумму для перевода денег с первого на второй счёт (через запятую): ");

                        if (!decimal.TryParse(Console.ReadLine(), out var amount))
                        {
                            Console.WriteLine("Ошибка: введен неверный формат числа");
                            break;
                        }

                        secondAccount.TransferMoney(firstAccount, amount);
                        break;

                    default:
                        Console.WriteLine("Ошибка: неверный ввод, баланс счёта не поменялся");
                        break;
                }

                firstAccount.PrintTransactions();
                secondAccount.PrintTransactions();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nУпражнение 9.3 - Создается метод Dispose и записываются данные о транзакциях счета в файл");

                account.Dispose();
                firstAccount.Dispose();
                secondAccount.Dispose();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Запрашивает баланс и тип банковского счёта для создания объекта
        /// </summary>
        /// <returns>Новый объект BankAccount или же null</returns>
        public static BankAccount NewAccount()
        {
            Console.WriteLine("Введите баланс счёта (через запятую): ");
            var isBalance = decimal.TryParse(Console.ReadLine(), out var balance);

            Console.WriteLine("Введите тип банковского счета (current/deposit): ");
            var isType = Enum.TryParse(Console.ReadLine(), true, out AccountType type);

            if (!isBalance || !isType)
            {
                Console.WriteLine("Ошибка: введены неккоректные данные или неверный формат");
                return null;
            }

            return new BankAccount(balance, type);
        }

        #endregion

        #region Домашнее задание 9.1

        public static void Homework9_1()
        {
            Console.WriteLine("\nДомашнее задание 9.1 - Список песен, сравнение песен с новыми конструкторами");

            var songs = new List<Song>();

            Song prev = null;

            Console.WriteLine("Ввод названий и авторов 4 песен");

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Console.WriteLine("Введите название песни: ");
                    string name = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        throw new ArgumentException("Ошибка: Название песни не может быть пустым");
                    }

                    Console.WriteLine("Введите автора песни: ");
                    string author = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(author))
                    {
                        throw new ArgumentException("Ошибка: Имя автора песни не может быть пустым");
                    }

                    Song song;

                    if (i == 0)
                    {
                        song = new Song(name, author);
                    }
                    else
                    {
                        song = new Song(name, author, prev);
                    }

                    songs.Add(song);
                    prev = song;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);

                    return;
                }
            }

            Console.WriteLine("Вывод информации о песнях: ");

            for (int i = 0; i < songs.Count; i++)
            {
                string line = $"{i + 1}. {songs[i].Title()}";

                if (songs[i].Prev != null)
                {
                    line += $" (предыдущая песня: {songs[i].Prev.Title()})";
                }

                Console.WriteLine(line);
            }

            Console.WriteLine("Сравнение первой и второй песни: ");

            Console.WriteLine($"1. {songs[0].Title()}\n2. {songs[1].Title()}");

            if (songs[0].Equals(songs[1]))
            {
                Console.WriteLine("Данные песни одинаковые");
            }
            else
            {
                Console.WriteLine("Данные песни разные");
            }

            Song mySong = new Song();
            Console.WriteLine($"Создана песня с помощью конструктора по умолчанию: {mySong.Title()}");

            Console.ReadKey();
        }

        #endregion
    }
}