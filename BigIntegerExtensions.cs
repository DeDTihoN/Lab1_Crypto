using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    public static class BigIntegerExtensions
    {
        public static int BitLength(this BigInteger value)
        {
            byte[] bytes = value.ToByteArray();
            int bitLength = bytes.Length * 8;
            byte lastByte = bytes[bytes.Length - 1];
            if (lastByte == 0)
                bitLength -= 8;

            while (lastByte > 0)
            {
                lastByte >>= 1;
                bitLength--;
            }

            return bitLength;
        }

        public static bool TestBit(this BigInteger value, int index)
        {
            return (value & (BigInteger.One << index)) != 0;
        }

        public static BigInteger Sqrt(this BigInteger value)
        {
            if (value < 0) throw new ArgumentException("Невозможно вычислить квадратный корень из отрицательного числа.", nameof(value));
            if (value == 0) return 0;

            BigInteger n = value;
            BigInteger result = (n / 2) + 1;
            BigInteger lastResult = 0;

            while (result < lastResult)
            {
                lastResult = result;
                result = (result + (n / result)) / 2;
            }

            return lastResult;
        }
    }
}
