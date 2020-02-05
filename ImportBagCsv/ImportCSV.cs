using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;
using ImportBagCsv.DAL;
using ImportBagCsv.Models;

namespace ImportBagCsv
{
    public static class ImportCSV
    {

        public static void DoImport()
        {
            long counter = 0L;
            long curCounter = 0L;
            //var engine = new FileHelperEngine<CsvModel>();

            //// To Read Use:
            //var result = engine.ReadFile(@"D:\Bag\bagadres-full.csv");


            var engine = new FileHelperAsyncEngine<CsvModel>();

            // Read
            using (engine.BeginReadFile(@"D:\Bag\bagadres-full.csv"))
            {
                using (var context = new Context())
                {
                    // The engine is IEnumerable
                    foreach (CsvModel model in engine)
                    {
                        AdresBuilder.Add(model.Straat, model.Huisnummer, model.Huisletter,
                            model.Huisnummertoevoeging,
                            model.Postcode, model.Woonplaats, model.Gemeente, model.Provincie, context);
                        if (model.Postcode.Equals("7944TV") && model.Huisnummer == 151)
                        {
                            Console.WriteLine(
                                $"Gevonden!\n{model.Straat} {model.Huisnummer}, {model.Postcode} {model.Woonplaats}, {model.Gemeente}, {model.Provincie}");
                        }

                        counter++;
                        curCounter++;
                        if (curCounter == 50000L)
                        {
                            context.SaveChanges();
                            curCounter = 0L;
                        }
                    }
                    context.SaveChanges();
                }

                Console.WriteLine($"Er zijn {counter.ToString()} records gelezen!");
            }


        }
    }
}
