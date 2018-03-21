using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Domain;
using System.IO;

namespace DataAccess
{
    public class CSVExportToCallList
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
                    streamWriter.WriteLine(
                        "Nummer;" +
                        "Virksomhedsnavn;" +
                        "Navn;" +
                        "Vedståede v. 2;" +
                        "Vedståede v. 3;" +
                        "Vedståede v. 5;" +
                        "Vedståede v. 6;" +
                        "Vedståede v. 7;" +
                        "Vundne v. 2;" +
                        "Vundne v. 3;" +
                        "Vundne v. 5;" +
                        "Vundne v. 6;" +
                        "Vundne v. 7"
                        );
                    List<Offer> offersToPrint = new List<Offer>();

                    foreach (Offer offer in ListContainer.Instance.OutputList)
                    {
                        if (offersToPrint.All(obj => obj.UserID != offer.UserID))
                        {
                            offersToPrint.Add(offer);
                            streamWriter.WriteLine(
                                $"{offer.OfferReferenceNumber};" +
                                $"{offer.Contractor.CompanyName};" +
                                $"{offer.Contractor.ManagerName};" +
                                $"{offer.Contractor.VehicleTypes[2]};" +
                                $"{offer.Contractor.VehicleTypes[3]};" +
                                $"{offer.Contractor.VehicleTypes[5]};" +
                                $"{offer.Contractor.VehicleTypes[6]};" +
                                $"{offer.Contractor.VehicleTypes[7]};" +
                                $"{offer.Contractor.NumberOfWonOffersOfType(2)};" +
                                $"{offer.Contractor.NumberOfWonOffersOfType(3)};" +
                                $"{offer.Contractor.NumberOfWonOffersOfType(5)};" +
                                $"{offer.Contractor.NumberOfWonOffersOfType(6)};" +
                                $"{offer.Contractor.NumberOfWonOffersOfType(7)}"
                            );
                        }
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