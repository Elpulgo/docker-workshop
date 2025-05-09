namespace Saruman.Core.Helpers;

public static class LogHelper
{
    public static void WrapLogOutput(Action action)
    {
        Console.WriteLine();
        Console.WriteLine("###########  @@@@@@@@@  #############");
        
        action();
        
        Console.WriteLine("... and so the party moved on..");
        Console.WriteLine();
    }
}