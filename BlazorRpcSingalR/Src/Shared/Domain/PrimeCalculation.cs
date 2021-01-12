using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorRpcSingalR.Shared.Domain
{
    public class PrimeCalculation
    {
        public static IEnumerable<int> EratosthenesSieve(int beginNo, int count, IReadOnlyCollection<int> primes)
        {
            if (beginNo < 2) throw new ArgumentOutOfRangeException(nameof(beginNo));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            primes ??= new int[] { };
            var range = primes.Concat(Enumerable.Range(beginNo, count)).ToList();
            for (var denominatorIdx = 0; denominatorIdx < range.Count; denominatorIdx++)
            {
                var denominator = range[denominatorIdx];
                for (var nominatorIdx = Math.Max(primes.Count, denominatorIdx); nominatorIdx < range.Count; nominatorIdx++)
                {
                    var nominator = range[nominatorIdx];
                    if (denominator != nominator && nominator % denominator == 0)
                    {
                        range.RemoveAt(nominatorIdx);
                    }
                }
                if (denominatorIdx >= primes.Count)
                    yield return denominator;
            }
        }
    }
}
