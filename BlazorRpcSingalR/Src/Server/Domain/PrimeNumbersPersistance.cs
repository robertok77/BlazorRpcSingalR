using System;
using System.Linq;

namespace BlazorRpcSingalR.Server.Domain
{
    public class PrimeNumbersPersistance
    {
        private readonly object _locker = new object();
        private int _count = 10_000;
        public int End { get; private set; } = 2;

        public int Count
        {
            get => _count - (End == 2 ? 2 : 0);
            private set => _count = value;
        }

        public int[] PrimeNumbersArr { get; private set; } = { };

        public PrimeNumbersPersistance(int end, int count, int[] primeNumbers)
        {
            End = end;
            Count = count;
            PrimeNumbersArr = primeNumbers;
        }

        public void Increase(int end, int? count = null, int[] primeNo = null)
        {
            if (end <= 0) throw new ArgumentOutOfRangeException(nameof(end));
            if (count != null && count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

            lock (_locker)
            {
                End += end;
                Count = count ?? Count;
                PrimeNumbersArr = primeNo != null? PrimeNumbersArr.Concat(primeNo).ToArray() : PrimeNumbersArr;
            }
        }

        public void Get(out int end, out int count, out int[] primeNo)
        {
            lock (_locker)
            {
                end = End;
                count = Count;
                primeNo = PrimeNumbersArr;
            }
        }
    }
}
