# NullLib.ConsoleEx
 Extension functions for C# Console apps

> namespace root: NullLib.ConsoleEx

 ## Measure Console Text

 1. ConsoleText.CalcCharLength(char c)
  2. ConsoleText.CalcStringLength(string str)
  3. ConsoleText.IsWideChar(char c)

## Advanced Scanner

1. ConsoleSc.Prompt = "some string"
2. ConsoleSc.ReadAsync(ConsoleKey until, bool intercept)
3. ConsoleSc.ReadLine()

> And lots of overrides for more features.

```ini
# Keys and Shortcuts
Insert : Switch insert mode (when using insert mode, cursor is full size)
Backspace : Backspace (remove one character before cursor)
Delete : Delete (remove one character after cursor)
Ctrl + Backspace : Clear all characters before cursor
Ctrl + Delete : Clear all characters after cursor
Esc : Clear all characters.
Up arrow : Switch to the previous input history
Down arror : Switch to the next input history (when in history end, use this key will clear all characters)
```



## Features

```csharp
// you can write some text when you are scanning
// and it will display well

Task.Run(() => 
{
   while(true)
   {
       Thread.Sleep(1000);
       ConsoleSc.WriteLine("Some text");
   } 
});

while(true)
{
    string instr = ConsoleSc.ReadLine();
    Console.Title = instr;
}
```

