using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TravelBot.Dialogs;
using TravelBot.Extras;
using static TravelBot.Dialog.BusFormFlow;

namespace TravelBot.Dialog
{
    public enum booleanChoice { Yes, No, maybe };

    [LuisModel(AppSetter.appid, AppSetter.usersecretidid)]
    [Serializable]
    public class GreetingDialogPerform : LuisDialog<object>
    {
        private const string FlightsOption = "Flights";
        private const string HotelsOption = "Hotels";
        private const string TrainOption = "Train";
        private const string PNROption = "Check PNR Status";
        private const string GobackOption = "Book Later";


        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I am Not that Smart to  Understand this..");
            context.Wait(MessageReceived);
        }

        string source;
        string destination;
        string travelclass;
        public List<EntityRecommendation> entities;
      [LuisIntent("TrainBooking")]
        public async Task TrainInfo(IDialogContext context, LuisResult result)
        {
             entities = new List<EntityRecommendation>(result.Entities);

            foreach (var entity in result.Entities)
            {
                if (entity.Type == "trainbook::source")
                {
                    entities.Add(new EntityRecommendation(type: "source") { Entity = entity.Entity });
                    source = entity.Entity;
                }
                if (entity.Type == "trainbook::destination")
                {
                    entities.Add(new EntityRecommendation(type: "destination") { Entity = entity.Entity });
                    destination = entity.Entity;
                }
                if (entity.Type == "trainbook::class")
                {
                    entities.Add(new EntityRecommendation(type: "trainclass") { Entity = entity.Entity });
                    travelclass = entity.Entity;
                }
                if (entity.Type == "builtin.datetimeV2.date")
                {
                    entities.Add(new EntityRecommendation(type: "DateOfJurney") { Entity = entity.Entity });
                    travelclass = entity.Entity;
                }
               // if (entity.Type == "builtin.number")
                //{
                //    entities.Add(new EntityRecommendation(type: "trainclass") { Entity = entity.Entity });
                //    travelclass = entity.Entity;
               // }
            }
           


          //////  foreach (var entity in result.Entities)
          //////  {
          //////      if(entity.Type=="trainbook::source")
          //////      {
                  
          //////          if (!context.ConversationData.TryGetValue(ContextConstants.source, out source))
          //////          {
          //////              source = entity.Entity;
          //////              context.ConversationData.SetValue(ContextConstants.source, source);

          //////              entities.Add(new EntityRecommendation(type: "trainbook::source") { Entity = source });
                       
          //////          }
          //////      }
          //////      if(entity.Type== "trainbook::destination")
          //////      {
          //////          if (!context.ConversationData.TryGetValue(ContextConstants.destination, out destination))
          //////          {
          //////              destination = entity.Entity;
          //////              context.ConversationData.SetValue(ContextConstants.destination, destination);
          //////          }
          //////      }
          //////      if(entity.Type== "trainbook::class")
          //////      {
          //////          if (!context.ConversationData.TryGetValue(ContextConstants.traveltype, out travelclass))
          //////          {
          //////              travelclass = entity.Entity;
          //////              context.ConversationData.SetValue(ContextConstants.traveltype, travelclass);
          //////          }
          //////      }
          //////  }
            await context.PostAsync($"I am Making {travelclass} Booking From {source} To {destination}");
          // PromptDialog.Confirm(context, OnConfirmation, "Should I Procced ?", "Didn't get that", 3, PromptStyle.Auto);

           PromptDialog.Choice(
             context: context,
             resume: ChoiceReceivedAsync,
              options: (IEnumerable<booleanChoice>)Enum.GetValues(typeof(booleanChoice)),
              prompt: "Should I Procced ?",
              retry: "Didn't get that",
              promptStyle: PromptStyle.Auto
         );

            // await context.PostAsync($"Book Ticket For {source} To {destination} in {travelclass} , Is that what you want me to do ??" ? "Yes", "No");

            //  await context.PostAsync($"Welcome {ContextConstants.source} ");

            //string  city = context.ConversationData.GetValue<string>(ContextConstants.source);
            //string city1 = context.ConversationData.GetValue<string>(ContextConstants.destination);
            //string city2 = context.ConversationData.GetValue<string>(ContextConstants.traveltype);

            // context.Wait(this.MessageReceivedAsync);
        }

        

