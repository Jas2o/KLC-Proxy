using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KLCProxy
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        public WindowLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            KHash hash = new KHash(txtUser.Text, txtPass.Password);
            Console.WriteLine(hash.ToString());
        }

        //--

        //From Kaseya's sample VSA client
        private class KHash
        {
            public string RandomNumber { get; protected set; }
            public string NativeSHA256Hash { get; protected set; } //???
            public string RawSHA256Hash { get; protected set; }
            public string CoveredSHA256Hash { get; protected set; }
            public string RawSHA1Hash { get; protected set; }
            public string CoveredSHA1Hash { get; protected set; }

            public KHash(string UserName, string Password)
            {
                RandomNumber = GenerateRandomNumber(8);
                RawSHA256Hash = CalculateHash(Password, "SHA-256");
                CoveredSHA256Hash = CalculateHash(Password, UserName, "SHA-256");
                CoveredSHA256Hash = CalculateHash(CoveredSHA256Hash, RandomNumber,
               "SHA-256");
                RawSHA1Hash = CalculateHash(Password, "SHA-1");
                CoveredSHA1Hash = CalculateHash(Password, UserName, "SHA-1");
                CoveredSHA1Hash = CalculateHash(CoveredSHA1Hash, RandomNumber, "SHA-1"
               );
            }
            private string CalculateHash(string Value1, string Value2, string
           HashingAlgorithm)
            {
                return CalculateHash(Value1 + Value2, HashingAlgorithm);
            }
            private string CalculateHash(string Value, string HashingAlgorithm)
            {
                byte[] arrByte;
                if (HashingAlgorithm == "SHA-1")
                {
                    SHA1 hash = SHA1.Create();
                    arrByte = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Value));
                }
                else if (HashingAlgorithm == "SHA-256")
                {
                    SHA256 hash = SHA256.Create();
                    arrByte = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Value));
                }
                else
                {
                    throw new ApplicationException(string.Format("Unknow hashing algorithm: {0}", HashingAlgorithm));
                }
                string s = "";
                for (int i = 0; i < arrByte.Length; i++)
                {
                    s += arrByte[i].ToString("x2");
                }
                return s;
            }
            private string GenerateRandomNumber(int numberOfDigits)
            {
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                byte[] numbers = new byte[numberOfDigits * 2];
                rng.GetNonZeroBytes(numbers);
                string result = "";
                for (int i = 0; i < numberOfDigits; i++)
                {
                    result += numbers[i].ToString();
                }
                result = result.Replace("0", "");
                return result.Substring(1, numberOfDigits);
            }
        }

    }
}
