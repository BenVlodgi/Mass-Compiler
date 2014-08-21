using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MassCompiler
{
    class Program
    {
        const string CONFIG_FILE_NAME = "mass-compiler-config";

        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] { "dev_room.vmf" };
#endif

            string vbspPath = string.Empty;
            string vbspParams = string.Empty;
            string vradPath = string.Empty;
            string vradParams = string.Empty;
            string vvisPath = string.Empty;
            string vvisParams = string.Empty;
            string outputdirectory = string.Empty;

            //Above, default values are given incase we don't get info from our config file

            
            string executableDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string config_path = Path.Combine(executableDir, CONFIG_FILE_NAME).Replace("file:\\", "");
            if (File.Exists(config_path))
            {
                var config = File.ReadAllLines(config_path);
                if (config.Length > 1 && !string.IsNullOrWhiteSpace(config[1])) vbspPath = config[1];
                if (config.Length > 3 && !string.IsNullOrWhiteSpace(config[3])) vbspParams = config[3];
                if (config.Length > 5 && !string.IsNullOrWhiteSpace(config[5])) vradPath = config[5];
                if (config.Length > 7 && !string.IsNullOrWhiteSpace(config[7])) vradParams = config[7];
                if (config.Length > 9 && !string.IsNullOrWhiteSpace(config[9])) vvisPath = config[9];
                if (config.Length > 11 && !string.IsNullOrWhiteSpace(config[11])) vvisParams = config[11];
                if (config.Length > 13 && !string.IsNullOrWhiteSpace(config[13])) outputdirectory = config[13];
            }
            else
            {
                WriteLine("No config found", ConsoleColor.Red);
                args = new string[] { };
            }

            for (int i = 0; i < args.Length; i++)
            {
                DateTime start = DateTime.Now;
                try
                {
                    string filename = args[i].Replace(".vmf", "");

                    WriteLine("Compiling vmf {0} / {1} ({2})", ConsoleColor.Yellow, ConsoleColor.Black, i + 1, args.Length, filename);
                    Process proc;

                    WriteLine("Running VBSP...", ConsoleColor.Yellow);
                    Write('"' + vbspPath + '"', ConsoleColor.Black, ConsoleColor.White);
                    WriteLine(' ' + vbspParams, ConsoleColor.Cyan, ConsoleColor.Black, filename);
                    proc = Process.Start(new ProcessStartInfo(vbspPath, string.Format(vbspParams, filename)) { UseShellExecute = false, RedirectStandardOutput = true });
                    Console.WriteLine(proc.StandardOutput.ReadToEnd());
                    proc.WaitForExit();
                    WriteLine("Finished.", ConsoleColor.Yellow);


                    WriteLine("Running VVIS...", ConsoleColor.Yellow);
                    Write('"' + vvisPath + '"', ConsoleColor.Black, ConsoleColor.White);
                    WriteLine(' ' + vvisParams, ConsoleColor.Cyan, ConsoleColor.Black, filename);
                    proc = Process.Start(new ProcessStartInfo(vvisPath, string.Format(vvisParams, filename)) { UseShellExecute = false, RedirectStandardOutput = true });
                    Console.WriteLine(proc.StandardOutput.ReadToEnd());
                    proc.WaitForExit();
                    WriteLine("Finished.", ConsoleColor.Yellow);

                    WriteLine("Running VRAD...", ConsoleColor.Yellow);
                    Write('"' + vradPath + '"', ConsoleColor.Black, ConsoleColor.White);
                    WriteLine(' ' + vradParams, ConsoleColor.Cyan, ConsoleColor.Black, filename);
                    proc = Process.Start(new ProcessStartInfo(vradPath, string.Format(vradParams, filename)) { UseShellExecute = false, RedirectStandardOutput = true });
                    Console.WriteLine(proc.StandardOutput.ReadToEnd());
                    proc.WaitForExit();
                    WriteLine("Finished.", ConsoleColor.Yellow);

                    //string bspName = args[i].Replace(".vmf", ".bsp");
                    //if (!Directory.Exists(outputdirectory))
                    //    Directory.CreateDirectory(outputdirectory);
                    //File.Copy(bspName, outputdirectory + bspName);
                }
                catch (Exception ex)
                {
                    WriteLine("There was an exception", ConsoleColor.Red);
                    Console.WriteLine(ex.ToString(), ConsoleColor.Yellow);
                }
                finally
                {
                    DateTime end = DateTime.Now;
                    WriteLine("Took {0} sconds.",ConsoleColor.Yellow, ConsoleColor.Black, end - start);
                }
            }

            Console.WriteLine("All done, press any key to continue...");
            Console.ReadKey(true);
        }
        private static void WriteLine(string format, ConsoleColor foreground = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black, params object[] args)
        {
            ConsoleColor f = Console.ForegroundColor;
            ConsoleColor b = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.WriteLine(format, args);
            Console.ForegroundColor = f;
            Console.BackgroundColor = b;
        }

        private static void Write(string format, ConsoleColor foreground = ConsoleColor.Gray, ConsoleColor background = ConsoleColor.Black, params object[] args)
        {
            ConsoleColor f = Console.ForegroundColor;
            ConsoleColor b = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(format, args);
            Console.ForegroundColor = f;
            Console.BackgroundColor = b;
        }
    }
}
