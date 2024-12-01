using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
// Интерфейс команды
public interface ICommand
{
    void Execute();
    void Undo();
}
// Команда для вставки текста
public class InsertTextCommand : ICommand
{
    private readonly TextEditor _textEditor;
    private readonly string _textToInsert;
    public InsertTextCommand(TextEditor textEditor, string textToInsert)
    {
        _textEditor = textEditor;
        _textToInsert = textToInsert;
    }
    public void Execute()
    {
        _textEditor.InsertText(_textToInsert);
    }
    public void Undo()
    {
        _textEditor.DeleteText(_textToInsert.Length);
    }
}
// Команда для удаления текста
public class DeleteTextCommand : ICommand
{
    private readonly TextEditor _textEditor;
    private readonly int _length;
    public DeleteTextCommand(TextEditor textEditor, int length)
    {
        _textEditor = textEditor;
        _length = length;
    }
    public void Execute()
    {
        _textEditor.DeleteText(_length);
    }

    public void Undo()
    {
        _textEditor.UndoDelete(_length);
    }
}
// Класс текстового редактора
public class TextEditor
{
    private string _text = string.Empty;
    private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();
    public void InsertText(string text)
    {
        _text += text;
        Console.WriteLine($"Текущий текст: {_text}");
    }
    public void DeleteText(int length)
    {
        if (length > _text.Length)
        {
            length = _text.Length;
        }

        _text = _text.Substring(0, _text.Length - length);
        Console.WriteLine($"Текущий текст: {_text}");
    }
    public void UndoDelete(int length)
    {
        // Восстановление текста (добавление пробелов для простоты)
        _text += new string(' ', length);
        Console.WriteLine($"Текущий текст: {_text}");
    }
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
    }
    public void UndoLastCommand()
    {
        if (_commandHistory.Count > 0)
        {
            var lastCommand = _commandHistory.Pop();
            lastCommand.Undo();
        }
        else
        {
            Console.WriteLine("Нет команд для отмены.");
        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        var textEditor = new TextEditor();

        // Ввод текста
        var insertCommand = new InsertTextCommand(textEditor, "Hello, World!");
        textEditor.ExecuteCommand(insertCommand);

        // Удаление текста
        var deleteCommand = new DeleteTextCommand(textEditor, 6);
        textEditor.ExecuteCommand(deleteCommand);

        // Отмена последней операции (удаление)
        textEditor.UndoLastCommand();

        // Отмена последней операции (ввод)
        textEditor.UndoLastCommand();
    }
}