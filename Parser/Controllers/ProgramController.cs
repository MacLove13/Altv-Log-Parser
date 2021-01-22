using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Parser.Localization;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Parser.Controllers
{
    public static class ProgramController
    {
        public const string AssemblyVersion = "0.0.4";
        public static readonly string Version = $"v{AssemblyVersion}";
        public const bool IsBetaVersion = false;
        public const string ParameterPrefix = "--";

        public static string ResourceDirectory;
        public static string LogLocation;

        /// <summary>
        /// Initializes the server IPs matching with the
        /// current server depending on the chosen locale
        /// and determines the newest log file if multiple
        /// server IPs are used to connect to the server
        /// </summary>
        public static void InitializeServerIp()
        {
            try
            {

            }
            catch
            {
                // Silent exception
            }
        }

        /// <summary>
        /// Parses the most recent chat log found at the
        /// selected RAGEMP directory path and returns it.
        /// Displays an error if a chat log does not
        /// exist or if it has an incorrect format
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="removeTimestamps"></param>
        /// <returns></returns>
        public static string ParseChatLog(string directoryPath, bool removeTimestamps)
        {
            try
            {
                // Read the chat log

                var directory = new DirectoryInfo(directoryPath + "\\logs");
                var myFile = (from f in directory.GetFiles()
                              orderby f.LastWriteTime descending
                              select f).First();

                LogLocation = myFile.FullName;

                List<string> lines = new List<string>();
                string log;
                using (StreamReader sr = new StreamReader(LogLocation))
                {
                    log = sr.ReadToEnd();
                }

                foreach (string line in File.ReadLines(LogLocation)) {
                    if (!line.Contains("chat_log:")) continue;

                    string new_line = line;
                    if (removeTimestamps)
                        new_line = Regex.Replace(line, @"\[\d{1,2}:\d{1,2}:\d{1,2}\] ", string.Empty);
                    new_line = new_line.Replace("\\n", "\n");
                    new_line = new_line.Replace("chat_log:", string.Empty);
                    new_line = WebUtility.HtmlDecode(new_line);
                    new_line = new_line.TrimEnd('\r', '\n');

                    lines.Add(new_line);
                }

                string result = String.Join("\n", lines.ToArray());
                return result;
            }
            catch
            {
                MessageBox.Show(Strings.ParseError, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }
    }
}
