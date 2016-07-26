using ChessPro.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessPro.Resources
{
    public class ChessEnginesPE
    {
        readonly string enginesDirectory;

        public enum BinaryType
        {
            SCS_32BIT_BINARY = 0,
            SCS_64BIT_BINARY = 6,
            SCS_DOS_BINARY = 1,
            SCS_OS216_BINARY = 5,
            SCS_PIF_BINARY = 3,
            SCS_POSIX_BINARY = 4,
            SCS_WOW_BINARY = 2,
        }

        [DllImport("kernel32.dll")]
        private static extern unsafe int GetBinaryType(string inApplicationPath, int* outBinaryTypePointer);

        public ChessEnginesPE(string enginesDirectory)
        {
            this.enginesDirectory = enginesDirectory;
        }

        public string[] GetAvailableChessEnginesNames()
        {
            string[] enginesPaths = Directory.GetFiles(enginesDirectory);
            BinaryType validPE = Environment.Is64BitOperatingSystem ? BinaryType.SCS_64BIT_BINARY : BinaryType.SCS_32BIT_BINARY;
            var validEngines = from e in enginesPaths where GetFileType(e) == validPE select e;
            var enginesNames = from e in validEngines select Path.GetFileNameWithoutExtension(e);

            return enginesNames.ToArray();
        }

        public EngineProcess GetEngineProcess(string engineName)
        {
            string filePath = string.Format("{0}\\{1}.exe", enginesDirectory, engineName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            else return new EngineProcess(filePath);
        } 

        public unsafe BinaryType GetFileType(string path)
        {
            int result;
            int* resultPointer = &result;
            //nonzero - ok, zero - path is not valid exe file.
            int functionReturs = GetBinaryType(path, resultPointer);

            if (functionReturs != 0)
            {
                return (BinaryType)result;
            }
            else throw new Exception("File path is incorrect (not exe file ?)");
        }

    }
}
