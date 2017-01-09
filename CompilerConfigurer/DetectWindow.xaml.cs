using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Management;
using System.IO;

namespace CompilerConfigurer
{
    public partial class DetectWindow : Window
    {
        public string VBSPLocation { get; set; }
        public string VRADLocation { get; set; }
        public string VVISLocation { get; set; }
        public string VBSPParameters { get; set; }
        public string VRADParameters { get; set; }
        public string VVISParameters { get; set; }

        private bool runDetect = true;

        public DetectWindow()
        {
            InitializeComponent();
            VBSPLocation = string.Empty;
            VRADLocation = string.Empty;
            VVISLocation = string.Empty;
            VBSPParameters = string.Empty;
            VRADParameters = string.Empty;
            VVISParameters = string.Empty;

            new Thread(new ThreadStart(ThreadLoop)).Start();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            VBSPLocation = vbspLocationCheckBox.IsChecked.HasValue && vbspLocationCheckBox.IsChecked.Value ? vbspLocation.Text : string.Empty;
            VRADLocation = vradLocationCheckBox.IsChecked.HasValue && vradLocationCheckBox.IsChecked.Value ? vradLocation.Text : string.Empty;
            VVISLocation = vvisLocationCheckBox.IsChecked.HasValue && vvisLocationCheckBox.IsChecked.Value ? vvisLocation.Text : string.Empty;
            VBSPParameters = vbspParametersCheckBox.IsChecked.HasValue && vbspParametersCheckBox.IsChecked.Value ? vbspParameters.Text : string.Empty;
            VRADParameters = vradParametersCheckBox.IsChecked.HasValue && vradParametersCheckBox.IsChecked.Value ? vradParameters.Text : string.Empty;
            VVISParameters = vvisParametersCheckBox.IsChecked.HasValue && vvisParametersCheckBox.IsChecked.Value ? vvisParameters.Text : string.Empty;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            runDetect = false;
            base.OnClosing(e);
        }

        private void ThreadLoop()
        {
            while (runDetect)
            {
                foreach (var proc in Process.GetProcesses())
                {
                    #region VBSP
                    if (proc.ProcessName == "vbsp")
                    {
                        string location = string.Empty;
                        string parameters = string.Empty;
                        using (ManagementObjectSearcher mos = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", proc.Id)))
                        {
                            foreach (ManagementObject mo in mos.Get())
                            {
                                //There should only be one
                                string commandLine = mo["CommandLine"] as string;
                                if (commandLine != null)
                                    getLocationParameters(commandLine, "vbsp", out location, out parameters);
                            }
                        }
                        Dispatcher.Invoke(() =>
                        {
                            if (!string.IsNullOrEmpty(location) && vbspLocation.Text != location)
                            {
                                vbspLocationCheckBox.IsChecked = true;
                                vbspLocation.Text = location;
                            }
                            if (!string.IsNullOrEmpty(parameters) && vbspParameters.Text != parameters)
                            {
                                vbspParameters.Text = parameters;
                                vbspParametersCheckBox.IsChecked = true;
                            }
                        });
                    }
                    #endregion
                    #region VRAD
                    if (proc.ProcessName == "vrad")
                    {
                        string location = string.Empty;
                        string parameters = string.Empty;
                        using (ManagementObjectSearcher mos = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", proc.Id)))
                        {
                            foreach (ManagementObject mo in mos.Get())
                            {
                                //There should only be one
                                string commandLine = mo["CommandLine"] as string;
                                if (commandLine != null)
                                    getLocationParameters(commandLine, "vrad", out location, out parameters);
                            }
                        }
                        Dispatcher.Invoke(() =>
                        {
                            if (!string.IsNullOrEmpty(location) && vradLocation.Text != location)
                            {
                                vradLocation.Text = location;
                                vradLocationCheckBox.IsChecked = true;
                            }
                            if (!string.IsNullOrEmpty(parameters) && vradParameters.Text != parameters)
                            {
                                vradParameters.Text = parameters;
                                vradParametersCheckBox.IsChecked = true;
                            }
                        });
                    }
                    #endregion
                    #region VVIS
                    if (proc.ProcessName == "vvis")
                    {
                        string location = string.Empty;
                        string parameters = string.Empty;
                        using (ManagementObjectSearcher mos = new ManagementObjectSearcher(string.Format("SELECT CommandLine FROM Win32_Process WHERE ProcessId = {0}", proc.Id)))
                        {
                            foreach (ManagementObject mo in mos.Get())
                            {
                                //There should only be one
                                string commandLine = mo["CommandLine"] as string;
                                if (commandLine != null)
                                    getLocationParameters(commandLine, "vvis", out location, out parameters);
                            }
                        }
                        Dispatcher.Invoke(() =>
                        {
                            if (!string.IsNullOrEmpty(location) && vvisLocation.Text != location)
                            {
                                vvisLocation.Text = location;
                                vvisLocationCheckBox.IsChecked = true;
                            }
                            if (!string.IsNullOrEmpty(parameters) && vvisParameters.Text != parameters)
                            {
                                vvisParameters.Text = parameters;
                                vvisParametersCheckBox.IsChecked = true;
                            }
                        });
                    }
                    #endregion
                }
                Thread.Sleep(1); // Give it a rest already!
            }
        }

        private void getLocationParameters(string commandLine, string expectedProcName, out string location, out string parameters)
        {
            int index = commandLine.IndexOf(expectedProcName + ".exe");
            if (index < 0)
            {
                location = parameters = string.Empty;
                return;
            }
            if (commandLine[0] == '"')
            {
                location = commandLine.Substring(1, commandLine.IndexOfNth("\"", 2) - 1);
                parameters = commandLine.Substring(commandLine.IndexOfNth("\"", 2) + 1).Trim();
            }
            else
            {
                location = commandLine.Substring(0, commandLine.IndexOf(' '));
                parameters = commandLine.Substring(commandLine.IndexOf(' ') + 1).Trim();
            }
        }
    }
}
