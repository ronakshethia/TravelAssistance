using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TravelBot.Model;

namespace TravelBot.Dialogs
{
    [LuisModel("de92ad4e-09f5-4ca0-8afd-27f62716fbe3", "820c3feacf284e8db4f4d02103e274e0")]

    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm Sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {

            string message = "";
            var myDate = new DateTime();
            myDate = DateTime.Now;
            if (myDate.Hour >= 0 && myDate.Hour < 12)
            {
                message = "Good Morning";
            }

            else if (myDate.Hour >= 12 && myDate.Hour < 18)
            {
                message = "Good Afternoon";
            }
            else if (myDate.Hour >= 18)
            {
                message = "Good Evening";
            }

            await context.PostAsync("Hi "+message + "Welcome to travel bot. How may i help you today ?");
            context.Wait(this.MessageReceived);

        }

        [LuisIntent("BookFlights")]
        public async Task FlightBooking(IDialogContext context, LuisResult result)
        {
            //context.Call(new FlightDialog(), Callback);
            var test = result.Entities;
            string Origin = test[0].Entity;
            string Destination = test[1].Entity;
            await context.PostAsync("Your Origin is "+Origin + "& Destination "+ Destination + "\n" + "Is It Correct.. ?");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("BookTrain")]
        public async Task TrainBooking(IDialogContext context, LuisResult result)
        {
            context.Call(new TrainDialog(), Callback);
        }

        [LuisIntent("HotelBooking")]
        public async Task HotelBooking(IDialogContext context, LuisResult result)
        {
            context.Call(new HotelsDialog(), Callback);
        }


        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        public async Task RoomReservation(IDialogContext context, LuisResult result)
        {

        }
    }
}