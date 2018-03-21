using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class SelctionTest
    {
        [TestMethod]
        public void TestMed3BudTilSammeRute()
        {
            var data = new List<Contractor> {
                new Contractor("16-1", "Jakob", "Jakobs Virksomhed", "Jakob@supermail.com", "3", "1", "0", "0", "10"),
                new Contractor("16-2", "Jonatan", "Jonatans Virksomhed", "Jonatan@Megamail.com", "5", "5", "5", "5", "5")
            };
            ListContainer.Instance.ContractorList = data;
            var data2 = new List<Offer> {
                new Offer("1-001", "1", "150","Jakob@supermail.com","", ""),
                new Offer("2-001", "1", "155","Jonatan@Megamail.com","", ""),
                new Offer("2-002", "1", "155","Jonatan@Megamail.com","", ""),
                };
            ListContainer.Instance.Offers = data2;

            var data3 = new List<RouteNumber> {
                new RouteNumber("1","2")
            };
            ListContainer.Instance.RouteNumberList = data3;
            var selectionController = new SelectionController();
            selectionController.Start();

            ListContainer listContainer = ListContainer.Instance;
            List<Offer> outputListByUserId = listContainer.OutputList;

            Assert.AreEqual(1, outputListByUserId.Count);
            Offer first = outputListByUserId.First();
            Assert.AreEqual(150, first.OperationPrice);
            Assert.AreEqual(1, first.RouteID);
            Assert.AreEqual(2, first.RequiredVehicleType);
            Assert.AreEqual(5, first.DifferenceToNextOffer);
            Assert.AreEqual("Jakob@supermail.com", first.UserID);
            Assert.AreEqual(1, first.Contractor.NumberOfWonType2Offers);
        }

        [TestMethod]
        public void TestContractorVinderIkkeFlereBudEndHanHarBilerTilOgVælgerBilligsteLøsning()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Contractor vinder ikke flere bud end han har biler til og vælger billigste l¢sning\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Contractor vinder ikke flere bud end han har biler til og vælger billigste l¢sning\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Contractor vinder ikke flere bud end han har biler til og vælger billigste l¢sning\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();
            selectionController.Start();

            ListContainer listContainer = ListContainer.Instance;
            List<Offer> outputListByUserId = listContainer.OutputList;

            Assert.AreEqual(2, outputListByUserId.Count);
            Offer first = outputListByUserId.First();
            Assert.AreEqual(150, first.OperationPrice);
            Assert.AreEqual(1, first.RouteID);
            Assert.AreEqual(2, first.RequiredVehicleType);
            Assert.AreEqual(5, first.DifferenceToNextOffer);
            Assert.AreEqual("Jakob@supermail.com", first.UserID);
            Assert.AreEqual(1, first.Contractor.NumberOfWonType2Offers);

            Offer second = outputListByUserId[1];
            Assert.AreEqual(155, second.OperationPrice);
            Assert.AreEqual(2, second.RouteID);
            Assert.AreEqual(2, second.RequiredVehicleType);
            Assert.AreEqual(int.MaxValue, second.DifferenceToNextOffer);
            Assert.AreEqual("Jonatan@Megamail.com", second.UserID);
            Assert.AreEqual(1, second.Contractor.NumberOfWonType2Offers);
        }

        [TestMethod]
        public void TestVogntype3()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Tester vogntype3\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Tester vogntype3\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Tester vogntype3\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();
            selectionController.Start();

            ListContainer listContainer = ListContainer.Instance;
            var outputListByUserId = listContainer.OutputList;

            Assert.AreEqual(2, outputListByUserId.Count);
            Offer first = outputListByUserId.First();
            //Assert.AreEqual(149, first.OperationPrice);
            Assert.AreEqual(96, first.RouteID);
            Assert.AreEqual(3, first.RequiredVehicleType);
            //Assert.AreEqual(1, first.DifferenceToNextOffer);
            Assert.AreEqual("Jonatan@Megamail.com", first.UserID);
            Assert.AreEqual(2, first.Contractor.NumberOfWonType3Offers);

            Offer second = outputListByUserId[1];
            //Assert.AreEqual(149, second.OperationPrice);
            Assert.AreEqual(104, second.RouteID);
            Assert.AreEqual(3, second.RequiredVehicleType);
            //Assert.AreEqual(1, second.DifferenceToNextOffer);
            Assert.AreEqual("Jonatan@Megamail.com", second.UserID);
            Assert.AreEqual(2, second.Contractor.NumberOfWonType3Offers);
        }

        [TestMethod]
        public void MereEnd2BudMedSammePrisTilEnRute_IkkeVindende()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Mere end 2 bud med samme pris til en rute(Ikke vindende)\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Mere end 2 bud med samme pris til en rute(Ikke vindende)\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Mere end 2 bud med samme pris til en rute(Ikke vindende)\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();
            selectionController.Start();

            ListContainer listContainer = ListContainer.Instance;
            List<Offer> outputListByUserId = listContainer.OutputList;

            Assert.AreEqual(1, outputListByUserId.Count);
            Offer first = outputListByUserId.First();
            Assert.AreEqual(149, first.OperationPrice);
            Assert.AreEqual(1, first.RouteID);
            Assert.AreEqual(2, first.RequiredVehicleType);
            Assert.AreEqual(1, first.DifferenceToNextOffer);
            Assert.AreEqual("Jakob3@supermail.com", first.UserID);
            Assert.AreEqual(1, first.Contractor.NumberOfWonType2Offers);
            Assert.AreEqual("4-001", first.OfferReferenceNumber);

            List<Offer> offers = listContainer.RouteNumberList[0].Offers;
            Assert.AreEqual(1, offers[0].DifferenceToNextOffer);
            Assert.AreEqual(int.MaxValue, offers[1].DifferenceToNextOffer);
            Assert.AreEqual(int.MaxValue, offers[2].DifferenceToNextOffer);
            Assert.AreEqual(int.MaxValue, offers[3].DifferenceToNextOffer);

            Assert.AreEqual(0, listContainer.ConflictList.Count);

            Console.WriteLine("hello");
        }

        [TestMethod]
        public void ManglendeTilbudTilEnRute()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Manglende tilbud til en rute\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Manglende tilbud til en rute\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Manglende tilbud til en rute\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();

            try
            {
                selectionController.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void BrugForPrioriteringVedRutenummer()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();

            try
            {
                selectionController.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void BrugForPrioriteringVedRutenummerLøsning1()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\L¢sning1\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\L¢sning1\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\L¢sning1\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();
            selectionController.Start();

            ListContainer listContainer = ListContainer.Instance;

            List<Offer> outputListByUserId = listContainer.OutputList.OrderBy(x => x.UserID).ToList();

            Assert.AreEqual(1, outputListByUserId.Count);
            Offer first = outputListByUserId.First();
            Assert.AreEqual(150, first.OperationPrice);
            Assert.AreEqual(1, first.RouteID);
            Assert.AreEqual(2, first.RequiredVehicleType);
            Assert.AreEqual(int.MaxValue, first.DifferenceToNextOffer);
            Assert.AreEqual("Jakob@supermail.com", first.UserID);
            Assert.AreEqual(1, first.Contractor.NumberOfWonType2Offers);
            Assert.AreEqual(1, first.RouteNumberPriority);
        }

        [TestMethod]
        public void BrugForPrioriteringVedRutenummerLøsning1_2()
        {
            IoController ioController = new IoController();
            ioController.InitializeImport(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\L¢sning1-2\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\L¢sning1-2\Tilbud_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Brug for prioritering ved Rutenummer\L¢sning1-2\RouteNumbers.csv"
            );
            var selectionController = new SelectionController();
            selectionController.Start();

            ListContainer listContainer = ListContainer.Instance;

            List<Offer> outputListByUserId = listContainer.OutputList.OrderBy(x => x.UserID).ToList();

            Assert.AreEqual(1, outputListByUserId.Count);
            Offer first = outputListByUserId.First();
            Assert.AreEqual(150, first.OperationPrice);
            Assert.AreEqual(1, first.RouteID);
            Assert.AreEqual(2, first.RequiredVehicleType);
            Assert.AreEqual(int.MaxValue, first.DifferenceToNextOffer);
            Assert.AreEqual("Jonatan@Megamail.com", first.UserID);
            Assert.AreEqual(1, first.Contractor.NumberOfWonType2Offers);
            Assert.AreEqual(2, first.RouteNumberPriority);
        }
    }
}