using System;

namespace latypova_8.Classes
{
    public class User
    {
        private string _name;
        private string _company;

        public User(string name)
        {
            Name = name;
            _company = "Наша IT-компания";
        }

        public User(string name, string company)
        {
            Name = name;
            Company = company;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Имя пользователя не может быть пустым");
                }

                _name = value;
            }
        }

        public string Company
        {
            get { return _company; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название компании не может быть пустым");
                }

                _company = value;
            }
        }

        /// <summary>
        /// Переопределяет ToString, возвращает строковое представление о пользователе в формате: Имя Компания
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} ({Company})";
        }
    }
}