using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.ComponentModel;

namespace TravelBot.Model
{

    public enum ServiceType
    {
        [Description("I")]
        PNRStatus = 1,
    }

    [Serializable]
    public class TrainInquiry
    {
        [Prompt("Select choice of {&} {||}")]
        public ServiceType serviceType;
        //[Pattern(@"^\d$")]
        [Prompt("Please enter your pnr number")]
        public string PnrNumber;

        public static IForm<TrainInquiry> BuildForm()
        {
            return new FormBuilder<TrainInquiry>()
                .Field(nameof(serviceType))
                .Field(nameof(PnrNumber))
                 .OnCompletion(async (context, profileForm) =>
                 {
                         // Tell the user that the form is complete
                         await context.PostAsync("Following is search result");
                 })
                .Build();
        }

    }


}