using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Naydenov.Nsudotnet.LinesCounter
{
    class Program
    {

        private static int CheckDirectory(string path, string ext)
        {
            int ans = 0;
            foreach (string dir in Directory.GetDirectories(path))
            {
                ans += CheckDirectory(dir, ext);
            }
            foreach (string file in Directory.GetFiles(path))
            {
                ans += CheckFile(file, ext);
            }
            return ans;
        }

        private static int CheckFile(string path, string ext)
        {
            int ans = 0;
            if (Path.GetExtension(path) == ext)
            {
                bool openedCom = false;
                var input = new StreamReader(path);
                string str;
                int i = 0;
                while ((str = input.ReadLine()) != null)
                {
                    i++;
                    str = str.Replace(Environment.NewLine, "");
                    str = String.Join("", str.Split(null));
                    int pos = 0;
                    while (true)
                    {
                        if (openedCom)
                        {
                            int t = str.IndexOf("*/", pos);
                            if (t < 0)
                            {
                                str = str.Remove(pos);
                                break;
                            } else
                            {
                                openedCom = false;
                                str = str.Remove(pos, t - pos + 2);
                                continue;
                            }
                        } else
                        {
                            int t1 = str.IndexOf("/*", pos);
                            int t2 = str.IndexOf("//", pos);
                            if ((t2 > -1) && ((t1 > t2) || (t1 == -1)))
                            {
                                str = str.Remove(t2);
                                break;
                            }
                            if ((t1 > -1) && ((t1 < t2) || (t2 == -1)))
                            {
                                pos = t1;
                                openedCom = true;
                                continue;
                            }
                            if ((t1 == -1) && (t2 == -1)) break;
                        }
                    }
                    if (str != "")
                    {
                        ans++;
                    }
                }
            }
            return ans;
        }

        static void Main(string[] args)
        {
            string ext;
            if (args.Length == 1)
            {
                ext = args[1];
            } else
            {
                Console.WriteLine("Enter extension:");
                ext = Console.ReadLine();
            }
            ext = Path.GetExtension(ext);
            Console.WriteLine(String.Format("Lines in project: {0}", CheckDirectory(Directory.GetCurrentDirectory(), ext)));
            Console.ReadKey();
        }
    }
}
