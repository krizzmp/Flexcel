using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Domain;

namespace DataAccess
{
    public class CSVExportToPublishList
    {
        public static void CreateFile(string filePath)
        {
            try
            {
                // Delete the file if it exists.
                if (File.Exists(filePath))
                {
                    // Note that no lock is put on the
                    // file and the possibility exists
                    // that another process could do
                    // something with it between
                    // the calls to Exists and Delete.
                    File.Delete(filePath);
                }

                // Create the file.
                using (StreamWriter streamWriter = new StreamWriter(filePath, true, Encoding.GetEncoding("iso-8859-1")))
                {
                    streamWriter.WriteLine("Garantivognsnummer" + ";" + "Virksomhedsnavn" + ";" + "Pris" + ";");
                    foreach (Offer offer in ListContainer.Instance.OutputList)
                    {
                        streamWriter.WriteLine(offer.RouteID + ";" + offer.Contractor.CompanyName + ";" +
                                               offer.OperationPrice + ";");
                    }

                    streamWriter.Close();
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Filen blev ikke gemt");
            }
        }
    }
}