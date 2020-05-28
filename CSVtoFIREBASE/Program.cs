using System;
using System.IO;
using System.Collections.Generic;

namespace CSVtoFIREBASE
{
    class Program
    {
        static int id = 225;
        static void Main(string[] args)
        {
            Dictionary<string, object> data;
            //.CSV path
            var reader = new StreamReader("path");

            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var values = line.Split(',');

                //edit your data here
                data = new Dictionary<string, object>();
                data.Add("ciclo", values[0]);
                data.Add("navio", values[1]);
                data.Add("contrato", values[2]);
                data.Add("cliente", values[3]);
                data.Add("boleto", values[4].Replace("zero", "0"));
                data.Add("dataManobra", values[5]);
                
                string aux = values[6] + values[7];
                aux = aux.Trim('"').Replace(".", "");
                aux = aux.Insert((aux.Length - 2), ",");
                data.Add("valor", Convert.ToDouble(aux));

                data.Add("vencimentoBoleto", values[8]);
                data.Add("realizadoPor", values[9]);
                data.Add("statusBoleto", values[10]);

                foreach (KeyValuePair<string, object> entry in data)
                {
                    Console.WriteLine(entry.Value);
                }
                InsertDocumentInDB(data);
            }

            Console.ReadLine();

            void InsertDocumentInDB(Dictionary<string, object> data) {
                FirebaseConnection conn = new FirebaseConnection();
                //name of collection, custom id and your data
                conn.AddDocumentWithId("collection", id.ToString(), data);
                id++;

                Console.WriteLine("----------------------");
                Console.WriteLine("Document Inserted");
                Console.WriteLine("----------------------");
            }
        }
    }
}
