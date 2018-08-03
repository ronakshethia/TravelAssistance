using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelBot.Model;

namespace TravelBot.Dialogs
{
    [Serializable]
    public class HotelsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Welcome to the Hotels finder!");
            var hotelsFormDialog = FormDialog.FromForm(HotelBooking.BuildForm, FormOptions.PromptInStart);
            //var hotelsFormDialog = FormDialog.FromForm(this.BuildHotelsForm, FormOptions.PromptInStart);
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

        private async Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<HotelBooking> result)
        {
            try
            {
                var searchQuery = await result;
                var hotels = await this.GetHotelsAsync(searchQuery);


                //await context.PostAsync($"I found in total {hotels.Count()} hotels for your dates:");
                var hotelLink = $"https://www.cleartrip.com/hotels/results?city={searchQuery.location}&state=Maharashtra&country=IN&area=&poi=&hotelId=&hotelName=&dest_code=33719&chk_in={searchQuery.checkindate}&chk_out={searchQuery.checkoutdate}&adults1={searchQuery.Adults}&children1={searchQuery.Children}&num_rooms=1";


                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                foreach (var hotel in hotels)
                {
                    HeroCard heroCard = new HeroCard()
                    {
                        Title = hotel.Name,
                        Subtitle = $"{hotel.Rating} starts. {hotel.NumberOfReviews} reviews. From ${hotel.PriceStarting} per night.",
                        Images = new List<CardImage>()
                        {
                            new CardImage() { Url = hotel.Image }
                        },
                        Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "More details",
                                Type = ActionTypes.OpenUrl,
                                Value = hotelLink
                            }
                        }
                    };

                    resultMessage.Attachments.Add(heroCard.ToAttachment());
                }

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
        
        private async Task<IEnumerable<Hotel>> GetHotelsAsync(HotelBooking searchQuery)
        {
            var hotels = new List<Hotel>();
            string[] hotelImage = new string[] {
                "https://media-cdn.tripadvisor.com/media/photo-s/0f/72/3e/04/the-grand-hotel-excelsior.jpg",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRlXXP7iOnz4wNHi1ra4ZL7XZxTSjUKs6Ml4i08lYw67Xq4OKRhvw",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQodHJ-ISO9Vx_4Jm5Ez4fqxx3JzLPop5fEdO78G4KAj-3WkjBcJA",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQRdYGFOORzq-UW0RFmVNxh0XvU4jzlI1rAhzxKdeSgLsrRhLV6",
                "http://www.thehotel-brussels.be/d/brussels/images/page_home-feature-1.jpg?1498727489"
            };
            // Filling the hotels results manually just for demo purposes
            for (int i = 1; i <= 5; i++)
            {
                var random = new Random(i);
                Hotel hotel = new Hotel()
                {
                    Name = $"{searchQuery.location} Hotel {i}",
                    Location = searchQuery.location,
                    Rating = random.Next(1, 5),
                    NumberOfReviews = random.Next(0, 5000),
                    PriceStarting = random.Next(80, 450),
                    Image = hotelImage[i-1]
                };

                hotels.Add(hotel);
            }

            hotels.Sort((h1, h2) => h1.PriceStarting.CompareTo(h2.PriceStarting));

            return hotels;
        }
        
    }
}