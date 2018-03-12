﻿using System.Globalization;
using System.Threading;

namespace Domain
{
    public class Offer
    {
        public string OfferReferenceNumber { get; }
        public float OperationPrice { get; }
        public bool IsEligible { get; set; } = true;

        public int RequiredVehicleType => ListContainer.Instance.RouteNumberList.Find(r => r.RouteId == RouteID).RequiredVehicleType;

        public int RouteID { get; }
        public string UserID { get; }
        public float DifferenceToNextOffer { get; set; }
        public int RouteNumberPriority { get; }
        public int ContractorPriority { get; }
        public Contractor Contractor => ListContainer.Instance.ContractorList.Find(c => c.UserId == UserID);
        public bool Win { get; set; }

        public Offer(string offerReferenceNumber, string routeId, string operationPrice, string userId,
            string routeNumberPriority, string contractorPriority)
        {   
            OfferReferenceNumber = offerReferenceNumber;
            RouteID = ParseToIntElseZero(routeId);
            OperationPrice = ParseToFloatElseZero(operationPrice);
            UserID = userId;
            RouteNumberPriority = ParseToIntElseZero(routeNumberPriority);
            ContractorPriority = ParseToIntElseZero(contractorPriority);
        }

        private static int ParseToIntElseZero(string str)
        {

            int.TryParse(str.Trim(), out int n);
            return n;
        }

        private static float ParseToFloatElseZero(string toParse)
        {
            string currentCultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo cultureInformation = new CultureInfo(currentCultureName);
            cultureInformation.NumberFormat.NumberDecimalSeparator = ",";
            toParse = toParse.Replace(" ", "");
            float.TryParse(toParse.Replace('.', ','), NumberStyles.Float, cultureInformation, out float n);
            return n;
        }
    }
}