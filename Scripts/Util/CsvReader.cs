using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Bale007.Util
{
    public static class CsvReader
    {
        public static void ProcessCsvRecords(string path, Action<CsvHelper.CsvReader> handler)
        {          
            using (StreamReader csvReader = new StreamReader(path))
            {
                using (var reader = new CsvHelper.CsvReader(csvReader))
                {
                    handler(reader);
                }
            }
        }

        public static void ProcessCsvRecords<T>(string path, Action<IEnumerable<T>> handler)
        {
            using (StreamReader csvReader = new StreamReader(path))
            {                          
                using (var reader = new CsvHelper.CsvReader(csvReader))
                {
                    handler(reader.GetRecords<T>());             
                }
            }
        }

        public static void ProcessCsvRecords<T>(TextAsset csvFile, Action<IEnumerable<T>> handler)
        {
            using (StringReader csvReader = new StringReader(csvFile.text))
            {
                using (var reader = new CsvHelper.CsvReader(csvReader))
                {
                    handler(reader.GetRecords<T>());
                }
            }
        }

        public static void ProcessCsvRecords(TextAsset csvFile, Action<CsvHelper.CsvReader> handler)
        {
            using (StringReader csvReader = new StringReader(csvFile.text))
            {
                using (var reader = new CsvHelper.CsvReader(csvReader))
                {
                    handler(reader);
                }
            }
        }
    }
 
}
