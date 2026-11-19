using System;
using latypova_8.Enums;
using System.Collections.Generic;

namespace latypova_8.Classes
{
    public class Project
    {
        private string _description;
        private DateTime _deadline;
        private User _customer;
        private User _teamLead;
        private List<WorkTask> _tasks;
        private ProjectStatus _status;

        public Project(string description, DateTime deadline, User customer, User teamLead)
        {
            Description = description;
            Deadline = deadline;
            Customer = customer;
            TeamLead = teamLead;
            _tasks = new List<WorkTask>();
            _status = ProjectStatus.Draft;
        }

        public string Description
        { 
            get { return _description; } 
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Описание проекта не может быть пустым");
                }

                _description = value;
            }
        }

        public DateTime Deadline
        {
            get { return _deadline; }
            set { _deadline = value; }
        }

        public User Customer
        {
            get { return _customer; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException("У проекта должен быть инициатор (заказчик)");
                }

                _customer = value;
            }
        }

        public User TeamLead
        {
            get { return _teamLead; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException("У проекта должен быть тимлид");
                }

                _teamLead = value;
            }
        }

        public List<WorkTask> Tasks
        {
            get { return _tasks; }
        }

        public ProjectStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// Создает новую задачу и добавляет в проект
        /// </summary>
        /// <param name="description">Описание задачи</param>
        /// <param name="deadline">Дедлайн задачи</param>
        /// <param name="teamlead">Тимлид проекта</param>
        /// <param name="firstExecutor">Первый исполнитель задачи</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddTask(string description, DateTime deadline, User teamlead, User firstExecutor)
        {
            if (_status != ProjectStatus.Draft)
            {
                throw new ArgumentException("Задачу можно создать только тогда, когда проект находится в статусе: Проект");
            }

            if (teamlead != TeamLead)
            {
                throw new ArgumentException("Задачи могут назначаться только тимлидом");
            }

            if (deadline > Deadline)
            {
                throw new ArgumentException("Дедлайн задачи не должен быть позже дедлайна проекта");
            }

            WorkTask task = new WorkTask(description, deadline, teamlead, firstExecutor);
            _tasks.Add(task);

            Console.WriteLine($"Тимлид {teamlead.Name} создал задачу '{task.Description}'. Предварительный исполнитель задачи: {firstExecutor.Name}");
        }

        /// <summary>
        /// Удаляет отклоненную исполнителем задачу из проекта
        /// </summary>
        /// <param name="teamlead">Тимлид проекта</param>
        /// <param name="task">Задача из проекта</param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteTask(User teamlead, WorkTask task)
        {
            if (teamlead != TeamLead)
            {
                throw new ArgumentException("Только тимлид может удалить задачу из проекта в случае отклонения задачи предполагаемым исполнителем");
            }

            if (!_tasks.Contains(task) || task is null)
            {
                throw new ArgumentException("Такой задачи нет в данном проекте");
            }

            if (task.Executor != null)
            {
                throw new ArgumentException("Можно удалить только ту задучу, от которой отказались");
            }

            _tasks.Remove(task);

            Console.WriteLine($"Тимлид {teamlead.Name} удалил задачу '{task.Description}' из проекта. Нынешнее количество задач в проекте: {_tasks.Count}");
        }

        /// <summary>
        /// Переводит проект из статуса: Проект в статус: Исполнение
        /// </summary>
        /// <param name="teamLead">Тимлид проекта</param>
        /// <exception cref="ArgumentException"></exception>
        public void ProjectInWork(User teamLead)
        {
            if (teamLead != _teamLead)
            {
                throw new ArgumentException("Привести проект в статус: Исполнение может только тимлид");
            }

            if (_status != ProjectStatus.Draft)
            {
                throw new ArgumentException("Проект уже запущен или завершен");
            }

            if (_tasks.Count == 0)
            {
                throw new ArgumentException("Проект нельзя запустить без задач");
            }

            foreach (var task in _tasks)
            {
                if (task.Executor is null)
                {
                    throw new ArgumentException($"Задача '{task.Description}' не имеет исполнителя. Проект не может иметь статус: Исполнение");
                }
            }

            _status = ProjectStatus.InProgress;

            Console.WriteLine($"Проект '{Description}' запущен. Статус: Исполнение");
        }

        /// <summary>
        /// Завершает проект и переводит его в статус: Закрыт
        /// </summary>
        /// <param name="teamLead">Тимлид проекта</param>
        /// <exception cref="ArgumentException"></exception>
        public void CompleteProject(User teamLead)
        {
            if (teamLead != _teamLead)
            {
                throw new ArgumentException("Закрыть проект может только тимлид");
            }

            if (_status != ProjectStatus.InProgress)
            {
                throw new ArgumentException("Проект еще не в исполнении или уже закрыт");
            }

            foreach (var task in _tasks)
            {
                if (task.Status != WorkTaskStatus.Completed)
                {
                    throw new ArgumentException($"Задача '{task.Description}' не завершена (нынешний статус: {task.Status}). Проект не может иметь статус: Закрыт");
                }
            }

            _status = ProjectStatus.Closed;

            Console.WriteLine($"Проект '{Description}' завершен. Статус: Закрыт");
        }

        /// <summary>
        /// Выводит основную информацию о проекте на консоль
        /// </summary>
        public void Print()
        {
            Console.WriteLine($"\nПроект: {Description}\nЗаказчик: {Customer}\nОтветственный (тимлид): {_teamLead.Name}\nСтатус проекта: {_status}");
        }
    }
}