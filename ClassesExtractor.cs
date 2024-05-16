using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace css_optimizer
{
    internal static class ClassesExtractor
    {
        public static SortedDictionary<string, string> Extract(List<FileInfo> files)
        {
            SortedDictionary<string,string> result = new SortedDictionary<string,string>();
            foreach (FileInfo file in files)
            {
                var lines = File.ReadLines(file.FullName);
                foreach (var line in lines) 
                        if (line.Contains("class=\""))
                        {
                            int start=line.IndexOf("class=\"")+7;
                            int end=line.IndexOf('"',start);
                            string clas = line.Substring(start, end-start).Trim();
                            string[] classes=clas.Split(' ');
                            foreach (var item in classes)                       
                                if(!result.ContainsKey(item)&&item!="")
                                    result.Add(item, string.Empty);                    
                        }                
            }
            return result;
        }
    }
}
