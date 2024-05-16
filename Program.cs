namespace css_optimizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = GetUserInput();
            var folder = new DirectoryInfo(input[0]);
            var htmlFiles = FilesExtractor.Extract(folder, f => f.Extension.Contains("html"));
            if (htmlFiles.Count > 0)
            {
                var cssFiles = FilesExtractor.Extract(folder, f => f.Extension.Contains("css"), input.Skip(1));
                if (cssFiles.Count > 0)
                {
                    Console.WriteLine("Removing Unnecessary CSS styles...");
                    var classesUsed = ClassesExtractor.Extract(htmlFiles);
                    CssOptimizer.Optimize(cssFiles, classesUsed);
                    Console.WriteLine("Done!");
                }
                else
                {
                    Console.WriteLine("Dolder Doesn't Conatin CSS files!");
                }
            }
            else
            {
                Console.WriteLine("Folder Doesn't Contain HTML files!");
            }
        }
        private static List<string> GetUserInput() 
        {
            List<string> input= new ();
            string projectRootFolder;
            do
            {
                Console.Clear();
                Console.Write("Please Enter a valid project path: ");
                projectRootFolder = Console.ReadLine()??string.Empty;
            } while (!Directory.Exists(projectRootFolder));
            input.Add(projectRootFolder);
            char choice;
            do
            {
                Console.Clear();
                Console.Write("Do you Want To Execlude Certain Folders?[y/n] ");
                choice = char.ToLower(Console.ReadKey(true).KeyChar);
                if (choice == 'y')
                {
                    Console.WriteLine("\nPlease enter a list of folders to be execluded separated by spaces:");
                    input.AddRange(Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }
            } while (choice!='y'&&choice!='n');
            Console.Clear();
            return input;
        }
    }
}
