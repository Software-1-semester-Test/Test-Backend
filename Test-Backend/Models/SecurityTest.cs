    using System;
    using System.IO;
    using System.Net;
    using System.Data.SqlClient;               // part of BCL in many SDKs
    using System.Security.Cryptography;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Net.Mail;

    namespace Test_Backend.Models
    {
        public class SecurityTest
        {
            public void BadMethod()
            {
                // S1215: Avoid calling GC.Collect()
                GC.Collect();

                // S2077: SQL injection (string concatenation)
                string? name = Console.ReadLine();
                string query = "SELECT * FROM Users WHERE Name = '" + name + "'";
                Console.WriteLine(query);
            }

            // 1) Simulated SQL injection (string concatenation)
            public void SqlInjectionExample(string userInput)
            {
                // Even though this doesn't use SqlClient, Sonar may still mark it
                // as a "security-sensitive" pattern because of string-based query construction.
                string unsafeQuery = "SELECT * FROM Users WHERE Name = '" + userInput + "'";
                Console.WriteLine("Running query: " + unsafeQuery);
            }

            // 2) Hard-coded credentials in a network context (S2068)
            public void HardcodedCredentialExample()
            {
                var cred = new NetworkCredential("admin", "SuperSecret123"); // should trigger S2068
                var smtp = new SmtpClient("smtp.example.com")
                {
                    Credentials = cred
                };

            }


            // 4) Weak hashing (S4426)
            public void WeakHashing()
            {
                using (var md5 = MD5.Create())
                {
                    byte[] digest = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("somedata"));
                    Console.WriteLine(BitConverter.ToString(digest));
                }
            }

            // 5) Disabling SSL certificate validation (S4830)
            public void DisableSslValidation()
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, errors) => true;
            }



            public void InsecureDeserialization(byte[] bytes)
            {
                var formatter = new BinaryFormatter(); // Sonar flags this
                using (var ms = new MemoryStream(bytes))
                {
                    var obj = (Dummy)formatter.Deserialize(ms);
                }
            }
        }

        // 6) Insecure deserialization (S5773)
        [Serializable]
        public class Dummy
        {
            public string _name;
        }
    }