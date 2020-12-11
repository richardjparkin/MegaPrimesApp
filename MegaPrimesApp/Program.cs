using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MegaPrimesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Simple program to output all the megaprime numbers up to a given number (inclusive)

            // Command line parameters
            // <1> - Number to generate megaprimes up to (REQUIRED)
            // <2> - Algorithm to use (OPTIONAL)
            //          FAST - Fastest algorithm developeed (DEFAULT)
            //          TD - Uses the trial by division algorithm
            //          SES - Uses the segmented version of the sieve of Eratosthenes algorithm


            if (args.Length == 0)
            {
                Console.Write("ERROR: Please supply a positive integer up to {0} (inclusive) as the first command line parameter\n", UInt32.MaxValue);
                return;
            }

            uint max;

            try
            {
                 max = Convert.ToUInt32(args[0]);
            }
            catch (OverflowException)
            {
                Console.Write("ERROR: Integer must be positive and not exceed {0}\n", UInt32.MaxValue);
                return;
            }
            catch (FormatException)
            {
                Console.Write("ERROR: Parameter '{0}' is not a valid integer\n", args[0]);
                return;
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<uint> megaPrimes;

            if (args.Length == 2)
            {
                if (args[1] == "TD")
                {
                    megaPrimes = MegaPrimeCalc.TrialDivision(max);
                }
                else if (args[1] == "SES")
                {
                    megaPrimes = MegaPrimeCalc.SieveEratosthenesSegmented(max);
                }
                else if (args[1] == "FAST")
                {
                    megaPrimes = MegaPrimeCalc.Fast(max);
                }
                else
                {
                    Console.Write("ERROR: Parameter '{0}' not recognised algorithm\n", args[1]);
                    return;
                }
            }
            else
            {
                megaPrimes = MegaPrimeCalc.Fast(max);
            }

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            Console.Write("{");
            
            foreach (uint i in megaPrimes)
            {
                Console.Write(" {0}", i);
            }
            
            Console.Write(" }\n\n");

            Console.Write("There are {0} megaprime numbers up to and including the number {1}\n\n", megaPrimes.Count, max);
            Console.Write("Time take is {0} seconds\n", ts.TotalSeconds);
        }
    }
}
