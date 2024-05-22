// See https://aka.ms/new-console-template for more information
using Crypto_Lab1_;
using System;
using System.Numerics;
using System.Text;

public class Program
{
    public static void Main()
    {
        for (int i = 2; i <= 30; ++i)
        {
            Console.WriteLine("Generated binary with length " + i + ": " + ToBase2(Ariphmetic.GetRandBinaryByLength(i)));
        }

        for (int i = 2; i <= 30; ++i)
        {
            Console.WriteLine("Generated decimal with length " + i + ": " + Ariphmetic.GeneratePrime(i));
        }

        List<string> carlMichaelList = new List<string>
        {
            "561", "162401", "449065", "552721", "314821", "63973",
            "410041", "340561", "8911", "530881"
        };

        BigInteger[] strongLucasPseudoprimes = { 5459, 5777, 10877, 16109, 18971, 22499, 24569, 25199, 40309, 58519, 75077, 97439, 100127, 113573, 115639, 130139, 155819, 158399, 161027, 162133, 176399, 176471, 189419, 192509, 197801, 224369, 230691, 231703, 243629, 253259, 268349, 288919, 313499, 324899 };
        BigInteger[] strongPseudoprimesToBase2 = { 2047, 3277, 4033, 4681, 8321, 15841, 29341, 42799, 49141, 52633, 65281, 74665, 80581, 85489, 88357, 90751, 104653, 130561, 196093, 220729, 233017, 252601, 253241, 256999, 271951, 280601, 314821, 357761, 390937, 458989, 476971, 486737 };
        BigInteger[] longPrimesNumbers = {
            BigInteger.Parse("874976257858102471576213669489"),
            BigInteger.Parse("7555554555554555554555554555557"),
            BigInteger.Parse("101088811181811181118181118880101"),
            BigInteger.Parse("983531983579983617983777983791983819"),
            BigInteger.Parse("502360950888298027355518043327124471675959709617316205938938264006428765255489"),
            BigInteger.Parse("227432689108589532754984915075774848386671439568260420754414940780761245893"),
            BigInteger.Parse("95939187858177756965635755514945393533272521159"),
            BigInteger.Parse("384956219213331276939737002152967117209600000001")
        };

        for (int i = 0; i < carlMichaelList.Count; i++)
        {
            if (Ariphmetic.BailliePSWPrimalityTest(new BigInteger(Convert.ToInt64(carlMichaelList[i]))))
            {
                Console.WriteLine("Failed for carmichael: " + carlMichaelList[i]);
            }
        }

        for (int i = 0; i < strongLucasPseudoprimes.Length; i++)
        {
            if (Ariphmetic.BailliePSWPrimalityTest(strongLucasPseudoprimes[i]))
            {
                Console.WriteLine("Failed for Lucas: " + strongLucasPseudoprimes[i]);
            }
        }

        for (int i = 0; i < strongPseudoprimesToBase2.Length; i++)
        {
            if (Ariphmetic.MillerRabinPrimalityTest(strongPseudoprimesToBase2[i]))
            {
                Console.WriteLine("Failed for base 2: " + strongPseudoprimesToBase2[i]);
            }
        }

        for (int i = 0; i < longPrimesNumbers.Length; i++){
            if (!Ariphmetic.BailliePSWPrimalityTest(longPrimesNumbers[i]))
            {
                Console.WriteLine("Failed for long prime: " + longPrimesNumbers[i]);
            }
        }

        for (int i = 1; i <= 4e5; ++i)
        {
            if (Ariphmetic.MillerRabinPrimalityTest(i) != Ariphmetic.IsPrime(i))
            {
                Console.WriteLine("Failed MR for test" + i);
            }
        }
        for (int i = 1; i <= 4e5; ++i)
        {
            if (Ariphmetic.BailliePSWPrimalityTest(i) != Ariphmetic.IsPrime(i))
            {
                Console.WriteLine("Failed BPSW for test" + i);
            }
        }
    }
    static string ToBase2(BigInteger number)
    {
        return number.ToString("B");
    }

    static string ToBase64(BigInteger number)
    {
        byte[] bytes = number.ToByteArray();
        return Convert.ToBase64String(bytes);
    }

    static string ToByteArray(BigInteger number)
    {
        byte[] byteArray = number.ToByteArray();
        string res = "";
        res += "ByteArray: [";
        for (int i = 0; i < byteArray.Length; i++)
        {
            res += byteArray[i].ToString("X");
            if (i < byteArray.Length - 1)
            {
                res += ", ";
            }
        }
        res += "]";
        return res;
    }
}