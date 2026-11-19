using System;
using latypova_8.Enums;

namespace latypova_8.Classes
{
    public class WorkTask
    {
        private string _description;
        private DateTime _deadline;
        private User _initiator;
        private User _executor;
        private WorkTaskStatus _status;
        private Report _taskReport;

        public WorkTask(string description, DateTime deadline, User initiator, User firstExecutor)
        {
            Description = description;
            Deadline = deadline;
            Initiator = initiator;
            _executor = firstExecutor;
            _status = WorkTaskStatus.Assigned;
            _taskReport = null;
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Описание задачи не может быть пустым");
                }

                _description = value;
            }
        }

        public DateTime Deadline
        {
            get { return _deadline; }
            set { _deadline = value; }
        }

        public User Initiator
        {
            get { return _initiator; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException("У задачи должен быть инициатор");
                }

                _initiator = value;
            }
        }

        public User Executor
        {
            get { return _executor; }
        }

        public WorkTaskStatus Status
        {
            get { return _status; }
        }

        public Report TaskReport
        {
            get { return _taskReport; }
        }

        /// <summary>
        /// Переопределяет исполнителя задачи после отклонения прошлого исполнителя
        /// </summary>
        /// <param name="initiator">Инициатор задачи</param>
        /// <param name="nextExecutor">Новый исполнитель</param>
        /// <exception cref="ArgumentException"></exception>
        public void Reassign(User initiator, User nextExecutor)
        {
            if (initiator != Initiator)
            {
                throw new ArgumentException("Переназначить задачу может только её инициатор");
            }

            if (_executor != null)
            {
                throw new ArgumentException("Инициатор не может переназначить задачу, которая не отклонена исполнителем");
            }

            if (nextExecutor is null)
            {
                throw new ArgumentException("Новый исполнитель задачи не может быть пустым");
            }

            _executor = nextExecutor;
            _status = WorkTaskStatus.Assigned;

            Console.WriteLine($"Пользователь {initiator.Name} назначил задачу '{Description}' исполнителю {_executor.Name}. Статус: Назначена");
        }

        /// <summary>
        /// Переводит задачу из статуса: Назначена в статус: В работе
        /// </summary>
        /// <param name="executor">Исполнитель задачи</param>
        /// <exception cref="ArgumentException"></exception>
        public void TakeTask(User executor)
        {
            if (_status != WorkTaskStatus.Assigned)
            {
                throw new ArgumentException("Взять задачу можно только со статусом: Назначена");
            }

            if (executor != _executor)
            {
                throw new ArgumentException("Взять задачу может только человек, которому она назначена");
            }

            _status = WorkTaskStatus.InProgress;

            Console.WriteLine($"Задача '{Description}' взята в работу, исполнитель: {executor.Name}. Статус: В работе");
        }

        /// <summary>
        /// Передает задачу другому исполнителю и возвращает задачу в статус: Назначена
        /// </summary>
        /// <param name="currentExecutor">Текущий исполнитель задачи</param>
        /// <param name="nextExecutor">Новый исполнитель, которому передается задача</param>
        /// <exception cref="ArgumentException"></exception>
        public void DelegateTask(User currentExecutor, User nextExecutor)
        {
            if (currentExecutor != _executor)
            {
                throw new ArgumentException("Делегировать задачу может только тот, кому она была до этого назначена");
            }
            
            if (nextExecutor is null)
            {
                throw new ArgumentException("Нельзя делегировать задачу пустому исполнителю");
            }

            if (_status != WorkTaskStatus.Assigned && _status != WorkTaskStatus.InProgress)
            {
                throw new ArgumentException("Задачу нельзя делегировать, если она не в статусах: Назначена, В работе");
            }

            User oldExecutor = _executor;
            _executor = nextExecutor;
            _status = WorkTaskStatus.Assigned;

            Console.WriteLine($"Задача '{Description}' делегирована от {oldExecutor.Name} исполнителю {_executor.Name}. Статус: Назначена");
        }

        /// <summary>
        /// Отклоняет задачу текущим исполнителем
        /// </summary>
        /// <param name="executor">Текущий исполнитель</param>
        /// <exception cref="ArgumentException"></exception>
        public void RefuseTask(User executor)
        {
            if (executor != _executor)
            {
                throw new ArgumentException("Отклонить задачу может только тот, кому она была назначена");
            }

            if (_status != WorkTaskStatus.Assigned)
            {
                throw new ArgumentException("Задачу можно отклонить, только если её статус: Назначена");
            }

            User exec = _executor;
            _executor = null;

            Console.WriteLine($"{exec.Name} отклонил задачу '{Description}'. Теперь задача свободна, её можно назначить другому или удалить");
        }

        /// <summary>
        /// Создает и загружает отчет о задаче на проверку инициатору задачи
        /// </summary>
        /// <param name="author">Автор (исполнитель) задачи</param>
        /// <param name="reportText">Текст отчета</param>
        /// <exception cref="ArgumentException"></exception>
        public void TurnReport(User author, string reportText)
        {
            if (author != _executor)
            {
                throw new ArgumentException("Сдать отчет может только тот, кто занимался задачей, т.е. её исполнитель");
            }

            if (_status != WorkTaskStatus.InProgress)
            {
                throw new ArgumentException("Отчет по задаче можно сдать тогда, когда у задачи статус: В работе");
            }

            _taskReport = new Report(reportText, author);
            _status = WorkTaskStatus.UnderReview;

            Console.WriteLine($"Отчет '{reportText}' по задаче '{Description}' исполнителя: {author.Name} - отправлен на проверку инициатору ({Initiator.Name}). Статус: На проверке");
        }

        /// <summary>
        /// Утверждает отчет инициатором и переводит задачу в статус: Выполнена
        /// </summary>
        /// <param name="supervisor">Инициатор задачи, проверяющий отчет</param>
        /// <exception cref="ArgumentException"></exception>
        public void ApproveReport(User supervisor)
        {
            if (supervisor != Initiator)
            {
                throw new ArgumentException("Утвердить отчет может только инициатор задачи");
            }

            if ( _status != WorkTaskStatus.UnderReview)
            {
                throw new ArgumentException("Утвердить отчет по задаче можно только тогда, когда статус задачи: На проверке");
            }

            _status = WorkTaskStatus.Completed;

            Console.WriteLine($"Инициатор {supervisor.Name} утвердил отчет '{_taskReport.Text}' по задаче '{Description}'. Статус: Выполнена");
        }

        /// <summary>
        /// Возвращает отчет на доработку инициатором задачи и возвращает задачу в статус: В работе
        /// </summary>
        /// <param name="supervisor">Инициатор задачи, проверяющий отчет</param>
        /// <exception cref="ArgumentException"></exception>
        public void RefineReport(User supervisor)
        {
            if (supervisor != Initiator)
            {
                throw new ArgumentException("Передать отчет на доработку может только инициатор задачи");
            }

            if (_status != WorkTaskStatus.UnderReview)
            {
                throw new ArgumentException("Передать отчет на доработку можно только тогда, когда статус задачи: На проверке");
            }

            Report taskReport = _taskReport;
            _status = WorkTaskStatus.InProgress;
            _taskReport = null;

            Console.WriteLine($"Инициатор {supervisor.Name} передал отчет '{taskReport.Text}' по задаче '{Description}' на доработку. Статус: В работе");
        }
    }
}