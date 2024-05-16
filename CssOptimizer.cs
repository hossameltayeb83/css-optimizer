using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace css_optimizer
{
    
    internal static class CssOptimizer
    {
        static SortedDictionary<string, string> _classesInHtml;
        public static void Optimize(List<FileInfo> cssFiles,SortedDictionary<string, string> classesInHtml)
        {
            _classesInHtml=classesInHtml;
            foreach (var file in cssFiles)
            {
                string fileName= file.FullName;
                string tempFile = Path.GetTempFileName();
                var page = File.ReadLines(fileName).ToList();
                var blocksToKeep = new List<string>();
                var curBlock = new List<string>();
                bool inParentheses = false;
                bool keep=false;
                foreach (var line in page)
                {
                    curBlock.Add(line);
                    if(line.Contains("@media")||line.Contains("@keyframes")||line.Contains("@supports"))
                    {
                        blocksToKeep.Add(line);
                        curBlock.Clear();
                    }
                    if (line.Contains("/*") && !inParentheses)
                    {
                        blocksToKeep.Add(line);
                        curBlock.Clear();
                    }
                    else if (line.Contains('{'))
                    {
                        inParentheses = true;
                        keep = ValidateSelectors(curBlock);
                    }
                    else if (inParentheses)
                    {
                        if (line.Contains('}'))
                        {
                            inParentheses = false;
                            if (keep)
                            {
                                blocksToKeep.AddRange(curBlock);
                            }
                            curBlock.Clear();
                        }
                    }
                    else if(line.Contains('}'))
                    {
                        blocksToKeep.AddRange(curBlock);
                        curBlock.Clear();
                    }
                }
                File.WriteAllLines(tempFile, blocksToKeep);
                File.Delete(fileName);
                File.Move(tempFile, fileName);
            }
        }

        private static bool ValidateSelectors(List<string> lines)
        {
            string selectors= PrepareSelectors(string.Join(' ',lines));
            string[] sections= selectors.Split(',');
            foreach (var section in sections)
                if(KeepSection(section))
                    return true;            
            return false;
        }

        private static bool KeepSection(string section)
        {
            string[] selectorsInSection = section.Trim().Split(' ', '>', '+', '~', '[', ':', '{').Where(s => s != "").ToArray();
            foreach (var selector in selectorsInSection)
            {
                int count = selector.Count(c => c == '.');
                if (count != 0)
                {
                    if (count == 1)
                    {
                        if (!_classesInHtml.ContainsKey(selector.Substring(selector.IndexOf('.') + 1)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        string[] classes = selector.Split('.');
                        foreach (var cs in classes)
                        {
                            if (cs != "" && !_classesInHtml.ContainsKey(cs))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static string PrepareSelectors(string section)
        {
            while (section.Contains('['))
            {
                section=section.Replace("["+section.Split('[', ']')[1]+"]","");
            }
            while (section.Contains(":not"))
            {
                int begin = section.IndexOf(":not(");
                section = section.Remove(begin, (section.IndexOf(')', begin) - begin) + 1);
            }
            return section;
        }
    }
}
