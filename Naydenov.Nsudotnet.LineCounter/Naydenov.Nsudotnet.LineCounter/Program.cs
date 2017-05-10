using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Naydenov.Nsudotnet.LineCounter
{
    class Program
    {

        private static int dir(string path, string ext, out int files)
        {
            int ans = 0;
            files = 0;
            foreach (string subPath in Directory.GetDirectories(path))
            {
                int t;
                ans += dir(subPath, ext, out t);
                files += t;
            }
            foreach (string fileName in Directory.GetFiles(path, ext))
            {
                bool t;
                ans += file(fileName, out t);
                if (t)
                {
                    files++;
                }
            }
            return ans;
        }

        private static int file(string path, out bool haveLine)
        {
            int ans = 0;
            haveLine = false;
            StreamReader input = new StreamReader(path);
            string str;
            while ((str = input.ReadLine()) != null)
            {
                str.Replace(Environment.NewLine, "");
                str = String.Join("", str.Split(null));
                if (!str.StartsWith("//") && !str.Equals(""))
                {
                    ans++;
                }
            }
            if (ans > 0) haveLine = true;
            return ans;
        }

        static void Main(string[] args)
        {
            string ext;
            if (args.Length == 1)
            {
                ext = args[0];
            }
            else
            {
                Console.WriteLine("Enter file extension");
                ext = Console.ReadLine();
            }
            Console.WriteLine();
            int t;
            int num = dir(Directory.GetCurrentDirectory(), ext, out t);
            Console.WriteLine(String.Format("{0} lines in {1} files", num, t));
            Console.ReadKey();
        }
    }
}
