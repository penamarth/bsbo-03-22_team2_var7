﻿using System;

namespace ConsoleApp24
{
    public class MySystem
    {
        // Поля
        private Director director;
        private Employer employer;

        // Метод для создания директора
        public void CreateDirector(string fullName, string position)
        {
            Console.WriteLine("=== Создание Руководителя ===");
            Console.WriteLine($"System.CreateDirector:\n\tФИО: {fullName}\n\tДолжность: {position}");
            this.director = new Director(fullName, position, this);
            Console.WriteLine($"bd:save фио={fullName} должность={position}\n");
        }

        // Метод для создания исполнителя
        public void CreateEmployer(string fullName, string position, string directorName)
        {
            Console.WriteLine("=== Создание Исполнителя ===");
            Console.WriteLine($"System.CreateEmployer:\n\tФИО: {fullName}\n\tДолжность: {position}\n\tДиректор: {directorName}");
            this.employer = new Employer(fullName, position, directorName, this);
            Console.WriteLine($"bd:save фио={fullName} должность={position} директор={directorName}\n");
        }

        // Метод для создания задачи
        public void CreateTask(string description, string creationDate, string plannedCompletionDate,
                               string actualCompletionDate, string executor, string status, string directorName)
        {
            Console.WriteLine("=== Создание Задачи ===");
            Console.WriteLine($"System.CreateTask:\n\tОписание: {description}\n\tДата Создания: {creationDate}\n\t" +
                              $"Планируемая Дата Завершения: {plannedCompletionDate}\n\t" +
                              $"Фактическая Дата Завершения: {(actualCompletionDate ?? "Не завершена")}\n\t" +
                              $"Исполнитель: {executor}\n\tСтатус: {status}\n\tДиректор: {directorName}");
            Console.WriteLine($"bd:select director {directorName}");
            this.director.CreateTask(description, creationDate, plannedCompletionDate,
                                     actualCompletionDate, executor, status, directorName);
            Console.WriteLine();
        }

        // Метод для удаления задачи
        public void DeleteTask(int id, string directorName)
        {
            Console.WriteLine("=== Удаление Задачи ===");
            Console.WriteLine($"System.DeleteTask:\n\tID: {id}\n\tДиректор: {directorName}");
            Console.WriteLine($"bd:select director {directorName}");
            this.director.DeleteTask(id, directorName);
            Console.WriteLine();
        }

        // Метод для изменения статуса задачи начальником
        public void ChangeStatusByDirector(int id, string directorName)
        {
            Console.WriteLine("=== Изменение Статуса Задачи Начальником ===");
            Console.WriteLine($"System.ChangeStatusByDirector:\n\tID: {id}\n\tДиректор: {directorName}");
            Console.WriteLine($"bd:select director {directorName}");
            this.director.ChangeStatus(id, directorName);
            Console.WriteLine();
        }

        // Метод для изменения статуса задачи исполнителем
        public void ChangeStatusByEmployer(int id, string employerName)
        {
            Console.WriteLine("=== Изменение Статуса Задачи Исполнителем ===");
            Console.WriteLine($"System.ChangeStatusByEmployer:\n\tID: {id}\n\tИсполнитель: {employerName}");
            Console.WriteLine($"bd:select employer {employerName}");
            this.employer.ChangeStatus(id, employerName);
            Console.WriteLine();
        }

        // Метод для просмотра статистики
        public void ViewStatistics(string parameter = "any")
        {
            Console.WriteLine("=== Просмотр Статистики ===");
            Console.WriteLine($"System.ViewStatistics:\n\tПараметр: {parameter}");
            Console.WriteLine($"bd:select {parameter}");
            Console.WriteLine("Отчет\n");
        }
    }

    // Абстрактный класс состояния
    public abstract class State
    {
        public abstract State ChangeState(int id);
    }

    // Конкретные классы состояний
    public class State1 : State
    {
        public override State ChangeState(int id)
        {
            Console.WriteLine("State1.ChangeState:");
            Console.WriteLine("\tbd:upgrade id State2");
            return new State2();
        }
    }

    public class State2 : State
    {
        public override State ChangeState(int id)
        {
            Console.WriteLine("State2.ChangeState:");
            Console.WriteLine("\tbd:upgrade id State3");
            return new State3();
        }
    }

    public class State3 : State
    {
        public override State ChangeState(int id)
        {
            Console.WriteLine("State3.ChangeState:");
            Console.WriteLine("\tNone");
            return null;
        }
    }

    // Класс задачи
    public class Task
    {
        protected State currentState;
        protected MySystem system; // Ссылка на MySystem

        public Task(MySystem system)
        {
            this.system = system;
            // Инициализируем начальное состояние
            currentState = new State1();
        }

