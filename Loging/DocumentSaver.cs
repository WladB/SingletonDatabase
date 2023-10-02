using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Loging
{
   public class DocumentSaver
    {
        static DocumentSaver instance;
        public static DocumentSaver getInstance()
        {
            if (instance == null)
            {
                instance = new DocumentSaver();
            }
            return instance;
        }
        public void saveToFile(string[] linesArray){
            string linesText = string.Join("\n", linesArray);

            string filePath = "File.txt";

            try
            {
                File.WriteAllText(filePath, linesText);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
