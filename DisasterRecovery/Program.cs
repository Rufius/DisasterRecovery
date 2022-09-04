using System;
using System.Collections.Generic;

namespace DisasterRecovery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // get the number of data centers
                var numberOfDataCenters = GetNumberOfDataCenters();

                // intitalize datacenters
                var dataCenters = InitDataCenters(numberOfDataCenters);

                // sync up all datacenters, so every dc has the same datasets
                SyncUpDataCenters(dataCenters);

                Console.WriteLine("done");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
        }

        private static int  GetNumberOfDataCenters()
        {
            var isParsed = int.TryParse(Console.ReadLine(), out int numberOfDataCenters);

            if (!isParsed || numberOfDataCenters < 0 || numberOfDataCenters > 999999)
                throw new Exception("wrong number of data centers");

            return numberOfDataCenters;
        }

        private static List<HashSet<int>> InitDataCenters(int numberOfDataCenters)
        {
            var dataCenters = new List<HashSet<int>>();

            for (int i = 0; i < numberOfDataCenters; i++)
            {
                var dataCenter = new HashSet<int>();

                var dataSets = GetDataSets(Console.ReadLine());

                foreach (var dataSet in dataSets)
                {
                    dataCenter.Add(dataSet);
                }

                dataCenters.Add(dataCenter);
            }

            return dataCenters;
        }

        private static IEnumerable<int> GetDataSets(string line)
        {
            if (line.Length > 1000)
                throw new Exception("input line must be at most 1000 characters long");

            var stringArray = line.Split(" ");
            foreach (var s in stringArray)
            {
                var isSuccess = int.TryParse(s, out int dataSetId);

                if (!isSuccess || dataSetId < 0 || dataSetId > 999999)
                    throw new Exception("wrong data set id");

                yield return dataSetId;
            }
        }

        private static void SyncUpDataCenters(List<HashSet<int>> dataCenters)
        {
            // first copy all missing data sets to the first data center
            var firstDataCenter = dataCenters[0];
            for (int i = 1; i < dataCenters.Count; i++)
            {
                var dataCenter = dataCenters[i];
                foreach (var dataSet in dataCenter)
                {
                    if (!firstDataCenter.Contains(dataSet))
                    {
                        firstDataCenter.Add(dataSet);
                        Console.WriteLine($"{dataSet} {i + 1} 1"); // in the output an index of a data center must start from 1, hence i + 1
                    }
                }
            }

            // now the first data center contains all the datasets, so sync the rest of the data centers with the first one
            for (int i = 1; i < dataCenters.Count; i++)
            {
                var dataCenter = dataCenters[i];
                foreach (var dataSet in firstDataCenter)
                {
                    if (!dataCenter.Contains(dataSet))
                    {
                        dataCenter.Add(dataSet);
                        Console.WriteLine($"{dataSet} 1 {i + 1}");
                    }
                }
            }
        }

    }
}
