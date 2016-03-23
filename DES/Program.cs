using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DES.Extensions;

namespace DES
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "testtesttesttest0123123 as";

            Console.WriteLine(data);

            DesClient client = new DesClient("1234567");

            string encryptedData = client.Encrypt(data);
            Console.WriteLine(encryptedData);

            string decryptedData = client.Decrypt(encryptedData);
            Console.WriteLine(decryptedData);
        }
    }
}