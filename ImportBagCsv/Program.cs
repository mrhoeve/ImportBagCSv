using System;

namespace ImportBagCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("momentje!");
            ImportCSV importCSV = new ImportCSV();
            importCSV.DoImport();
        }
    }
}
