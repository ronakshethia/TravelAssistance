using AdaptiveCards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelBot.Helper;
using TravelBot.Model;

namespace TravelBot.Dialogs
{
    [Serializable]
    public class TrainDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Welcome to Train Bot System!");
            var hotelsFormDialog = FormDialog.FromForm(TrainInquiry.BuildForm, FormOptions.PromptInStart);
            context.Call(hotelsFormDialog, this.ResumeAfterHotelsFormDialog);
        }

        private IForm<HotelsQuery> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<HotelsQuery> processHotelsSearch = async (context, state) =>
            {
                await context.PostAsync($"Ok. Searching for Hotels in {state.Destination} from {state.CheckIn.ToString("MM/dd")} to {state.CheckIn.AddDays(state.Nights).ToString("MM/dd")}...");
            };

            return new FormBuilder<HotelsQuery>()
                .Field(nameof(HotelsQuery.Destination))
                .Message("Looking for hotels in {Destination}...")
                .AddRemainingFields()
                .OnCompletion(processHotelsSearch)
                .Build();
        }

        private async Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<TrainInquiry> result)
        {
            try
            {
                var searchQuery = await result;
                var pnrModel = await this.GetPnrAsync(searchQuery.PnrNumber);
                var resultMessage = context.MakeMessage();
                resultMessage.Attachments = new List<Attachment>();

                AdaptiveCard card = new AdaptiveCard();

                // Specify speech for the card.
                card.Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>";

                // Add text to the card.
                card.Body.Add(new TextBlock()
                {
                    Text = "Your PNR Status for :"+ searchQuery.PnrNumber,
                    Size = TextSize.Large,
                    Weight = TextWeight.Bolder
                });
                card.Body.Add(new TextBlock()
                {
                    Text = $"Train Number: {pnrModel.train.number}"
                });
                card.Body.Add(new TextBlock()
                {
                    Text = $"Train Name: {pnrModel.train.name}"
                });
                card.Body.Add(new TextBlock()
                {
                    Text = $"Journey Class: {pnrModel.journey_class.code}"
                });
                // Add text to the card.
                card.Body.Add(new TextBlock()
                {
                    Text = $"Total passanger: {pnrModel.total_passengers}" 
                });

                // Add text to the card.
                card.Body.Add(new TextBlock()
                {
                    Text = $"From Station: {pnrModel.from_station.name}"
                });

                // Add text to the card.
                card.Body.Add(new TextBlock()
                {
                    Text = $"Reservation upto Station: {pnrModel.reservation_upto.name}"
                });
                // Add text to the card.
                card.Body.Add(new TextBlock()
                {
                    Text = $"Date of journey: {pnrModel.doj}"
                });
               

                int counter = 1;
                foreach (var item in pnrModel.passengers)
                {
                    card.Body.Add(new TextBlock()
                    {
                        Text = $"Passanger {counter} booking_status: {item.booking_status} || Current Status: {item.current_status}"
                    });
                }
                                               

                // Create the attachment.
                Attachment attachment = new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                };

                resultMessage.Attachments.Add(attachment);

                //var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);


                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation. Quitting from the HotelsDialog";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }

        private async Task<RailPNR> GetPnrAsync(string pnr)
        {
            var data = await VendorApi.GetPNR(pnr);
            var model = JsonConvert.DeserializeObject<RailPNR>(data);
            return model;
        }

    }
}