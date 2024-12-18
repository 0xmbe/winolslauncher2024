using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
public class Program
{
    public static void Main(string[] args)
    {
        Console.Write(@"
 __      ___      ___  _    ___   _                      _              ___ __ ___ _ _  
 \ \    / (_)_ _ / _ \| |  / __| | |   __ _ _  _ _ _  __| |_  ___ _ _  |_  )  \_  ) | | 
  \ \/\/ /| | ' \ (_) | |__\__ \ | |__/ _` | || | ' \/ _| ' \/ -_) '_|  / / () / /|_  _|
  _\_/\_/ |_|_||_\___/|____|___/ |____\__,_|\_,_|_||_\__|_||_\___|_|   /___\__/___| |_| 
 | |__ _  _  |  \/  | |__  ___ _ _  ___ __ _                                            
 | '_ \ || | | |\/| | '_ \/ -_) ' \(_-</ _` |                                           
 |_.__/\_, | |_|  |_|_.__/\___|_||_/__/\__,_|                                           
       |__/                                                                             
");
        string path = GetPathToOLS();
        DateTime originaltDate = DateTime.Now;
        int newYear = 2021; int newMonth = 11; int newDay = 11;
        DateTime changedDate = new DateTime((int)newYear, (int)newMonth, (int)newDay, originaltDate.Hour, originaltDate.Minute, originaltDate.Second);
        ApplyThisDate(changedDate);
        try
        {
            Process process = StartProcessWithoutElevation(path);
            Console.WriteLine($"--------------------");
            Console.WriteLine($"CHANGE CURRENT DATE");
            Console.WriteLine($"--------------------");
            Console.WriteLine($"<Press Key>");
            Console.WriteLine($"(1) Original Date");
            Console.WriteLine($"(2) Changed Date");
            while (true)
            {
                Thread.Sleep(1000);
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.D1)
                    {
                        ApplyThisDate(originaltDate);
                    }
                    else if (key == ConsoleKey.D2)
                    {
                        ApplyThisDate(changedDate);
                    }
                }
                Process[] processes = Process.GetProcesses();
                bool isExeRunning = IsProcessRunning(process.ProcessName);
                if (isExeRunning == false)
                {
                    Console.WriteLine($"Process Closed: {process.ProcessName}");
                    ApplyThisDate(originaltDate);
                    Thread.Sleep(4000);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    static bool IsProcessRunning(string processName)
    {
        Process[] processes = Process.GetProcesses();
        foreach (var p in processes)
        {
            if (p.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
    public static Process StartProcessWithoutElevation(string executablePath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            UseShellExecute = false
        };
        Console.WriteLine($"Starting: {startInfo.FileName.ToString()}");

        return Process.Start(startInfo);
    }
    static void ApplyThisDate(DateTime _date)
    {
        Process.Start("cmd", "/c DATE " + _date.ToShortDateString());
        Thread.Sleep(500);
        Console.WriteLine($"Date Set: {DateTime.Now}");
    }
    static string GetPathToOLS()
    {
        string path1 = @"C:\Program Files (x86)\EVC\WinOLS\ols.exe";
        string path2 = @"C:\Program Files\EVC\WinOLS\ols.exe";

        if (File.Exists(path1))
        {
            return path1;
        }
        else if (File.Exists(path2))
        {
            return path2;
        }
        else
        {
            Console.WriteLine("WinOLS not found! Exiting");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
        return null;
    }
}
