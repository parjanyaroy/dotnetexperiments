using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using System.Diagnostics;

namespace ConsoleApp1
{
class Program
{
static string nircmdPath = "C:\\Test\\";
static string token = "_wQHTitZaWoAAAAAAAAVzq7yC6W0cQvx6r_pVJFZiUpRRcPlMwzpnWYqXzDA1udw";
static void Main(string[] args)
{
var task = Task.Run((Func<Task>)Program.Run1);
task.Wait();
    Task.Factory.StartNew(() =>
    {
        System.Threading.Thread.Sleep(1000);
        Console.WriteLine("Hi");
    });
}
static async Task Run1()
{
using (var dbx = new DropboxClient(token))
{
    var list = await dbx.Files.ListFolderAsync("/AmazonAlexa");
    foreach (var item in list.Entries.Where(i => i.IsFile))
    {
        Console.WriteLine(item.Name);
        ReadAssistantCommand(item.Name);
    }
                
}
}
static void ExecuteCommand(string command)
{
//int exitCode;
ProcessStartInfo processInfo;
Process process;
command = nircmdPath + command;
processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
processInfo.CreateNoWindow = true;
processInfo.UseShellExecute = false;
// *** Redirect the output ***
processInfo.RedirectStandardError = true;
processInfo.RedirectStandardOutput = true;
process = Process.Start(processInfo);
//process.WaitForExit();
// *** Read the streams ***
// Warning: This approach can lead to deadlocks, see Edit #2
//string output = process.StandardOutput.ReadToEnd();
//string error = process.StandardError.ReadToEnd();
//exitCode = process.ExitCode;
//Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
//Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
//Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
process.Close();
}

static void ReadAssistantCommand(string command)
{
if (command.Equals("shutdownpc.txt"))
{
    Console.WriteLine("Shutting Down PC");
}
else if (command.Equals("mutevolume.txt"))
{
    ExecuteCommand("nircmd.exe mutesysvolume 1");
}
else if(command.Equals("launchprime.txt"))
{
        openBrowser("https://www.primevideo.com");
}
}

static void openBrowser(String website)
{
    Console.WriteLine("Launching Browser :: "+website);
    var process = new ProcessStartInfo("C:\\Users\\parjroy\\AppData\\Local\\Vivaldi\\Application\\vivaldi.exe");
    process.Arguments = website;
    Process.Start(process);
}
}
}

