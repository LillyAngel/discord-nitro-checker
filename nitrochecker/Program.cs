using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using Leaf.xNet;
using System.Collections.Specialized;


namespace ultimatechecker
{
    
    class Program
    {
        public static List<string> codes = new List<string>();
        public static List<string> proxies = new List<string>();
        public static string proxyType = string.Empty;
        public static string currentTmie = DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss");
        public static string webhookURL = string.Empty;
        public static int hits;
        public static int invalid;
        public static int ratelimit;
        public static int proxyError;
        public static int cheked;
        public static int comboAmount;
        public static int threads;
        public static int codetogen;
        public static int thread1;
        public static ParallelLoopResult parallelLoopResult;

        public static List<string> codesgen = new List<string>();


        public static string ultimate = @"
  _    _ _   _______ _____ __  __       _______ ______    _____ ______ _   _ ______ _____        _______ ____  _____  
 | |  | | | |__   __|_   _|  \/  |   /\|__   __|  ____|  / ____|  ____| \ | |  ____|  __ \    /\|__   __/ __ \|  __ \ 
 | |  | | |    | |    | | | \  / |  /  \  | |  | |__    | |  __| |__  |  \| | |__  | |__) |  /  \  | | | |  | | |__) |
 | |  | | |    | |    | | | |\/| | / /\ \ | |  |  __|   | | |_ |  __| | . ` |  __| |  _  /  / /\ \ | | | |  | |  _  / 
 | |__| | |____| |   _| |_| |  | |/ ____ \| |  | |____  | |__| | |____| |\  | |____| | \ \ / ____ \| | | |__| | | \ \ 
  \____/|______|_|  |_____|_|  |_/_/    \_|_|  |______|  \_____|______|_| \_|______|_|  \_/_/    \_|_|  \____/|_|  \_\   
            ";


        [STAThread]
        static void Main(string[] args)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Directory.CreateDirectory(Environment.CurrentDirectory + @"\\Results\\");
            Console.Title = "Discord Nitro Checker by ULTIMATEGENERATOR";

            Console.ForegroundColor = ConsoleColor.Blue;
      
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", ultimate));

            Console.ForegroundColor = ConsoleColor.Blue;             
            checkCodes();           
        }
        public static class Extensions
        {
            public static string Scramble( string s)
            {
                return new string(s.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
            }
        }

        public static void check (string code)
        {
            string url = "https://discordapp.com/api/v9/entitlements/gift-codes/"+code+"?with_application=false&with_subscription_plan=true";
            Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest();
            httpRequest.UserAgent = Http.ChromeUserAgent();
            if (proxyType == "HTTP")
            {
                httpRequest.Proxy = HttpProxyClient.Parse(randomProxy());
                httpRequest.Proxy.ConnectTimeout = 1000;
            }
            else if (proxyType == "SOCKS4")
            {
                httpRequest.Proxy = Socks4ProxyClient.Parse(randomProxy());
                httpRequest.Proxy.ConnectTimeout = 1000;
            }
            else if (proxyType == "SOCKS5")
            {
                httpRequest.Proxy = Socks5ProxyClient.Parse(randomProxy());
                httpRequest.Proxy.ConnectTimeout = 1000;

            }

            try
            {

                var respionse = httpRequest.Get(url);

                if (respionse.ToString().Contains("redeemed"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[+] HIT: " + code);
                    writeFile(code);
                    hits += 1;
                    cheked += 1;
                    if(webhookURL!= String.Empty)
                    {
                        sendWebhookMSG(webhookURL, "ULTIMATE NITRO CHECKER", "VALID NITRO HIT!: discord.gift/" + code);
                    }
                }
            }
            catch(Leaf.xNet.HttpException ex)
            {
                if (ex.Message.Contains("404"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("[-] Invalid Code: " + code);
                    invalid += 1;
                    cheked += 1;
                }
                if (ex.Message.Contains("429"))
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("[#] Rate limited: " + code);
                    check(code);
                    ratelimit += 1;
                }
                if (ex.Status.Equals(HttpExceptionStatus.ConnectFailure))
                {
                    proxyError += 1;
                    check(code);
                   
                }
            }
        }
        
        public static void writeFile(string code)
        {
            string file = Environment.CurrentDirectory + @"\\Results\\[Hits] " + currentTmie + ".txt";
            File.AppendAllText(file, code + Environment.NewLine);
        }
        public static void seperateThread()
        {
            parallelLoopResult = Parallel.ForEach(codes, code => {
                new ParallelOptions {
                    MaxDegreeOfParallelism = threads 
                    
                };
                check(code);
                Console.Title = $"Discord Nitro Checker by ULTIMATEGENERATOR | Hits: {hits} | Invalid: {invalid} | Proxy Error: {proxyError} | Rate limited: {ratelimit} | Remaining: {comboAmount - cheked} ";
            });
            
        }
        public static string randomProxy()
        {
            Random random = new Random();
            string[] ararayProx = proxies.ToArray();
            int indexx = random.Next(ararayProx.Length) ;
            return ararayProx[indexx];
        }
        public static void sendWebhookMSG(string url, string user, string content)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.UploadValues(url, new NameValueCollection
                {
                    {
                        "content", content
                    },
                    {
                        "username", user
                    }
                });
            }
            catch(WebException ex)
            {
                Console.WriteLine(ex.Message);  
            }
        }

        
        public static void checkCodes()
        {
            Console.Clear();
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", ultimate));


            Console.WriteLine("[!] OPEN CODES FILE THEN PROXY FILE.");
            Console.Write("> ");

            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "TEXT FILES (*.txt)|*.txt";
            opf.Title = "OPEN A CODES FILE";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                foreach (string code in File.ReadAllLines(opf.FileName))
                {
                    codes.Add(code);
                }
            }

            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "Text files (*.txt)|*.txt";
            pfd.Title = "Open a proxy file";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                foreach (string code in File.ReadAllLines(pfd.FileName))
                {
                    proxies.Add(code);
                }
            }

            comboAmount = codes.Count;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[!] Successfully inserted {codes.Count} code(s) and {proxies.Count} proxie(s) \n");

            Console.Write("[!] HOW MANY THREADS: \n");
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.Blue;
            string threadsS = Console.ReadLine();
            threads = Convert.ToInt32(threadsS);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[!] PROXY TYPE: [1] HTTP [2] SOCKS4 [3] SOCKS5 \n");
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.Blue;
            string prox = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[!] DO YOU WANT TO USE DISCORD WEBHOOKS? [1] YES [2] NO \n");
            Console.Write("> ");

            Console.ForegroundColor = ConsoleColor.Blue;
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Enter your discord webhook URL \n");
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Blue;
                string url = Console.ReadLine();
                webhookURL = url;
            }

            switch (prox)
            {
                case "1":
                    proxyType = "HTTP";
                    break;
                case "2":
                    proxyType = "SOCKS4";
                    break;
                case "3":
                    proxyType = "SOCKS5";
                    break;
            }

            Console.Clear();
            Thread thread = new Thread(seperateThread);
            thread.IsBackground = true;
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", ultimate));
            thread.Start();

            while(parallelLoopResult.IsCompleted != true)
            {
                if (parallelLoopResult.IsCompleted)
                {
                    Console.Title = $"Discord Nitro Checker & Generator by wulu#0827 | Hits: {hits} | Invalid: {invalid} | Proxy Error: {proxyError} | Rate limited: {ratelimit} | Remaining: {comboAmount - cheked} ";
                    Console.WriteLine($"[!] Finished checking codes. Results: Hits: {hits} | Invalid: {invalid} | Proxy Error: {proxyError} | Rate limited: {ratelimit} | Remaining: {comboAmount - cheked}");
                    Console.ReadLine();
                }
            }
        }
    }
}
