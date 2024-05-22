using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab1_
{
    public class BigNumber
    {
        private List<int> digits;
        private bool isNegative;

        public static BigNumber GenerateRandomByLength(int bitLength)
        {
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();

            sb.Append(rand.Next(1,10));

            // Генерация случайных битов заданной длины
            for (int i = 1; i < bitLength; i++)
            {
                sb.Append(rand.Next(0, 10));
            }

            return new BigNumber(sb.ToString());
        }

        public static BigNumber GeneretaRandomBinaryByLength(int bitLength){
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();

            sb.Append(1);

            // Генерация случайных битов заданной длины
            for (int i = 1; i < bitLength; i++)
            {
                sb.Append(rand.Next(0, 2));
            }

            BigNumber res = 0;
            for (int i = 0; i < bitLength; i++)
            {
                res = res * 2 + sb[i] - '0';
            }
            return res;
        }

        // Конструктор
        public BigNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value cannot be null or empty");
            }

            if (value[0] == '-')
            {
                isNegative = true;
                value = value.Substring(1);
            }
            else
            {
                isNegative = false;
            }

            digits = new List<int>();
            for (int i = value.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(value[i]))
                {
                    throw new ArgumentException("Invalid character in the number");
                }
                digits.Add(value[i] - '0');
            }
        }

        // Конструктор для int
        public BigNumber(int value)
        {
            if (value < 0)
            {
                isNegative = true;
                value = -value;
            }
            else
            {
                isNegative = false;
            }

            digits = new List<int>();
            if (value == 0)
            {
                digits.Add(0);
            }
            else
            {
                while (value > 0)
                {
                    digits.Add(value % 10);
                    value /= 10;
                }
            }
        }

         // Оператор неявного преобразования из int в BigNumber
        public static implicit operator BigNumber(int value)
        {
            return new BigNumber(value);
        }

        private BigNumber(List<int> digits, bool isNegative)
        {
            this.digits = digits;
            this.isNegative = isNegative;
        }

        // Перевизначення додавання
        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            if (a.isNegative == b.isNegative)
            {
                return new BigNumber(Add(a.digits, b.digits), a.isNegative);
            }
            else if (a.isNegative)
            {
                return b - new BigNumber(a.digits, false);
            }
            else
            {
                return a - new BigNumber(b.digits, false);
            }
        }

        // Перевизначення додавання з int
        public static BigNumber operator +(BigNumber a, int b)
        {
            return a + new BigNumber(b);
        }

        public static BigNumber operator +(int a, BigNumber b)
        {
            return new BigNumber(a) + b;
        }

        // Перевизначення віднімання
        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            if (a.isNegative == b.isNegative)
            {
                int comparison = Compare(a.digits, b.digits);
                if (comparison == 0)
                {
                    return new BigNumber("0");
                }
                else if (comparison > 0)
                {
                    return new BigNumber(Subtract(a.digits, b.digits), a.isNegative);
                }
                else
                {
                    return new BigNumber(Subtract(b.digits, a.digits), !a.isNegative);
                }
            }
            else if (a.isNegative)
            {
                return -(-a + b);
            }
            else
            {
                return a + -b;
            }
        }

        // Перевизначення віднімання з int
        public static BigNumber operator -(BigNumber a, int b)
        {
            return a - new BigNumber(b);
        }

        public static BigNumber operator -(int a, BigNumber b)
        {
            return new BigNumber(a) - b;
        }

        // Перевизначення унарного мінусу
        public static BigNumber operator -(BigNumber a)
        {
            return new BigNumber(a.digits, !a.isNegative);
        }

        // Перевизначення множення
        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            return new BigNumber(Multiply(a.digits, b.digits), a.isNegative != b.isNegative);
        }

        // Перевизначення множення з int
        public static BigNumber operator *(BigNumber a, int b)
        {
            return a * new BigNumber(b);
        }

        public static BigNumber operator *(int a, BigNumber b)
        {
            return new BigNumber(a) * b;
        }

        // Перевизначення ділення
        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            var result = Divide(a.digits, b.digits);
            return new BigNumber(result.Item1, a.isNegative != b.isNegative);
        }

        // Перевизначення ділення з int
        public static BigNumber operator /(BigNumber a, int b)
        {
            return a / new BigNumber(b);
        }

        public static BigNumber operator /(int a, BigNumber b)
        {
            return new BigNumber(a) / b;
        }

        // Перевизначення залишку від ділення
        public static BigNumber operator %(BigNumber a, BigNumber b)
        {
            var result = Divide(a.digits, b.digits);
            return new BigNumber(result.Item2, a.isNegative);
        }

        // Перевизначення залишку від ділення з int
        public static BigNumber operator %(BigNumber a, int b)
        {
            return a % new BigNumber(b);
        }

        public static BigNumber operator %(int a, BigNumber b)
        {
            return new BigNumber(a) % b;
        }

        // Перевизначення оператора порівняння на рівність
        public static bool operator ==(BigNumber a, BigNumber b)
        {
            return a.isNegative == b.isNegative && Compare(a.digits, b.digits) == 0;
        }

        // Перевизначення оператора порівняння на рівність з int
        public static bool operator ==(BigNumber a, int b)
        {
            return a == new BigNumber(b);
        }

        public static bool operator ==(int a, BigNumber b)
        {
            return new BigNumber(a) == b;
        }

        // Перевизначення оператора порівняння на нерівність
        public static bool operator !=(BigNumber a, BigNumber b)
        {
            return !(a == b);
        }

        // Перевизначення оператора порівняння на нерівність з int
        public static bool operator !=(BigNumber a, int b)
        {
            return !(a == b);
        }

        public static bool operator !=(int a, BigNumber b)
        {
            return !(new BigNumber(a) == b);
        }

        // Перевизначення оператора порівняння на більше
        public static bool operator >(BigNumber a, BigNumber b)
        {
            return Compare(a, b) > 0;
        }

        // Перевизначення оператора порівняння на більше з int
        public static bool operator >(BigNumber a, int b)
        {
            return a > new BigNumber(b);
        }

        public static bool operator >(int a, BigNumber b)
        {
            return new BigNumber(a) > b;
        }

        // Перевизначення оператора порівняння на менше
        public static bool operator <(BigNumber a, BigNumber b)
        {
            return Compare(a, b) < 0;
        }

        // Перевизначення оператора порівняння на менше з int
        public static bool operator <(BigNumber a, int b)
        {
            return a < new BigNumber(b);
        }

        public static bool operator <(int a, BigNumber b)
        {
            return new BigNumber(a) < b;
        }

        // Перевизначення оператора порівняння на більше або дорівнює
        public static bool operator >=(BigNumber a, BigNumber b)
        {
            return Compare(a, b) >= 0;
        }

        // Перевизначення оператора порівняння на більше або дорівнює з int
        public static bool operator >=(BigNumber a, int b)
        {
            return a >= new BigNumber(b);
        }

        public static bool operator >=(int a, BigNumber b)
        {
            return new BigNumber(a) >= b;
        }

        // Перевизначення оператора порівняння на менше або дорівнює
        public static bool operator <=(BigNumber a, BigNumber b)
        {
            return Compare(a, b) <= 0;
        }

        // Перевизначення оператора порівняння на менше або дорівнює з int
        public static bool operator <=(BigNumber a, int b)
        {
            return a <= new BigNumber(b);
        }

        public static bool operator <=(int a, BigNumber b)
        {
            return new BigNumber(a) <= b;
        }

        // Перевизначення методу ToString
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (isNegative)
            {
                sb.Append('-');
            }

            for (int i = digits.Count - 1; i >= 0; i--)
            {
                sb.Append(digits[i]);
            }

            return sb.ToString();
        }

        // Перевизначення Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this == (BigNumber)obj;
        }

        // Перевизначення GetHashCode
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        // Додаткові методи для операцій
        private static List<int> Add(List<int> a, List<int> b)
        {
            List<int> result = new List<int>();
            int carry = 0;
            int length = Math.Max(a.Count, b.Count);

            for (int i = 0; i < length; i++)
            {
                int sum = carry;
                if (i < a.Count)
                {
                    sum += a[i];
                }
                if (i < b.Count)
                {
                    sum += b[i];
                }

                result.Add(sum % 10);
                carry = sum / 10;
            }

            if (carry > 0)
            {
                result.Add(carry);
            }

            return result;
        }

        private static List<int> Subtract(List<int> a, List<int> b)
        {
            List<int> result = new List<int>();
            int borrow = 0;

            for (int i = 0; i < a.Count; i++)
            {
                int diff = a[i] - (i < b.Count ? b[i] : 0) - borrow;
                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result.Add(diff);
            }

            while (result.Count > 1 && result[result.Count - 1] == 0)
            {
                result.RemoveAt(result.Count - 1);
            }

            return result;
        }

        private static List<int> Multiply(List<int> a, List<int> b)
        {
            List<int> result = new List<int>(new int[a.Count + b.Count]);

            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < b.Count; j++)
                {
                    result[i + j] += a[i] * b[j];
                    result[i + j + 1] += result[i + j] / 10;
                    result[i + j] %= 10;
                }
            }

            while (result.Count > 1 && result[result.Count - 1] == 0)
            {
                result.RemoveAt(result.Count - 1);
            }

            return result;
        }

        private static Tuple<List<int>, List<int>> Divide(List<int> a, List<int> b)
        {
            // Проверяем наличие деления на ноль
            if (b.Count == 1 && b[0] == 0)
            {
                throw new DivideByZeroException("Division by zero.");
            }

            // Убираем ведущие нули в делимом
            while (a.Count > 1 && a[a.Count - 1] == 0)
            {
                a.RemoveAt(a.Count - 1);
            }

            // Убираем ведущие нули в делителе
            while (b.Count > 1 && b[b.Count - 1] == 0)
            {
                b.RemoveAt(b.Count - 1);
            }

            // Проверяем, если делитель больше делимого, то результат деления - 0
            if (Compare(a, b) < 0)
            {
                return new Tuple<List<int>, List<int>>(new List<int> { 0 }, new List<int>(a));
            }

            // Делимое
            List<int> dividend = new List<int>(a);
            // Делитель
            List<int> divisor = new List<int>(b);

            // Частное
            List<int> quotient = new List<int>();

            // Инициализируем начальное значение остатка
            List<int> remainder = new List<int>(dividend.GetRange(0, divisor.Count));

            // Выполняем деление в столбик
            for (int i = divisor.Count - 1; i < dividend.Count; i++)
            {
                for (var j = 0; j < remainder.Count(); ++j)
                {
                    Console.Write(remainder[j]);
                }
                Console.WriteLine();

                for (var j = 0; j < divisor.Count(); ++j)
                {
                    Console.Write(divisor[j]);
                }
                Console.WriteLine();

                if (Compare(remainder, divisor) < 0)
                {
                    if (i < dividend.Count - 1)
                    {
                        remainder.Add(dividend[i + 1]);
                    }
                    continue;
                }

                int digit = 0;
                while (Compare(remainder, divisor) >= 0)
                {
                    remainder = Subtract(remainder, divisor);
                    digit++;
                }
                quotient.Add(digit);

                if (i < dividend.Count - 1)
                {
                    remainder.Add(dividend[i + 1]);
                }
            }

            // Убираем ведущие нули в частном
            while (quotient.Count > 1 && quotient[quotient.Count-1] == 0)
            {
                quotient.RemoveAt(0);
            }

            // Убираем ведущие нули в остатке
            while (remainder.Count > 1 && remainder[remainder.Count-1] == 0)
            {
                remainder.RemoveAt(0);
            }

            return new Tuple<List<int>, List<int>>(quotient, remainder);
        }

        private static int Compare(List<int> a, List<int> b)
        {
            while(a.Count()>0 && a[a.Count() - 1] == 0)
            {
                a.RemoveAt(a.Count()-1);
            }
            while(b.Count()>0 && b[b.Count()-1] == 0)
            {
                b.RemoveAt(b.Count()-1);
            }
            if (a.Count != b.Count)
            {
                return a.Count.CompareTo(b.Count);
            }

            for (int i = a.Count - 1; i >= 0; i--)
            {
                if (a[i] != b[i])
                {
                    return a[i].CompareTo(b[i]);
                }
            }

            return 0;
        }

        private static int Compare(BigNumber a, BigNumber b)
        {
            if (a.isNegative != b.isNegative)
            {
                return a.isNegative ? -1 : 1;
            }

            int comparison = Compare(a.digits, b.digits);
            return a.isNegative ? -comparison : comparison;
        }
    }
}
