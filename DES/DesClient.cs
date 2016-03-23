using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DES.Extensions;

namespace DES
{
    internal class DesClient
    {
        private const int PARTITION_SIZE = 64;

        private readonly DesAlgorithm algorithm;

        public DesClient(string key)
        {
            this.algorithm = new DesAlgorithm(key);
        }

        public string Encrypt(string data)
        {
            byte[] dataBytes = Encoding.Default.GetBytes(data.Trim());
            List<bool> bits = new List<bool>(new BitArray(dataBytes).Cast<bool>());

            List<List<bool>> bitsParts = this.BitPartition(bits);

            List<bool> encryptedBits = new List<bool>();

            foreach (List<bool> part in bitsParts)
            {
                List<bool> part64 = this.CheckAndExpandList(part);
                var p = algorithm.RunDes(part64);
                encryptedBits.AddRange(p);
            }

            byte[] res = new BitArray(encryptedBits.ToArray()).ToByteArray();
            string str1 = Encoding.Default.GetString(res);

            return str1;
        }

        private List<List<bool>> BitPartition(IList<bool> bits)
        {
            List<List<bool>> parts = new List<List<bool>>();
            for (int i = 0; i < bits.Count; i += PARTITION_SIZE)
            {
                parts.Add(bits.Skip(i).Take(PARTITION_SIZE).ToList());
            }

            return parts;
        }

        private List<bool> CheckAndExpandList(IList<bool> bits)
        {
            if (bits.Count < PARTITION_SIZE)
            {
                List<bool> expanded = new List<bool>(bits);
                List<bool> additional = new List<bool>(new bool[PARTITION_SIZE - bits.Count]);
                expanded.AddRange(additional);
                return expanded;
            }

            return bits.ToList();
        }

        public string Decrypt(string data)
        {
            byte[] dataBytes = Encoding.Default.GetBytes(data.Trim());
            List<bool> bits = new List<bool>(new BitArray(dataBytes).Cast<bool>());

            List<List<bool>> bitsParts = this.BitPartition(bits);

            List<bool> decryptBits = new List<bool>();

            foreach (List<bool> part in bitsParts)
            {
                decryptBits.AddRange(algorithm.RunUnDes(part));
            }

            return Encoding.Default.GetString(new BitArray(decryptBits.ToArray()).ToByteArray());
        }
    }
}