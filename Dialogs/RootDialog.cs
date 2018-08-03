﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using TravelBot.Model;

namespace TravelBot.Dialogs
{
    [Serializable]
    public class RootDialog
    {

        public static readonly IDialog<string> dialog = Chain.PostToChain()
           .Select(msg => msg.Text)
           .Switch(
           //new RegexCase<IDialog<string>>(new Regex("^hi", RegexOptions.IgnoreCase), (context, text) =>
           //{
           //    return Chain.ContinueWith(new GreetingDialog(), AfterGreetingContinuation);
           //})
           // ,
           new DefaultCase<string, IDialog<string>>((context, text) =>
           {
               return Chain.ContinueWith(FormDialog.FromForm(OnlineTravel.BuildForm, FormOptions.PromptInStart), AfterGreetingContinuation);
           })
            )
           .Unwrap()
           .PostToUser();

        private async static Task<IDialog<string>> AfterGreetingContinuation(IBotContext context, IAwaitable<object> item)
        {
            var data = await item as OnlineTravel;

          return  Chain.PostToChain()
             .Select(msg => msg.Text)
             .Switch(new RegexCase<IDialog<string>>(new Regex("^Hotel", RegexOptions.IgnoreCase), (cnt, text) =>
             {
                 return Chain.ContinueWith(FormDialog.FromForm(HotelBooking.BuildForm,FormOptions.PromptInStart), AfterBookingContinuation);
             })
             )
             .Unwrap()
            .PostToUser();

            //return Chain.Return($"You have selected: {data.options}");  
        }

        private async static Task<IDialog<string>> AfterBookingContinuation(IBotContext context, IAwaitable<object> item)
        {
            var token = await item;
            var name = "User";
            context.UserData.TryGetValue<string>("Name", out name);
            return Chain.Return($"Thank you for using hotel bot: {name}");
        }
    }
}