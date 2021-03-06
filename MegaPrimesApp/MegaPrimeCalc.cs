﻿using System;
using System.Collections.Generic;

namespace MegaPrimesApp
{
    static class MegaPrimeCalc
    {
        private static bool IsPrime(uint number)
        {
            // Function to determine whether or not a number is a prime
            // Considered prime if no number from 2 up to the sqrt<number> is exactly divisible
            // Any number greater than the sqrt<number> that is exactly divisible will be a factor of a number <= sqrt<number>
                        
            if (number <= 1)
            {
                return false;
            } 
            else if (number % 2 == 0)
            {
                return number == 2;
            }

            uint root = Convert.ToUInt32(Math.Sqrt(Convert.ToDouble(number)));

            for (uint i = 3; i <= root; i+=2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AreAllDigitsPrime(uint number)
        {
            // Function to determine whether or not ALL digits of a number are prime

            while (number != 0)
            {
                uint digit = number % 10;
                number /= 10;

                if (digit != 2 && digit != 3 && digit != 5 && digit != 7)
                {
                    return false;
                }
            }

            return true;
        }

        private static List<uint> SieveEratosthenesBasic(uint max)
        {
            // Function that retrieves all the prime numbers up to <max> using the sieve of Eratosthenes algorithm (https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes)
            // Cannot be used as a solution on it own because of the memory requirements when <max> is very large
            // Is used to implement the segmented version of the sieve of Eratosthenes algorithm

            List<uint> primes = new List<uint>();

            bool[] primesTmp = new bool[max + 1];

            for (uint i = 0; i <= max; i++)
            {
                primesTmp[i] = true;
            }

            for (uint i = 2; i <= max; i++)
            {
                if (primesTmp[i])
                {
                    primes.Add(i);

                    for (uint j = i + i; j <= max; j += i)
                    {
                        primesTmp[j] = false;
                    }
                }
            }

            return primes;
        }

        public static List<uint> Fast(uint max)
        {
            // Function that retrieves all the megaprime numbers up to <max> by quickly identifying the numbers whos digits are all prime
            // If ANY digits of a number are NOT prime then instead of increasing the number by 1, increase the first digit that wasn't prime by 1

            // e.g.

            // If the current number is 422222222 then there is no point checking 422222223 - 499999999 because they will all fail because of the 4
            // Therefore we can safely increase the last digit by one to 522222222

            List<uint> megaPrimes = new List<uint>();
            
            ulong i = 2;

            while (i <= max)
            {
                uint number = (uint)i;
                uint digit = 0;
                uint index = 0;

                // Iterate through digits, least significant first

                while (number != 0)
                {
                    digit = number % 10;

                    if (digit != 2 && digit != 3 && digit != 5 && digit != 7)
                    {
                        break;
                    }

                    number /= 10;
                    index++;
                }

                if (number == 0)
                {
                    // All digits ARE primes so check if the number is prime too

                    if (IsPrime((uint)i))
                    {
                        megaPrimes.Add((uint)i);
                    }

                    // Check next number

                    i++;
                }
                else
                {
                    // All digits are NOT primes, increase the first digit that failed by 1

                    i += Convert.ToUInt32(Math.Pow(10, Convert.ToDouble(index)));
                }
            }
            
            return megaPrimes;
        }

        public static List<uint> SieveEratosthenesSegmented(uint max)
        {
            // Function that retrieves all the megaprime numbers up to <max> using the segmented version of the sieve of Eratosthenes algorithm (https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes)
            // It has to be the segmented rather than the simple version because of the memory requirements when <max> is very large

            List<uint> megaPrimes = new List<uint>();

            // Start by generating all the prime numbers up to the sqrt<max>

            uint maxSegSize = Convert.ToUInt32(Math.Sqrt(Convert.ToDouble(max)));

            List<uint> primes = SieveEratosthenesBasic(maxSegSize);

            foreach (uint i in primes)
            {
                if (AreAllDigitsPrime(i))
                {
                    megaPrimes.Add(i);
                }
            }

            // Then break the rest of the number sequence from sqrt<max> to <max> into segments of size sqrt<max>
            // Calculating the megaprimes in each segment

            ulong segStart = maxSegSize + 1;

            while (segStart <= max)
            {
                ulong segEnd = segStart + maxSegSize - 1;
                if (segEnd > max)
                    segEnd = max;

                ulong segSize = segEnd - segStart + 1;

                bool[] segPrimes = new bool[segSize];

                for (uint i = 0; i < segSize; i++)
                {
                    segPrimes[i] = true;
                }
                
                foreach (uint i in primes)
                {
                    ulong checkStart = (segStart / i) * i;
                    if (segStart % i != 0)
                    {
                        checkStart += i;
                    }

                    for (ulong j = checkStart; j <= segEnd; j += i)
                    {
                        segPrimes[j - segStart] = false;
                    }
                }
                
                for (uint j = 0; j < segSize; j++)
                {
                    uint number = (uint)(j + segStart);

                    if (segPrimes[j] && AreAllDigitsPrime(number))
                    {
                        megaPrimes.Add(number);
                    }
                }

                segStart += maxSegSize;
            }

             return megaPrimes;
        }

        public static List<uint> Simple(uint max)
        {
            // Function to determine all the megaprime numbers up to <max> using the simple method

            List<uint> megaPrimes = new List<uint>();

            if (max < 2)
            {
                return megaPrimes;
            }

            megaPrimes.Add(2);

            for (ulong i = 3; i <= max; i += 2)
            {
                if (AreAllDigitsPrime((uint)i) && IsPrime((uint)i))
                {
                    megaPrimes.Add((uint)i);
                }
            }

            return megaPrimes;
        }
    }
}
