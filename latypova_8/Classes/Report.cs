using System;

namespace latypova_8.Classes
{
    public class Report
    {
        private string _text;
        private DateTime _completeDate;
        private User _author;

        public Report(string text, User author)
        {
            Text = text;
            Author = author;
            _completeDate = DateTime.Now;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Текст отчета не может быть пустым");
                }

                _text = value;
            }
        }

        public DateTime CompleteDate
        {
            get { return _completeDate; }
        }

        public User Author
        {
            get { return _author; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException("У отчета должен быть исполнитель (автор)");
                }

                _author = value;
            }
        }
    }
}