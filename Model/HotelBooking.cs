using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TravelBot.Model
{

    public enum BedSizeOption
    {
        King,
        Queen,
        Single,
        Double
    }

    public enum AmenitiesOption
    {
        Kitchen,
        ExtraTowel,
        Wifi,
        GymAccess
    }

    [Serializable]
    public class HotelBooking
    {
        [Prompt("Please enter location")]
        public string location;
        [Prompt("Please enter checkin date in dd/mm/yyyy")]
        [Pattern(@"^\d{1,2}/\d{1,2}/\d{4}$")]
        public string checkindate { get; set; }
        [Prompt("Please enter checkout date in dd/mm/yyyy")]
        [Pattern(@"^\d{1,2}/\d{1,2}/\d{4}$")]
        public string checkoutdate { get; set; }
        [Prompt("Enter number of adults between 0 to 3")]
        [Numeric(1, 3)]
        public int? Adults { get; set; }
        [Prompt("Enter number of childs between 0 to 1")]
        [Numeric(0, 3)]
        public int? Children { get; set; }
        [Prompt("Enter number of room you want (Max 2 room)")]
        [Numeric(1, 2)]
        public int? NoOfRooms { get; set; }
        public BedSizeOption? Bedsize;
        public List<AmenitiesOption> Amenities { get; set; }

        public static IForm<HotelBooking> BuildForm()
        {
            return new FormBuilder<HotelBooking>()
                .Field(nameof(location))
                .Field(nameof(checkindate))
                .Field(nameof(checkoutdate),
                  validate: async (state, value) =>
                  {
                      var result = new ValidateResult { IsValid = true, Value = value };
                      //Here checkoutdate is present inside value
                      //Parse your date in string to Date object
                      DateTime checkindate = DateTime.ParseExact(state.checkindate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                      DateTime checkoutdate = DateTime.ParseExact(Convert.ToString(value), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                      //If checkoutdate is less than checkin date then its invalid input
                      if (checkoutdate < checkindate)
                      {
                          result.IsValid = false;
                          result.Feedback = "Checkout date can't be less than checkin date";
                      }
                      return result;
                  })
                  .Field(nameof(NoOfRooms))                  
                  .Field(nameof(Adults))
                  .Field(nameof(Children))
                  .Field(nameof(Bedsize))
                  .Field(nameof(Amenities))
                 .OnCompletion(async (context, profileForm) =>
                 {
                     // Tell the user that the form is complete
                     await context.PostAsync("Hurrey we found best hotels for you.");
                 })
                .Build();
        }

    }
}