using System;

namespace tumakov_8.Classes
{
    public class Song
    {
        private string _name;
        private string _author;
        private Song _prev;

        public Song()
        {
            _name = "Неизвестно";
            _author = "Неизвестно";
            _prev = null;
        }

        public Song(string name, string author)
        {
            FillName(name);
            FillAuthor(author);
            FillPrev(null);
        }

        public Song(string name, string author, Song prev)
        {
            FillName(name);
            FillAuthor(author);
            FillPrev(prev);
        }

        /// <summary>
        /// Метод для заполнения поля name
        /// </summary>
        /// <param name="name">Название песни</param>
        /// <exception cref="ArgumentException"></exception>
        public void FillName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Ошибка: Название песни не может быть пустым");
            }

            _name = name;
        }

        /// <summary>
        /// Метод для заполнения поля author
        /// </summary>
        /// <param name="author">Имя автора песни</param>
        /// <exception cref="ArgumentException"></exception>
        public void FillAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Ошибка: Имя автора песни не может быть пустым");
            }

            _author = author;
        }

        /// <summary>
        /// Метод для заполнения поля prev
        /// </summary>
        /// <param name="prev">Предыдущая песня</param>
        public void FillPrev(Song prev)
        {
            _prev = prev;
        }

        public Song Prev
        {
            get { return _prev; }
        }

        /// <summary>
        /// Метод для печати информации о песне
        /// </summary>
        /// <returns>Информация о песне, включая минимум её название и автора</returns>
        public string Title()
        {
            return $"{_name} - {_author}";
        }

        /// <summary>
        /// Сравнивает текущую песню с другим объектом
        /// </summary>
        /// <param name="obj">Объект для сравнения, другая песня</param>
        /// <returns>True - если название и автор совпадают, иначе - false</returns>
        public override bool Equals(object obj)
        {
            Song otherSong = obj as Song;

            if (otherSong == null)
            {
                return false;
            }

            return _name == otherSong._name && _author == otherSong._author;
        }
    }
}