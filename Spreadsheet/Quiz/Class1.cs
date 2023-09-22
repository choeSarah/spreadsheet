using Internal;
using System.Collections.Generic;

namespace Quiz;

1   static IEnumerable<int> Primes()
{
    2     yield return 2;
    3     int p = 3;
    4     while (true)
    {
        5       if (IsPrime(p))
            6         yield return p;
        7       p += 2;
        8     }
    9   }
10
11  static void Main(string[] args)
{
    12    IEnumerable<int> primes = Primes();
    13    foreach (int p in primes)
        14      Console.WriteLine(p);
    15  }

