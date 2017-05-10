using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Naydenov.Nsudotnet.Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if ((args.Length == 4) && (args[0].Equals("encrypt")))
                {
                    encode(args[2], args[1], args[3]);
                    return;
                }
                if ((args.Length == 5) && (args[0].Equals("decrypt")))
                {
                    decode(args[2], args[1], args[4], args[3]);
                    return;
                }
                throw new Exception("Param error");
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static System.Security.Cryptography.SymmetricAlgorithm getAlg(string algName)
        {
            System.Security.Cryptography.SymmetricAlgorithm ans = null;
            switch (algName)
            {
                case "des":
                    ans = new System.Security.Cryptography.DESCryptoServiceProvider();
                    break;
                case "aes":
                    ans = new System.Security.Cryptography.AesCryptoServiceProvider();
                    break;
                case "rc2":
                    ans = new System.Security.Cryptography.RC2CryptoServiceProvider();
                    break;
                case "rijndael":
                    ans = new System.Security.Cryptography.RijndaelManaged();
                    break;
                default:
                    throw new Exception(String.Format("Unknown param {0}", algName));
            }
            return ans;
        }

        private static string getFileName(string path)
        {
            int index = path.LastIndexOf('.');
            if (index < 0)
            {
                index = path.Length;
            }
            return path.Substring(0, index);
        }

        private static void encode(string alg, string input, string output)
        {
            var algorithm = getAlg(alg);
            algorithm.GenerateKey();
            algorithm.GenerateIV();
            File.WriteAllLines(String.Format("{0}.key.txt", getFileName(input)), 
                new string[] {Convert.ToBase64String(algorithm.Key), Convert.ToBase64String(algorithm.IV)});
            Stream a, b, c;
            (a = new System.Security.Cryptography.CryptoStream(
                b = new FileStream(input, FileMode.Open), 
                algorithm.CreateEncryptor(), 
                System.Security.Cryptography.CryptoStreamMode.Read))
                .CopyTo(c = new FileStream(output, FileMode.Create));
            a.Close();
            b.Close();
            c.Close();
        }

        private static void decode(string alg, string input, string output, string keyFile)
        {
            var algorithm = getAlg(alg);
            var IVKey = File.ReadAllLines(keyFile);
            algorithm.Key = Convert.FromBase64String(IVKey[0]);
            algorithm.IV = Convert.FromBase64String(IVKey[1]);
            Stream a, b, c;
            (a = new System.Security.Cryptography.CryptoStream(
                b = new FileStream(input, FileMode.Open),
                algorithm.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Read))
                .CopyTo(c = new FileStream(output, FileMode.Create));
            a.Close();
            b.Close();
            c.Close();
        }
        
    }
}
