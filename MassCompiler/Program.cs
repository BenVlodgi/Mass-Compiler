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
        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] { "dev_room.vmf" };
#endif

            string vbspPath=string.Empty;
            string vbspParams = string.Empty;
            string vradPath = string.Empty;
            string vradParams = string.Empty;
            string vvisPath = string.Empty;
            string vvisParams = string.Empty;
            string outputdirectory = string.Empty;

            //Above, default values are given incase we don't get info from our config file

            if (File.Exists("config"))
            {
                var config = File.ReadAllLines("config");
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
                Console.WriteLine("No config found");
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                DateTime start = DateTime.Now;
                try
                {
                    string filename = args[i].Replace(".vmf", "");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Compiling vmf {0} / {1} ({2})", i + 1, args.Length, filename);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Process proc;

                    Console.WriteLine("Running VBSP...");
                    Console.ForegroundColor = ConsoleColor.White; Console.Write("vbsp.exe "); 
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(string.Format(vbspParams, filename)); 
                    Console.ForegroundColor = ConsoleColor.Gray;
                    proc = Process.Start(new ProcessStartInfo(vbspPath, string.Format(vbspParams, filename)) { UseShellExecute = false, RedirectStandardOutput = true });
                    Console.WriteLine(proc.StandardOutput.ReadToEnd());
                    proc.WaitForExit();
                    Console.WriteLine("Finished.");

                    Console.WriteLine("Running VVIS...");
                    Console.ForegroundColor = ConsoleColor.White; Console.Write("vvis.exe ");
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(string.Format(vvisParams, filename));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    proc = Process.Start(new ProcessStartInfo(vvisPath, string.Format(vvisParams, filename)) { UseShellExecute = false, RedirectStandardOutput = true });
                    Console.WriteLine(proc.StandardOutput.ReadToEnd());
                    proc.WaitForExit();
                    Console.WriteLine("Finished.");

                    Console.WriteLine("Running VRAD...");
                    Console.ForegroundColor = ConsoleColor.White; Console.Write("vrad.exe ");
                    Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(string.Format(vradParams, filename));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    proc = Process.Start(new ProcessStartInfo(vradPath, string.Format(vradParams, filename)) { UseShellExecute = false, RedirectStandardOutput = true });
                    Console.WriteLine(proc.StandardOutput.ReadToEnd());
                    proc.WaitForExit();
                    Console.WriteLine("Finished.");

                    //string bspName = args[i].Replace(".vmf", ".bsp");
                    //if (!Directory.Exists(outputdirectory))
                    //    Directory.CreateDirectory(outputdirectory);
                    //File.Copy(bspName, outputdirectory + bspName);
                }
                catch (Exception ex)
                {
                    ConsoleColor saveit = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There was an exception");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(ex.ToString());
                    Console.ForegroundColor = saveit;
                }
                finally
                {
                    DateTime end = DateTime.Now;
                    Console.WriteLine("Finished. Took {0} sconds.", end - start);
                }
            }

            Console.WriteLine("All done, press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