        private async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<booleanChoice> result)
        {
            var res = await result;
            if(res == booleanChoice.Yes)
            {
                var busflow = new FormDialog<BusFormFlow>(new BusFormFlow(), BusFormFlow.BuildForm, FormOptions.PromptInStart, entities);

                context.Call<BusFormFlow>(busflow, this.ResumeAfterCall);
             //   context.Call(new BusFormFlow(), this.ResumeAfterCall);
            }
            if(res== booleanChoice.No)
            {


            }
            if(res == booleanChoice.maybe)
            {

            }
           
        }
        int no;
        private async Task ResumeAfterCall(IDialogContext context, IAwaitable<BusFormFlow> result)
        {
            //throw new NotImplementedException();
            // return null;
            Random R = new Random();
            var Res = await result;

            if (Res.nooftickets == 0)
            {
                no = 1;
            }
            else
            {
                no = Res.nooftickets;
            }
            if (Res.journytype == JourneyType.OneWay)
            {
                var receiptCard = new ReceiptCard
                {
                    Title = Res.FullName,
                    Facts = new List<Fact> { new Fact("PNR Number", R.Next(100000, 800000).ToString()), new Fact("Payment Method", "VISA 5555-****"), new Fact("Source", Res.FromAddress.ToString()), new Fact("Destination", Res.ToAddress.ToString()), new Fact("Date", Res.DateOfJurney.ToString()), new Fact("Return Date", Res.DateOfReturn.ToString()), new Fact("Journey Type", Res.journytype.ToString()) },
                    Items = new List<ReceiptItem>
                {
                    new ReceiptItem("Per Ticket", price: $"Rs 120", quantity: Res.nooftickets.ToString(), image: new CardImage(url: "http://www.pngmart.com/files/6/Ticket-PNG-Picture.png")),
                   // new ReceiptItem("App Service", price: "$ Rs 80", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                    Tax = "18.5 %",
                    Total = (120 * no).ToString(),
                    Buttons = new List<CardAction>
                {
                    new CardAction(
                        ActionTypes.OpenUrl,
                        "More information",
                        "http://st1.bgr.in/wp-content/uploads/2014/07/indian-railway-logo1.png",
                        "https://enquiry.indianrail.gov.in/ntes/index.html")
                }
                };
                var resultMessage = context.MakeMessage();
                var receipt = receiptCard.ToAttachment();
                resultMessage.Attachments.Add(receipt);
                await context.PostAsync(resultMessage);
            }

            if (Res.journytype == JourneyType.TwoWay)
            {

                var receiptCard1 = new ReceiptCard
                {
                    Title = Res.FullName,
                    Facts = new List<Fact> { new Fact("PNR Number", R.Next(100000, 800000).ToString()), new Fact("Payment Method", "VISA 5555-****"), new Fact("Source", Res.FromAddress.ToString()), new Fact("Destination", Res.ToAddress.ToString()), new Fact("Date", Res.DateOfJurney.ToString()), new Fact("Return Date", Res.DateOfReturn.ToString()), new Fact("Journey Type", Res.journytype.ToString()) },
                    Items = new List<ReceiptItem>
                {
                    new ReceiptItem("Per Ticket", price: $"Rs 120", quantity: Res.nooftickets.ToString(), image: new CardImage(url: "http://www.pngmart.com/files/6/Ticket-PNG-Picture.png")),
                   // new ReceiptItem("App Service", price: "$ Rs 80", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                    Tax = "18.5 %",
                    Total = (120 * no).ToString(),
                    Buttons = new List<CardAction>
                {
                    new CardAction(
                        ActionTypes.OpenUrl,
                        "More information",
                        "http://st1.bgr.in/wp-content/uploads/2014/07/indian-railway-logo1.png",
                        "https://enquiry.indianrail.gov.in/ntes/index.html")
                }
                };

                var receiptCard = new ReceiptCard
                {
                    Title = Res.FullName,
                    Facts = new List<Fact> { new Fact("PNR Number", R.Next(100000, 800000).ToString()), new Fact("Payment Method", "VISA 5555-****"), new Fact("Source", Res.ToAddress.ToString()), new Fact("Destination", Res.FromAddress.ToString()), new Fact("Date", Res.DateOfJurney.ToString()), new Fact("Return Date", Res.DateOfReturn.ToString()), new Fact("Journey Type", Res.journytype.ToString()) },
                    Items = new List<ReceiptItem>
                {
                    new ReceiptItem("Per Ticket", price: $"Rs 120", quantity: Res.nooftickets.ToString(), image: new CardImage(url: "http://www.pngmart.com/files/6/Ticket-PNG-Picture.png")),
                   // new ReceiptItem("App Service", price: "$ Rs 80", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                    Tax = "18.5 %",
                    Total = (120 * no).ToString(),
                    Buttons = new List<CardAction>
                {
                    new CardAction(
                        ActionTypes.OpenUrl,
                        "More information",
                        "http://st1.bgr.in/wp-content/uploads/2014/07/indian-railway-logo1.png",
                        "https://enquiry.indianrail.gov.in/ntes/index.html")
                }
                };
                var resultMessage = context.MakeMessage();
                var receipt = receiptCard.ToAttachment();
                var receipt2 = receiptCard1.ToAttachment();
                resultMessage.Attachments.Add(receipt2);
                resultMessage.Attachments.Add(receipt);

                await context.PostAsync(resultMessage);
            }


            context.Done<object>(null);

        }

        //private async Task OnConfirmation(IDialogContext context, IAwaitable result)
        //{
        //    var res = await result;
        //   //  await context.PostAsync("Great..! Would like to know some details from you");

        //    context.Call(new MyTrainClass(), this.ResumeAfterOptionDialog);
        //}



        [LuisIntent("SendAway")]
        public async Task Seeya(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("cyaa soon..!!");
        }




        [LuisIntent("Greeting")]
        public async Task StartAsync(IDialogContext context, LuisResult result)
        {
            string message = "";
            var myDate = new DateTime();
            myDate = DateTime.Now;
            if (myDate.Hour >= 0 && myDate.Hour < 12)
            {
                message = "Good Morning";
            }
            else if (myDate.Hour >= 12 && myDate.Hour < 17)
            {
                message = "Good Afternoon";
            }
            else if (myDate.Hour >= 18)
            {
                message = "Good Evening";
            }
            await context.PostAsync(message + "\n" + "Welcome to travel bot. How Can I Help You ?");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Services")]
        public async Task Services(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I can make Hotel & Travel Booking's for you");
        }


        List<EntityRecommendation> botinfo;
        [LuisIntent("Botsinfo")]
        public async Task BotInfo(IDialogContext context, LuisResult result)
        {
            botinfo = new List<EntityRecommendation>(result.Entities);

            foreach(var bot in botinfo)
            {
                
                switch(bot.Entity)
                {
                    case "creator":
                        {
                            var resultMessage = context.MakeMessage();
                            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                            HeroCard heroCard = new HeroCard()
                            {
                                Title = "Techease Systems",
                                Subtitle = "D -210 Neelkanth Business Park, Ramdev nagar Rd., Vidyavihar (W), Mumbai, Maharashtra 400086",
                                Text = "022 2510 4230",
                                Images = new List<CardImage>()
                        {
                            new CardImage() { Url = "http://www.techeasesystems.in/img/logo-short.png" }
                        },
                                Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "More details",
                                Type = ActionTypes.OpenUrl,
                                Value = "http://www.techeasesystems.in/"
                            }
                        }
                            };

                            resultMessage.Attachments.Add(heroCard.ToAttachment());
                            await context.PostAsync(resultMessage);
                            break;
                        }
                    case "company": 
                        {
                            
                            break;
                        }
                    case "services":
                        {
                            //var ss = Task.Run(async () =>
                            await context.PostAsync("Can Book Travel Tickets");
                            break;
                        }
                    case "technology":
                        {
                            //var ss = Task.Run(async () => 
                            await context.PostAsync("Bot Framework 3.7");
                            break;
                        }
                    case "trainer":
                        {
                            
                            break;
                        }
                    case "version":
                        {
                     
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }

            }


            
        }

        [LuisIntent("TravelBooking")]
        public async Task Booking(IDialogContext context, LuisResult result)
        {
            await this.ShowOptionsAsync(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpMe(IDialogContext context, LuisResult result)
        {
            // await this.ShowOptionsAsync(context, result);
            await context.PostAsync("Your Can Start With Asking Me to \" Book My Ticket or Travel Booking \" "
               
                );
        }


        [LuisIntent("Report")]
        public async Task Report(IDialogContext context, LuisResult result)
        {
            //  context.Call(new FeedbackDialog("Did you solve problem ?",""), this.ResumeAfterOptionDialog);
            PromptDialog.Text(context, AfterPromptMethod, "Ohh.. tell us your concern please..?", attempts: 3);
        }

       public async Task AfterPromptMethod(IDialogContext context, IAwaitable<string> userInput)
        {
            Random r = new Random();
            r.Next(1000,2000);
          await  context.PostAsync("we have reported your concern" +"\n" +"Our Support Team will resolve the issue soon");
            await context.PostAsync("here is your ticket no " + r.Next() + " for futher assistance");
            context.Done<object>(null);
        }

        [LuisIntent("Complements")]
        public async Task Complements(IDialogContext context, LuisResult result)
        {
            var test = result.Entities[0].Type;
            if(test== "Comp::bad")
            {
                await context.PostAsync("Sorry..! would try to make improvements in my next release");
            }
            else if(test== "Comp::good")
            {
                await context.PostAsync("Thankyou..!!");
            }
           else if(test== "Comp::abuse")
            {
                await context.PostAsync("Are you talking about yourself..??");
            }
            else
            {
                await context.PostAsync("i am not able to understand that complement..");
            }
            //var act = result.Entities;
            //  string origin = act[0].Entity;
            //  string des = act[1].Entity;             
        }

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is .");
            // context.Wait(this.MessageReceivedAsync);
        }
        private async Task ShowOptionsAsync(IDialogContext context, LuisResult result)
        {
            //var act = result.Entities;
          //  string origin = act[0].Entity;
          //  string des = act[1].Entity;
       

            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { FlightsOption, HotelsOption, TrainOption,PNROption ,GobackOption }, "Sure..! Tell me what booking would like to make..?", "Not a valid option", 3);
           // context.Call(new FlightDialog(), this.ResumeAfterOptionDialog);
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
                        context.Call(new BusFormFlow(), this.ResumeAfterOptionDialog);             
                        break;
                    case PNROption:
                        context.Call(new TrainDialog(), this.ResumeAfterOptionDialog);
                        break;
                    
                    case GobackOption:
                        // await this.ResumeAfterOptionDialog(context,result);
                        context.Done<object>(null);
                        await context.PostAsync("Got That..! Type Help To Know More About Me");
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attempts :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceived);
            }
        }


        private async Task StopBook(IDialogContext context, IAwaitable<string> result)
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
        }
        private async Task OnFlightClass(IDialogContext context, IAwaitable<string> result)
        {
            context.Call(new FlightDialog(), this.ResumeAfterOptionDialog);
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
                //  await context.PostAsync($"Thanks for using our Bot..!" + "\n"+ "See you soon..");
                context.Call(new FeedbackDialog("Did you solve problem ?", ""), this.Completion);
                
                // context.Done<object>(null);
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
        public virtual async Task Completion(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
          await context.PostAsync("we can start again anytime");
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Wait(MessageReceivedAsync);
        }
    }
}