using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class EngineProcess
    { 
        public delegate void ProcessMessageSendDelegate(string outputData);
        public event ProcessMessageSendDelegate EngineOutputReceived;
        public event ProcessMessageSendDelegate EngineInputSended;
        public bool ProcessRunning
        {
            get
            {
                if (chessEngineProcess != null)
                {
                    return !chessEngineProcess.HasExited;
                }
                else return false;
            }
        }

        private string fileName;
        private StreamWriter stdiWriter;
        private Process chessEngineProcess;
        private List<string> engineOutputLines;
        public string lastOutput;

        private object _threadSafe = new object();

        public EngineProcess(string fileName)
        {
            this.fileName = fileName;
            engineOutputLines = new List<string>();
        }

        public void RunEngineProcess()
        {
            chessEngineProcess = new Process();
            chessEngineProcess.StartInfo = CreateProcessStartInfo();
            chessEngineProcess.OutputDataReceived += 
                (sender, args) => 
                {
                    lock (_threadSafe)
                    {
                        engineOutputLines.Add(args.Data);
                    }

                    lastOutput = args.Data;

                    EngineOutputReceived?.Invoke(args.Data);
                };


            chessEngineProcess.Start();
            stdiWriter = chessEngineProcess.StandardInput;

            chessEngineProcess.BeginOutputReadLine();
        }

        public void WaitForSingleStandardOutputString()
        {
            while (true)
            {
                lock (_threadSafe)
                {
                    if (engineOutputLines.Count > 0)
                        break;
                }
                Thread.Sleep(10);
            }
        }

        public void QuitEngineProcess()
        {
            chessEngineProcess.CloseMainWindow();
        }

        public void SendString(string command)
        {
            stdiWriter.WriteLine(command);
            EngineInputSended?.Invoke(command);
        }

        public bool ReadStandartOutput(ref string readedLine)
        {
            bool result;
            lock (_threadSafe)
            {
                if (engineOutputLines.Count > 0)
                {
                    readedLine = engineOutputLines[0];
                    engineOutputLines.RemoveAt(0);
                    result = true; 
                }
                else result = false;
            }
            return result;
        }

        

        private ProcessStartInfo CreateProcessStartInfo()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            return startInfo;
        }
    }
}
