using latypova_8.Classes;
using latypova_8.Enums;
using System;
using System.Collections.Generic;

namespace latypova_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task();
        }

        #region Задание

        public static void Task()
        {
            Console.WriteLine("Задание - Написать систему управления проектами");

            User customer = new User("Иван Иваныч Иванов", "Квадрат");
            User teamLead = new User("Евгений");

            List<User> users = new List<User>
            {
                new User("Сергей"),
                new User("Марго"),
                new User("Акакий"),
                new User("Джордан"),
                new User("Руслан"),
                new User("Зинаида"),
                new User("Алла"),
                new User("Настя"),
                new User("Аполлон"),
                new User("Луиджи"),
            };
            
            Project project = new Project("Разработка сайта архитектурного/строительного бюро", new DateTime(2027, 02, 15), customer, teamLead);
            project.Print();

            Console.WriteLine("\nПроверка: Попытка сотрудника, не тимлида, создать задачу");
            try
            {
                project.AddTask("Реализация интерактивного портфолио", new DateTime(2026, 11, 28), users[3], users[6]);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            Console.WriteLine($"\nЗадачи по проекту '{project.Description}': ");

            project.AddTask("Проектирование интерактивной карты объектов", new DateTime(2026, 11, 25), teamLead, users[8]);
            project.AddTask("Разработка сложной структуры портфолио", new DateTime(2026, 11, 28), teamLead, users[5]);
            project.AddTask("Разработка кастомной 3D-визуализации", new DateTime(2026, 12, 01), teamLead, users[0]);
            project.AddTask("Верстка по макетам", new DateTime(2026, 12, 05), teamLead, users[1]);
            project.AddTask("Валидация форм на клиенте", new DateTime(2026, 12, 11), teamLead, users[4]);
            project.AddTask("Реализация отправки уведомлений", new DateTime(2026, 12, 13), teamLead, users[8]);
            project.AddTask("Проектирование и создание базы данных", new DateTime(2026, 12, 17), teamLead, users[9]);
            project.AddTask("Разработка серверного API", new DateTime(2026, 12, 18), teamLead, users[7]);
            project.AddTask("Управление ролями и правами доступа", new DateTime(2026, 12, 29), teamLead, users[6]);
            project.AddTask("Настройка сервера и деплой", new DateTime(2027, 01, 22), teamLead, users[3]);
            project.AddTask("Базовое тестирование", new DateTime(2027, 02, 01), teamLead, users[2]);

            WorkTask firstTask = project.Tasks[0];
            WorkTask workTask = project.Tasks[8];

            Console.WriteLine("\nПроверка: отказ от задач, делегирование, принятие задач, ввод проекта в исполнение");

            try
            {
                firstTask.RefuseTask(users[2]);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            firstTask.RefuseTask(users[8]);

            try
            {
                project.ProjectInWork(teamLead);
            }
            catch (Exception ex)
            {
                ShowError (ex.Message);
            }

            Console.WriteLine();
            workTask.DelegateTask(users[6], users[5]);
            firstTask.Reassign(teamLead, users[7]);
            firstTask.RefuseTask(users[7]);
            project.DeleteTask(teamLead, firstTask);

            Console.WriteLine("\nЗапуск проекта в работу: ");
            project.ProjectInWork(teamLead);

            Console.WriteLine("\nПередача задачи, принятие в работу: ");
            workTask.TakeTask(users[5]);
            workTask.DelegateTask(users[5], users[0]);
            workTask.TakeTask(users[0]);

            Console.WriteLine("\nСдача отчётов по задаче: ");
            workTask.TurnReport(users[0], "Настроил роли для админа и контентщика в CMS, доступ проверил, все работает корректно");
            workTask.RefineReport(teamLead);
            workTask.TurnReport(users[0], "Поправил валидацию токенов и убрал лишние логи в коде");
            workTask.ApproveReport(teamLead);

            Console.WriteLine("\nПроверка: попытка запустить проект до завершения задач");
            try
            {
                project.CompleteProject(teamLead);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            Console.WriteLine("\nАвтоматическое выполнение всех других задач, сдача отчётов: ");
            foreach (WorkTask task in project.Tasks)
            {
                if (task.Status != WorkTaskStatus.Completed)
                {
                    try
                    {
                        if (task.Status is WorkTaskStatus.Assigned)
                        {
                            task.TakeTask(task.Executor);
                        }

                        if (task.Status is WorkTaskStatus.InProgress)
                        {
                            task.TurnReport(task.Executor, "Задача выполнена, суть корректна");
                        }

                        if (task.Status is WorkTaskStatus.UnderReview)
                        {
                            task.ApproveReport(teamLead);
                        }

                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                    }
                }
            }

            project.CompleteProject(teamLead);

            Console.ReadKey();
        }

        /// <summary>
        /// Вызывает сообщение об ошибке в консоль
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public static void ShowError(string message)
        {
            Console.WriteLine($"Ошибка: {message}");
        }

        #endregion
    }
}