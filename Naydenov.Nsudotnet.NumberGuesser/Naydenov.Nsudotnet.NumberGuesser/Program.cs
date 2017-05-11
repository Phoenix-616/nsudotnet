using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naydenov.Nsudotnet.NumberGuesser
{
    class Program
    {

        private static string[] sentence = {"You are wrong, ", "Not this time, ", "Try harder, ", "You can do it better, "};

        static void Main(string[] args)
        {
            var rand = new Random();
            int number = rand.Next() % 101;
            Console.WriteLine("Your name, player:");
            string player = Console.ReadLine();
            string temp;
            int tmp;

            DateTime start = DateTime.Now;
            LinkedList<int> answers = new LinkedList<int>();
            Console.WriteLine("Game started. Enter your answer");
            for (int i = 1; ; i = (i + 1) % 4)
            {
                if (i == 0)
                {
                    Console.WriteLine(sentence[rand.Next() % sentence.Length] + player);
                }
                if (int.TryParse(temp = Console.ReadLine(), out tmp))
                {
                    answers.AddLast(tmp);
                    if (tmp == number)
                    {
                        Console.WriteLine(String.Format("Victory in {0} minutes after {1} attempts", (DateTime.Now - start).Minutes, answers.Count));
                        foreach (int ans in answers)
                        {
                            string sign = "";
                            if (ans == number) sign = " =";
                            if (ans < number) sign = " <";
                            if (ans > number) sign = " >";
                            Console.WriteLine(ans + sign);
                        }
                        break;
                    }
                    if (tmp > number)
                    {
                        Console.WriteLine("smaller");
                    }
                    else
                    {
                        Console.WriteLine("bigger");
                    }
                }
                else
                {
                    if (temp.Equals("q"))
                    {
                        Console.WriteLine("I'm sorry you loose");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("I don't understand you, " + player);
                    }
                }
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
