using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static int count = 0;
        static void Main(string[] args)
        {
            try
            {
                string filename = @"C:\FileNames.txt";
                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                }
                DirectoryInfo dir = new DirectoryInfo(@"C:\Code\eDellPrograms\MAIN\TRUNK\source\V3-SS\SS\SS.Tests\OSC\Data");
                foreach (var item in dir.GetDirectories())
                {
                    File.AppendAllText(filename, "In Directory-----------------------------");
                    File.AppendAllText(filename, item.Name + Environment.NewLine);
                    Console.WriteLine($"Getting files in directory {item.Name}");
                    var files = item.GetFiles();
                    foreach (var file in files)
                    {
                        if (file.Extension.Contains("json"))
                        {
                            continue;
                        }
                        List<XElement> elements = new List<XElement>();
                        Console.WriteLine($"Removing nodes from file {file.FullName}");
                        RemoveElements(file.FullName, elements);
                        
                        File.AppendAllText(filename, file.Name + Environment.NewLine);
                    }
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                }
                File.AppendAllText(filename, "----------------Total occurance------------------");
                File.AppendAllText(filename, count.ToString());
                File.AppendAllText(filename, "-------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        public static void ElmentWithouChild(XElement element, List<XElement> elements)
        {
            if (element.HasElements)
            {
                var chileelemnets = element.Elements();
                foreach (var item in chileelemnets)
                {
                    ElmentWithouChild(item, elements);
                }
            }

           else
            {
                elements.Add(element);
            }
        }

        public static void RemoveElements(string file, List<XElement> elements)
        {
            var xmlStr = File.ReadAllText(file);

            XDocument doc = XDocument.Load(file);

            var result = doc.Elements();
            foreach (var item in result)
            {
                ElmentWithouChild(item,elements);

            }

            foreach (var item in elements)
            {
                if (item.Value.ToLower().Contains("configurator") || item.Value.ToLower().Contains("dsa") || item.Value.ToLower().Contains("gii") || item.Value.Contains("partner"))
                {
                    if (item.Value.Contains("http"))
                    {
                        item.Remove();
                        count++;
                        doc.Save(file);
                    }
                }
            }
        }
    }
}
