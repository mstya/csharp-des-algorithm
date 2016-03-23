using System;
using System.Collections;
using System.Text;
using DES.Extensions;

namespace DES
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "testtest";
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            BitArray dataBits = new BitArray(dataBytes);

            Console.WriteLine("Data: {0}", data);
            Console.Write("Data bits: ");
            dataBits.PrintBitArray();

            DesAlgorithm algorithm = new DesAlgorithm("1234567");
            BitArray encoded = algorithm.RunDes(dataBits);

            Console.Write("Encoded data bits: ");
            encoded.PrintBitArray();
            
            BitArray decoded = algorithm.RunUnDes(encoded);

            Console.Write("Decoded data bits: ");
            decoded.PrintBitArray();

            byte[] bytes = decoded.ToByteArray();

            Console.Write("Decoded data: ");
            Console.Write(Encoding.Default.GetString(bytes));

            Console.WriteLine();
        }
    }
}