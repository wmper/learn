using System;
using System.Numerics;

namespace Example.RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            // 找出质数 P 、Q
            BigInteger p = 997207;
            BigInteger q = 997219;

            // 计算公共模数 N = P * Q
            BigInteger n = p * q; // n = 994433767333

            // 欧拉函数 φ(N) = (P - 1)(Q - 1)
            BigInteger r = (p - 1) * (q - 1); // r = 994431772908

            // 计算公钥E	1 < E < φ(N)	E的取值必须是整数 E 和 φ(N) 必须是互质数
            BigInteger e = 9293754887;

            // E * D % φ(N) = 1
            int d = (int)((1 + r) / e);

            // 即公开的公钥为：n = 994433767333，e = 9293754887
            // 保密的私钥为：n = 994433767333，d = 107

            BigInteger m = 3;

            // 加密	C ＝ M E mod N
            BigInteger c = BigInteger.ModPow(m, e, n); // c = 601

            Console.WriteLine("密文：" + c);

            // 解密	M ＝C D mod N
            var m2 = BigInteger.Pow(c, d) % n;

            Console.Write("解密：" + m2);

            Console.ReadKey();
        }
    }
}
