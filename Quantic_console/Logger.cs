using Quantic_console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_gui
{
    /**
     * Class responsible for logging moves played by user and computer moves with considered moves with their evaluation
     * also logs which player has won. Logging is realized into file 
     */
    internal class Logger
    {
        readonly string filename;
        public Logger()
        {
            DateTime currentTime = DateTime.Now;
            filename = "logs/" + currentTime.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
        }

        /**
         * Logs played or considered move
         */
        public void Log(Move move)
        {
                Log(move.ToString());
        }

        /**
         * Logs custom message
         */
        public void Log(string message)
        {
                if (!File.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }
                using (StreamWriter streamWriter = new StreamWriter(filename, true))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
        }

    }
}
