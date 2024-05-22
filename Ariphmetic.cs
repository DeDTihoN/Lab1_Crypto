using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    internal class Ariphmetic
    {
        const int MillerRabinConst = 30;

        private static BigInteger PowMod(BigInteger a, BigInteger st, BigInteger mod)
        {
            BigInteger res = 1;

            while (st > 0)
            {
                if (st % 2 == 1)
                {
                    res = (res * a) % mod;
                }

                a = (a * a) % mod;
                st = st / 2;
            }
            return res;
        }

        private static BigInteger MultiplyMod(BigInteger a, BigInteger b, BigInteger mod)
        {
            BigInteger res = 0;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    res = (res + a) % mod;
                }

                a = (a + a) % mod;
                b = b / 2;
            }
            return res;
        }

        public static bool IsPrime(int n)
        {
            if (n == 1)
            {
                return false;
            }
            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool MillerRabinPrimalityTest(BigInteger n, int k = MillerRabinConst)
        {
            if (n <= 1 || n == 4)
                return false;
            if (n <= 3)
                return true;


            BigInteger d = n - 1;
            while (d % 2 == 0)
                d /= 2;

            for (int i = 0; i < k; i++)
            {
                if (!MillerRabinTest(n, d))
                    return false;
            }

            return true;
        }

        private static bool MillerRabinTest(BigInteger n, BigInteger d)
        {
            Random rand = new Random();
            BigInteger a = 2 + (GetRandBigInteger(n - 4) % (n - 4));
            BigInteger x = PowMod(a, d, n);
            if (x == 1 || x == n - 1)
                return true;

            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;
                if (x == 1) return false;
                if (x == n - 1) return true;
            }
            return false;
        }

        // Генерация простых чисел заданной битности
        public static BigInteger GeneratePrimeBinary(int bitLength)
        {
            Random rand = new Random();
            BigInteger primeCandidate;

            do
            {
                primeCandidate = GetRandBinaryByLength(bitLength);
            } while (!BailliePSWPrimalityTest(primeCandidate)); 

            return primeCandidate;
        }

        public static BigInteger GeneratePrime(int bitLength)
        {
            BigInteger primeCandidate = GetRandByLength(bitLength);
            while (!BailliePSWPrimalityTest(primeCandidate))
            {
                primeCandidate = GetRandByLength(bitLength);
            }
            return primeCandidate;
        }

        // Lucas sequence
        public static bool LucasPrimalityTest(BigInteger n)
        {
            if (n == 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0)
            {
                return false;
            }
            // Choose parameters D, P, Q such that D = P^2 - 4Q
            BigInteger D = 5;
            while (true)
            {
                BigInteger g = BigInteger.GreatestCommonDivisor(D, n);

                if (1 < g && g < n)
                {
                    return false;
                }

                if (JacobiSymbol(D, n) == -1) { break; }

                if (D > 0)
                    D = -(D + 2);
                else
                    D = -(D - 2);
            }

            BigInteger P = 1;
            BigInteger Q = (P * P - D) / 4;

            return LucasTest(n, D, P, Q);
        }

        private static BigInteger JacobiSymbol(BigInteger a, BigInteger b)
        {
            if (a == 0) return 0;
            if (a == 1) return 1;
            if (a < 0)
                if ((b & 2) == 0)
                    return JacobiSymbol(-a, b);
                else
                    return -JacobiSymbol(-a, b);
            BigInteger a1 = a, e = 0;
            while ((a1 & 1) == 0)
            {
                a1 >>= 1;
                ++e;
            }
            int s;
            if ((e & 1) == 0 || (b & 7) == 1 || (b & 7) == 7)
                s = 1;
            else
                s = -1;
            if ((b & 3) == 3 && (a1 & 3) == 3)
                s = -s;
            if (a1 == 1)
                return s;
            return s * JacobiSymbol(b % a1, a1);
        }


        private static bool LucasTest(BigInteger n, BigInteger D, BigInteger P, BigInteger Q)
        {
            BigInteger u = 1;
            BigInteger v = P;
            BigInteger u2m = 1;
            BigInteger v2m = P;
            BigInteger qm = Q;
            BigInteger qm2 = Q * 2;
            BigInteger qkd = Q;
            BigInteger d_n = n + 1, s = 0;
            while ((d_n & 1) == 0)
            {
                ++s;
                d_n >>= 1;
            }

            // Console.WriteLine("n = {0}, D = {1}, P = {2}, Q = {3}", n, D, P, Q);

            for (BigInteger mask = 2; mask <= d_n; mask <<= 1)
            {
                // Console.WriteLine("u = {0}, v = {1}, qm = {2}", u, v, qm);
                // Console.WriteLine("u2m = {0}, v2m = {1}, qm2 = {2}", u2m, v2m, qm2);
                u2m = (u2m * v2m) % n;
                v2m = (v2m * v2m) % n;
                while (v2m < qm2) v2m += n;
                v2m -= qm2;
                v2m = (v2m + n) % n;
                qm = (qm * qm) % n;
                qm2 = qm * 2;
                if ((d_n & mask) > 0)
                {
                    BigInteger t1 = (u2m * v) % n, t2 = (v2m * u) % n,
                    t3 = (v2m * v) % n, t4 = (((u2m * u) % n) * D) % n;
                    u = t1 + t2;
                    if ((u & 1) > 0) u += n;
                    u = (u >> 1) % n;
                    v = t3 + t4;
                    if ((v & 1) > 0) v += n;
                    v = (v >> 1) % n;
                    qkd = (qkd * qm) % n;
                }
            }

            if (u == 0 || v == 0) return true;
            BigInteger qkd2 = (qkd * 2) % n;
            for (int r = 1; r < s; ++r)
            {
                v = ((v * v) % n - qkd2 + n) % n;
                if (v < 0) v += n;
                if (v >= n) v -= n;
                v %= n;
                if (v == 0) return true;
                if (r < s - 1)
                {
                    qkd = (qkd * qkd) % n;
                    qkd2 = (qkd * 2) % n;
                }
            }
            return false;
        }

        public static bool BailliePSWPrimalityTest(BigInteger n)
        {
            if (n < 2) return false;
            if (n.IsEven) return n == 2;
            // Small prime test
            BigInteger[] smallPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
            foreach (var p in smallPrimes)
            {
                if (n == p) return true;
                if (n % p == 0) return false;
            }

            BigInteger sqrt = n.Sqrt();

            if (sqrt * sqrt == n) return false;

            // Miller-Rabin test
            if (!MillerRabinPrimalityTest(n)) return false;

            // Lucas test
            return LucasPrimalityTest(n);
        }

        static public BigInteger GetRandBigInteger(BigInteger n)
        {
            // Проверка на корректный диапазон
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "The value must be greater than 0.");
            }

            // Инициализация генератора случайных чисел
            Random rand = new Random();
            byte[] bytes = n.ToByteArray();
            BigInteger result;

            do
            {
                rand.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; // Убедиться, что число неотрицательное
                result = new BigInteger(bytes);
            } while (result < 1 || result > n);

            return result;
        }

        static public BigInteger GetRandByLength(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length must be greater than 0.");
            }

            Random rand = new Random();
            BigInteger result = 0;
            for (int i = 0; i < length; i++)
            {
                result *= 10;
                if (i == 0)
                {
                    result += rand.Next(1, 10);
                }
                else
                {
                    result += rand.Next(0, 10);
                }
            }

            return result;
        }

        static public BigInteger GetRandBinaryByLength(int bitLength)
        {
            if (bitLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bitLength), "The length must be greater than 0.");
            }

            Random rand = new Random();
            BigInteger result = 0;
            for (int i = 0; i < bitLength; i++)
            {
                result *= 2;
                if (i == 0)
                {
                    result += 1;
                }
                else
                {
                    result += rand.Next(0, 2);
                }
            }

            return result;
        }
    }
}
