using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loging
{
   public class Logger
    {
        static Logger instance;
        DocumentSaver saver = DocumentSaver.getInstance();
        public static Logger getInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }
        List<string> LogInf = new List<string>();
        public void Info(string log) {
            LogInf.Add(log + "; ");
        }
        public void SaveInfo()
        {
            saver.saveToFile(LogInf.ToArray());
        }
    }
}
