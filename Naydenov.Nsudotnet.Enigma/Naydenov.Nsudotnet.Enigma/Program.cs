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
                    Encode(args[2], args[1], args[3]);
                    return;
                }
                if ((args.Length == 5) && (args[0].Equals("decrypt")))
                {
                    Decode(args[2], args[1], args[4], args[3]);
                    return;
                }
                throw new Exception("Param error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static System.Security.Cryptography.SymmetricAlgorithm GetAlgorithm(string algName)
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

        private static string GetFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        private static void Encode(string alg, string input, string output)
        {
            using (var algorithm = GetAlgorithm(alg))
            {
                algorithm.GenerateKey();
                algorithm.GenerateIV();
                File.WriteAllLines(String.Format("{0}.key.txt", GetFileName(input)),
                    new string[] { Convert.ToBase64String(algorithm.Key), Convert.ToBase64String(algorithm.IV) });
                using (var cryptor = algorithm.CreateEncryptor())
                using (var inStream = new FileStream(input, FileMode.Open))
                {
                    using (var crStream = new System.Security.Cryptography.CryptoStream(
                        inStream,
                        cryptor,
                        System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (var outStream = new FileStream(output, FileMode.Create))
                        {
                            crStream.CopyTo(outStream);
                        }
                    }
                }
            }
        }

        private static void Decode(string alg, string input, string output, string keyFile)
        {
            using (var algorithm = GetAlgorithm(alg))
            {
                var IVKey = File.ReadAllLines(keyFile);
                algorithm.Key = Convert.FromBase64String(IVKey[0]);
                algorithm.IV = Convert.FromBase64String(IVKey[1]);
                using (var cryptor = algorithm.CreateDecryptor())
                using (var inStream = new FileStream(input, FileMode.Open))
                {
                    using (var crStream = new System.Security.Cryptography.CryptoStream(
                        inStream,
                        cryptor,
                        System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        using (var outStream = new FileStream(output, FileMode.Create))
                        {
                            crStream.CopyTo(outStream);
                        }
                    }
                }
            }
        }

    }
}
