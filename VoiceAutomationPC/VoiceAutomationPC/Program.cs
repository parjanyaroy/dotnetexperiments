    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dropbox.Api;

    namespace VoiceAutomationPC
    {
    class Program
    {
    static int requestInterval = 50;
    static Dictionary<String,String> websiteMap= null;
    static Dictionary<String, String> commandMap = null;
    static DropboxClient dbx = null;
    static string nircmdPath = null;//"C:\\Test\\";
    static string browserPath = null;// "C:\\Users\\parjroy\\AppData\\Local\\Vivaldi\\Application\\vivaldi.exe";
    static string token = null;//"_wQHTitZaWoAAAAAAAAVzq7yC6W0cQvx6r_pVJFZiUpRRcPlMwzpnWYqXzDA1udw";
    static string dropboxFolderPath = null;//"/AmazonAlexa";

    static void Main(string[] args){
            try
            {
                Console.WriteLine("______ _____    ___        _                        _   _             ");
                Console.WriteLine("| ___ /  __ \\  / _ \\      | |                      | | (_)            ");
                Console.WriteLine("| |_/ | /  \\/ / /_\\ \\_   _| |_ ___  _ __ ___   __ _| |_ _  ___  _ __  ");
                Console.WriteLine("|  __/| |     |  _  | | | | __/ _ \\| '_ ` _ \\ / _` | __| |/ _ \\| '_ \\ ");
                Console.WriteLine("| |   | \\__/\\ | | | | |_| | || (_) | | | | | | (_| | |_| | (_) | | | |");
                Console.WriteLine("\\_|    \\____/ \\_| |_/\\__,_|\\__\\___/|_| |_| |_|\\__,_|\\__|_|\\___/|_| |_|");
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("");
                Console.WriteLine("Created By : Parjanya Roy");
                Console.WriteLine("Created On : 11.03.2019");
                Console.WriteLine("Contact    : parjanyaroy@gmail.com");
                Console.WriteLine("");
                initializeWebsiteMap();
                initializeCommandMap();
                readConfigurationFromFile();
                Console.WriteLine("");
                Console.WriteLine("Configuration and Commands are now intialized.Waiting for Input .....");
                Console.WriteLine("");
                dbx = new DropboxClient(token);
                
            }
            catch(Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString() + " Exception At Data Setup :: Message ::    " + e.Message);
                Console.WriteLine(DateTime.Now.ToString() + " Exception At Data Setup :: Stacktrace :: " + e.Message);
            }
    for (;;){
    Thread.Sleep(requestInterval);
                try { 
    var task = Task.Run((Func<Task>)Program.CheckForRequests);
    task.Wait();
                }
                catch(Exception e)
                {
                    Console.WriteLine(DateTime.Now.ToString() + " Exception At Automation Process :: Message ::    " + e.Message);
                    Console.WriteLine(DateTime.Now.ToString() + " Exception At Automation Process :: Stacktrace :: " + e.Message);
                }
    }
    }

    private static void readConfigurationFromFile(){
      using (var reader = new System.IO.StreamReader(System.IO.Directory.GetCurrentDirectory() + "\\configFile.txt")){
                while (!reader.EndOfStream){
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (values[0].Trim().Equals("nircmdPath")){
                        nircmdPath = values[1];
                        Console.WriteLine(DateTime.Now.ToString() + " nircmdPath :: " + values[1]);
                    }
                    else if (values[0].Trim().Equals("browserPath")){
                        browserPath = values[1];
                        Console.WriteLine(DateTime.Now.ToString() + " browserPath :: " + values[1]);
                    }
                    else if (values[0].Trim().Equals("token")){
                        token = values[1];
                        Console.WriteLine(DateTime.Now.ToString() + " token :: " + values[1]);
                    }
                    else if (values[0].Trim().Equals("dropboxFolderPath")){
                        dropboxFolderPath = values[1];
                        Console.WriteLine(DateTime.Now.ToString() + " dropboxFolderPath :: " + values[1]);
                    }
                }
            }
            
        }

    private static Dictionary<String,String> readMapFromFile(String fileName)
    {
            Dictionary<String, String> fetchedMap = new Dictionary<string, string>();
            using (var reader = new System.IO.StreamReader(System.IO.Directory.GetCurrentDirectory()+"\\"+fileName))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    fetchedMap.Add(values[0], values[1]);
                }
            }
            return fetchedMap;
        }

    private static void initializeWebsiteMap()
    {
    websiteMap = readMapFromFile("websitemapfile.txt");
    }

    private static void initializeCommandMap()
    {
    commandMap = readMapFromFile("commandmapfile.txt");
    }

    static async Task CheckForRequests()
    {
    var list = await dbx.Files.ListFolderAsync(dropboxFolderPath);
    foreach (var item in list.Entries.Where(i => i.IsFile))
    {
    if (item.Name != "doasearch.txt"){
    ReadAssistantCommand(item.Name);
    }
    else{
    using (var response = await dbx.Files.DownloadAsync(dropboxFolderPath + "/"+item.Name))
     {
         String queryParam = await response.GetContentAsStringAsync();
         openBrowser("https://www.google.com", queryParam);
      }
    }
    await dbx.Files.DeleteAsync(dropboxFolderPath +"/" + item.Name);
    }
    dbx.Files.CreateFolderAsync(dropboxFolderPath);

    
    }
    
    static void ReadAssistantCommand(string command)
    {
    if (commandMap.ContainsKey(command))
    {
    String commandString = null;
    commandMap.TryGetValue(command, out commandString);
    if (null != commandString)
    ExecuteCommand(commandString);
    }
    else if (websiteMap.ContainsKey(command))
    {
    String webUrl = null;
    websiteMap.TryGetValue(command,out webUrl);
    if(null!= webUrl)
    openBrowser(webUrl);
    }
    else{
    Console.WriteLine(DateTime.Now.ToString() + " COMMAND_NOT_FOUND :: " + command);
    }
    }

    static void openBrowser(String website)
    {
    Console.WriteLine(DateTime.Now.ToString()+" Launching Browser :: " + website);
    var process = new ProcessStartInfo(browserPath);
    process.Arguments = website;
    Process.Start(process);
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
    Console.WriteLine(DateTime.Now.ToString() + " Executing Command :: " + command);
    process = Process.Start(processInfo);
    process.Close();
    }

    static void openBrowser(String website,String searchQuery)
        {
            Console.WriteLine(DateTime.Now.ToString() + " Searching In Google :: " + website+" For:: "+searchQuery);
            searchQuery = searchQuery.Trim();
            searchQuery = searchQuery.Replace(' ', '+');
            var process = new ProcessStartInfo(browserPath);
            process.Arguments = website+ "/search?q="+searchQuery;
            Process.Start(process);
        }
    }

    }
