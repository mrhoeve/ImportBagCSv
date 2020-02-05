using System;

namespace ImportBagCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start importeren...\n\n");
            ImportCSV importCSV = new ImportCSV();
            importCSV.DoImport();
        }
    }
}
