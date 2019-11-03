using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PHNeutral.ConsoleWriter
{
    /**
     * Console writer class writes messages to a textbox control
     * */
    class ConsoleWriter
    {
        public enum ConsoleLogLevel { None, ErrorsOnly, Standard, Verbose }
        Properties.Settings settings = Properties.Settings.Default;

        /**
         * <summary>Write a line to the console. </summary>
         * <param name="message">String to write to console. </param>
         * <param name="tbControl">The textbox control for the console.</param>
         * <param name="logLevel">The minimum logLevel setting required to display this log message</param>
         */
        public void WriteLine(string message, TextBox tbControl, ConsoleLogLevel logLevel)
        {
            if (CheckLogLevel(logLevel))
            {
                tbControl.AppendText(message);
                tbControl.AppendText(Environment.NewLine);
            }
        }

        /**
 * <summary>Write a line to the console. </summary>
 * <param name="message">String that will be appended to the appendString. </param>
 * <param name="appendString">The string to append to.</param>
 * <param name="logLevel">The minimum logLevel setting required to display this log message</param>
 */
        public void WriteLineAppendString(string message, ref string appendString, ConsoleLogLevel logLevel)
        {
            if (CheckLogLevel(logLevel))
            {
                appendString += message + Environment.NewLine;
            }
        }

        /**
         * <summary>Checks the user's logLevel and returns true if they have the minimum required.</summary>
         * <param name="logLevel">The minimum logLevel.</param>
         */
        public bool CheckLogLevel(ConsoleLogLevel logLevel)
        {
            return (settings.ConsoleLogLevel >= (int)logLevel);
        }
    }
}
