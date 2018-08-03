using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using TravelBot.Model;

namespace TravelBot.Dialogs
{
    [LuisModel("de92ad4e-09f5-4ca0-8afd-27f62716fbe3", "820c3feacf284e8db4f4d02103e274e0")]
    [Serializable]
    public class GreetingDialog : LuisDialog<object>
    {
        private const string FlightsOption = "Flights";
        private const string HotelsOption = "Hotels";
        private const string TrainOption = "Train";


   //     [LuisIntent("Greeting")]
        public async Task StartAsync(IDialogContext context, LuisResult result)
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
            await context.PostAsync(message + "\n"+"Welcome to travel bot. How may i help you today ?");
            context.Wait(this.MessageReceived);
        }


        public virtual Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item, LuisResult results)
        {
            return null;
        }


        [LuisIntent("BookFlights")]
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result, LuisResult results)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("help") || message.Text.ToLower().Contains("support") || message.Text.ToLower().Contains("problem"))
            {
                await context.Forward(new SupportDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
            }
            else
            {
                await this.ShowOptionsAsync(context, results);
            }
        }

        private async Task ShowOptionsAsync(IDialogContext context, LuisResult result)
        {
            var act = result.Entities;
            string origin = act[0].Entity;
            string des = act[1].Entity;
     
        
         PromptDialog.Choice(context, this.OnFlightClass, new List<string>() { FlightsOption, HotelsOption, TrainOption }, "Are you looking for a flight or a hotel or train booking?", "Not a valid option", 3);
            context.Call(new FlightDialog(), this.ResumeAfterOptionDialog);


        }

        private async Task OnFlightClass(IDialogContext context, IAwaitable<string> result)
        {
            context.Call(new FlightDialog(), this.ResumeAfterOptionDialog);
        }


            private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case FlightsOption:
                        context.Call(new FlightDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case HotelsOption:
                        context.Call(new HotelsDialog(), this.ResumeAfterOptionDialog);
                        break;
                    case TrainOption:
                        context.Call(new TrainDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attempts :(. But don't worry, I'm handling that exception and you can try again!");

               // context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            var ticketNumber = await result;

            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
           // context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
                //context.Call(new FeedbackDialog("Did you solve problem ?", ""), this.ResumeAfterOptionDialog);
                //  context.Wait(StartAgain);
                // await context.PostAsync($"would you like to continue");
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
               // context.Wait(this.MessageReceivedAsync);
            }
        }

        //private async Task StartAgain(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    var message = await result;
        //    if (message.Text.ToLower().Contains("yes"))
        //    {
        //        context.Wait(this.MessageReceivedAsync);
        //    }            
        //}
    }
}