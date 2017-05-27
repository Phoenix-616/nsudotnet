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
                bool continueLooping = false;
                bool lostCode = false;
                var input = new StreamReader(path);
                string str;
                int i = 0;
                while ((str = input.ReadLine()) != null)
                {
                    i++;
                    int pos = 0;
                    continueLooping = false;
                    str = str.Trim();
                    lostCode = false;
                    while (str != "")
                    {
                        str = str.Trim();
                        Console.WriteLine(str);
                        if (openedCom)
                        {
                            int idx;
                            if ((idx = str.IndexOf("*/")) == -1)
                            {
                                continueLooping = true;
                                break;
                            } else
                            {
                                str = str.Substring(idx + 2);
                                openedCom = false;
                                continue;
                            }
                        } else
                        {
                            int idx1 = str.IndexOf("//"), idx2 = str.IndexOf("/*");
                            if ((idx1 < 0) && (idx2 < 0)) break;
                            if ((idx1 > -1) && ((idx1 < idx2) || (idx2 < 0)))
                            {
                                str = str.Substring(0, idx1);
                                break;
                            }
                            if ((idx2 > -1) && ((idx2 < idx1) || (idx1 < 0)))
                            {
                                openedCom = true;
                                if (idx2 > 0) lostCode = true;
                                str.Substring(idx2 + 2);
                                continue;
                            }
                        }
                    }
                    if (continueLooping) continue;
                    str = str.Trim();
                    if ((str != "") || lostCode)
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
