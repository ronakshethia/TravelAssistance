using Microsoft.Bot.Builder.FormFlow;
using System;


namespace TravelBot.Model
{

    public enum TravelOption
    {
        Air,
        Train,
        Hotel
    }

    [Serializable]
    public class OnlineTravel
    {
        [Prompt("Select {&} what you want to do ? {||}")]
        public TravelOption? options { get; set; }

        public static IForm<OnlineTravel> BuildForm()
        {
            return new FormBuilder<OnlineTravel>()
                .Field(nameof(options))
                .Build();
        }

    }
}