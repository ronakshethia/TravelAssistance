using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using TravelBot.Data;

namespace TravelBot.Model
{
    [Serializable]
    public enum FlightTypes
    {
        [Description("I")]
        International = 1,

        [Description("D")]
        Domestic = 2
    }
    [Serializable]
    public enum ClassTypes
    {
        [Description("F")]
        FirstClass = 'F',
        [Description("B")]
        Business = 'B',
        [Description("E")]
        Economy = 'E'
    }
    [Serializable]
    public enum IsMeal
    {
        Yes = 1,
        No = 2
    }
    [Serializable]
    public enum FoodMenu
    {
        Sandwich = 1,
        Noodles = 2,
        Samosa = 3,
        Cookies = 4,
        Juice = 5,
        Tea = 6,
        Coffee = 7
    }

    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "I do not understand \"{0}\".", "Try again, I don't get \"{0}\".")]
    public class FlightInquiry
    {
        [Prompt("Select choice of {&} {||}")]
        public FlightTypes flightTypes;
        [Prompt("Select {&}  {||}")]
        public ClassTypes classTypes;
        [Prompt("Enter from where you are travelling")]
        public string Source;
        [Prompt("Enter where you want to go")]
        public string Destination;
        [Prompt("Enter date of journey in dd/mm/yyyy format")]
        public DateTime? DateOfJurney;
        [Prompt("Enter number of adults between 1 to 5")]
        [Numeric(1, 5)]
        public int? NumberOfAdult;
        [Prompt("Enter number of child between 0 to 5")]
        [Numeric(0, 5)]
        public int? NumberOfChild;
        [Prompt("Enter number of infants between 0 to 5")]
        [Numeric(0, 5)]
        public int? NumberOfInfants;
        //[Optional]
        //public IsMeal isMeal;
        //public FoodMenu foodMenu;

        public static IForm<FlightInquiry> BuildForm()
        {
            
            return new FormBuilder<FlightInquiry>()       
              //  .Field(nameof(DateOfJurney),validate :  async (date, value) => 
              //  {
                    //var result = new ValidateResult { IsValid = true, Value = value };

                    //// DateTime traveldate = DateTime.ParseExact(date.DateOfJurney.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //DateTime dt = value as DateTime;
                    //DateTime traveldate = DateTime.ParseExact(Convert.ToString(value), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //if (traveldate > DateTime.Now.AddMonths(3))
                    //{
                    //    result.IsValid = false;
                    //    result.Feedback = "you can book ticket for 3 months prior only from the date " + DateTime.Today.ToString();
                    //}
                    //return result;

              //  })
                 .OnCompletion(async (context, profileForm) =>
                 {
              //       var cities = JsonConvert.DeserializeObject<TravelBot.Data.CitiesModel>();
                     // Tell the user that the form is complete
                     await context.PostAsync("Following is search result");
                 })
                .Build();
        }
    }

}