        public virtual void CreateTask(string description, string creationDate, string plannedCompletionDate,
                               string actualCompletionDate, string executor, string status, string directorName)
        {
            Console.WriteLine("Task.CreateTask:");
            Console.WriteLine($"\tОписание: {description}");
            Console.WriteLine($"\tДата Создания: {creationDate}");
            Console.WriteLine($"\tПланируемая Дата Завершения: {plannedCompletionDate}");
            Console.WriteLine($"\tФактическая Дата Завершения: {(actualCompletionDate ?? "Не завершена")}");
            Console.WriteLine($"\tИсполнитель: {executor}");
            Console.WriteLine($"\tСтатус: {status}");
            Console.WriteLine($"\tДиректор: {directorName}\n");
            Console.WriteLine($"bd:save Описание={description}, Дата_создания={creationDate}, Планируемая_дата_завершения={plannedCompletionDate}, " +
                              $"Фактическая_дата_завершения={(actualCompletionDate ?? "Не завершена")}, Исполнитель={executor}, Статус={status}\n");

        }

        public virtual void DeleteTask(int id, string directorName)
        {
            Console.WriteLine("Task.DeleteTask:");
            Console.WriteLine($"\tID: {id}");
            Console.WriteLine($"\tДиректор: {directorName}");
            Console.WriteLine("\tbd:delete Описание, Дата_создания, Планируемая_дата_завершения, " +
                              "Фактическая_дата_завершения, Исполнитель, Статус");
            ChangeCurrentState(id);
        }

        public virtual void ChangeStatus(int id, string directorName)
        {
            Console.WriteLine("Task.ChangeStatus:");
            Console.WriteLine($"\tID: {id}");
            Console.WriteLine($"\tДиректор: {directorName}");
            Console.WriteLine($"\tbd:update статус для задачи с id: {id}");
            ChangeCurrentState(id);
        }

        protected void ChangeCurrentState(int id)
        {
            if (currentState != null)
            {
                currentState = currentState.ChangeState(id);
            }
            else
            {
                Console.WriteLine("\tЗадача уже в конечном состоянии.\n");
            }
        }
    }

    // Класс Director, наследуется от Task
    public class Director : Task
    {
        private string fullName;
        private string position;

        public Director(string fullName, string position, MySystem system) : base(system)
        {
            this.fullName = fullName;
            this.position = position;
            Console.WriteLine($"Director.__init__:\n\tФИО: {fullName}\n\tДолжность: {position}\n");
        }

        public override void CreateTask(string description, string creationDate, string plannedCompletionDate,
                               string actualCompletionDate, string executor, string status, string directorName)
        {
            Console.WriteLine("Director.CreateTask:");
            base.CreateTask(description, creationDate, plannedCompletionDate, actualCompletionDate, executor, status, directorName);
        }

        public override void DeleteTask(int id, string directorName)
        {
            Console.WriteLine("Director.DeleteTask:");
            base.DeleteTask(id, directorName);
        }

        public override void ChangeStatus(int id, string directorName)
        {
            Console.WriteLine("Director.ChangeStatus:");
            base.ChangeStatus(id, directorName);
        }
    }

    // Класс Employer, наследуется от Task
    public class Employer : Task
    {
        private string fullName;
        private string position;
        private string directorName;

        public Employer(string fullName, string position, string directorName, MySystem system) : base(system)
        {
            this.fullName = fullName;
            this.position = position;
            this.directorName = directorName;
            Console.WriteLine($"Employer.__init__:\n\tФИО: {fullName}\n\tДолжность: {position}\n\tДиректор: {directorName}\n");
        }

        public void ViewStatistics(string parameter = "Executor")
        {
            Console.WriteLine("Employer.ViewStatistics:");
            Console.WriteLine($"\tПараметр: {parameter}");
            system.ViewStatistics(parameter);
        }

        public override void DeleteTask(int id, string directorName)
        {
            Console.WriteLine("Employer.DeleteTask:");
            base.DeleteTask(id, directorName);
        }

        public override void ChangeStatus(int id, string directorName)
        {
            Console.WriteLine("Employer.ChangeStatus:");
            base.ChangeStatus(id, directorName);
        }
    }

    // Класс программы
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== Начало Программы ===\n");
            var system = new MySystem();

            string directorFullName = "Иванов Иван Иванович";
            string directorPosition = "Генеральный директор";
            system.CreateDirector(directorFullName, directorPosition);

            string employerFullName = "Петров Петр Петрович";
            string employerPosition = "Менеджер";
            string directorName = directorFullName;
            system.CreateEmployer(employerFullName, employerPosition, directorName);

            string description = "Разработать новый сайт";
            string creationDate = "2024-12-13";
            string plannedCompletionDate = "2024-12-20";
            string actualCompletionDate = null;
            string executor = employerFullName;
            string status = "Создано";
            system.CreateTask(description, creationDate, plannedCompletionDate,
                              actualCompletionDate, executor, status, directorName);

            int taskId = 1;
            system.ChangeStatusByEmployer(taskId, employerFullName);
            system.ChangeStatusByEmployer(taskId, employerFullName);
            string parameter = "Общий отчет";
            system.ViewStatistics(parameter);

            Console.WriteLine("=== Конец Программы ===");
            Console.ReadLine();
        }
    }
}
