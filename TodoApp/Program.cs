using System;
using Todo.Core;

class Program
{
    static void Main()
    {
        var list = new TodoList();

        Console.WriteLine("Введите путь к JSON-файлу для загрузки (оставьте пустым, если нового файла нет):");
        string loadPath = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(loadPath))
        {
            try
            {
                list.Load(loadPath);
                Console.WriteLine($"Загружено {list.Count} задач из {loadPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
        }

        Console.WriteLine("Введите задачи (пустая строка для завершения):");
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(input))
                break;

            list.Add(input);
        }

        if (list.Count == 0)
        {
            Console.WriteLine("Список задач пуст. Выход.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nТекущие задачи:");
            for (int i = 0; i < list.Count; i++)
            {
                var item = list.Items[i];
                Console.WriteLine($"{i + 1}. {item.Title} - Done: {item.IsDone}");
            }

            Console.WriteLine("\nКоманды: done <номер>, undone <номер>, add <задача>, remove <номер>, save, exit");
            Console.Write("> ");
            string command = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(command))
                continue;

            var parts = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "done":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int doneIndex) || doneIndex < 1 || doneIndex > list.Count)
                    {
                        Console.WriteLine("Неверный номер задачи.");
                        break;
                    }
                    list.Items[doneIndex - 1].MarkDone();
                    break;

                case "undone":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int undoneIndex) || undoneIndex < 1 || undoneIndex > list.Count)
                    {
                        Console.WriteLine("Неверный номер задачи.");
                        break;
                    }
                    list.Items[undoneIndex - 1].MarkUndone();
                    break;

                case "add":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Введите текст задачи после команды add.");
                        break;
                    }
                    list.Add(parts[1]);
                    break;

                case "remove":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int removeIndex) || removeIndex < 1 || removeIndex > list.Count)
                    {
                        Console.WriteLine("Неверный номер задачи.");
                        break;
                    }
                    list.Remove(list.Items[removeIndex - 1].Id);
                    break;

                case "save":
                    Console.Write("Введите путь для сохранения файла JSON: ");
                    string savePath = Console.ReadLine()?.Trim() ?? "tasks.json";
                    try
                    {
                        list.Save(savePath);
                        Console.WriteLine($"Список задач успешно сохранён в {savePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
                    }
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }
}