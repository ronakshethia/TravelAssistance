using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TravelBot.Data;
using TravelBot.Model;

namespace TravelBot.Dialogs
{
    [Serializable]
    public class FlightDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("We Started With Your Flight Booking");
            var flightFormDialog = FormDialog.FromForm(FlightInquiry.BuildForm, FormOptions.PromptInStart);
            //var hotelsFormDialog = FormDialog.FromForm(this.BuildHotelsForm, FormOptions.PromptInStart);
            context.Call(flightFormDialog, this.ResumeAfterHotelsFormDialog);
        }

        private async Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<FlightInquiry> result)
        {
            try
            {
                var searchQuery = await result as FlightInquiry;
                // var hotel = await GetFlightsAsync(searchQuery);


                var city = new Dictionary<string, string>() {
                    { "mumbai","BOM"},
                    { "delhi","DEL"},
                    { "ahmedabad","AMD"},
                    { "goa","GOI"}
                };
                string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                string uploadPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\cities.json";
                string uploadPath1 = AppDomain.CurrentDomain.BaseDirectory + "Data\\airports.json";
                var citty = File.ReadAllText(uploadPath);
                var airport = File.ReadAllText(uploadPath1);

                            //  var treatments = JsonConvert.DeserializeObject<List<Class1>>(citty);
                var airs = JsonConvert.DeserializeObject<AirportModel>(airport);

               
                    var sou = airs.airports.Where(t => t.city_name.ToLower().Contains(searchQuery.Source.ToLower())).FirstOrDefault();
               
                var des = airs.airports.Where(t => t.city_name.ToLower().Contains(searchQuery.Destination.ToLower())).FirstOrDefault();


                var sourceCode = sou.IATA_code;
                var destinationCode = des.IATA_code;

                var datofj = searchQuery.DateOfJurney?.ToString("yyyyMMdd");
                var createLink = $"https://www.goibibo.com/flights/air-{sourceCode}-{destinationCode}-{datofj}--{searchQuery.NumberOfAdult}-{searchQuery.NumberOfChild}-{searchQuery.NumberOfInfants}-{GetEnumDescription(searchQuery.classTypes)}-D/";

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();
                string airindia = path + "\\airindia.png";
                string indigo = path + "\\indigo.png";
                string jet = path + "\\jet.png";
                string spicejet = path + "\\spicejet.jpg";

                HeroCard heroCard = new HeroCard()
                {
                    Title = sou.airport_name + " to \n" + des.airport_name,
                    Subtitle = "found something for you", Text ="Click Here To See Price's",
                    Images = new List<CardImage>()
                        {
                new CardImage() { Url = airindia }
                        },
                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "Click for More details",
                                Type = ActionTypes.OpenUrl,
                                Value = createLink
                            }
                        }
                };

                HeroCard heroCard1 = new HeroCard()
                {
                    Title = sou.airport_name + " to \n" + des.airport_name,
                    Subtitle = "found something for you",
                    Text = "Click Here For Price's",
                    Images = new List<CardImage>()
                        {
                            new CardImage() { Url = indigo }
                        },
                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "Click for More details",
                                Type = ActionTypes.OpenUrl,
                                Value = createLink
                            }
                        }
                };
                HeroCard heroCard2 = new HeroCard()
                {
                    Title = sou.airport_name + " to \n" + des.airport_name,
                    Subtitle = "found something for you",
                    Text = "Click Here For Price's",
                    Images = new List<CardImage>()
                        {
                            new CardImage() { Url = jet }
                        },
                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "Click for More details",
                                Type = ActionTypes.OpenUrl,
                                Value = createLink
                            }
                        }
                };
                HeroCard heroCard3 = new HeroCard()
                {
                    Title = sou.airport_name + " to \n" + des.airport_name,
                    Subtitle = "found something for you",
                    Text = "Click Here For Price's",
                    Images = new List<CardImage>()
                        {
                            new CardImage() { Url = spicejet }
                        },
                    Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "Click for More details",
                                Type = ActionTypes.OpenUrl,
                                Value = createLink
                            }
                        }
                };


                resultMessage.Attachments.Add(heroCard.ToAttachment());
                resultMessage.Attachments.Add(heroCard1.ToAttachment());
                resultMessage.Attachments.Add(heroCard2.ToAttachment());
                resultMessage.Attachments.Add(heroCard3.ToAttachment());

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

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        //private async Task<Hotel> GetFlightsAsync(FlightInquiry searchQuery)
        //{
        //    //Flight flight = null;
        //    //var requ = new FlightRequest()
        //    //{
        //    //    Source = searchQuery.Source,
        //    //    Destination = searchQuery.Destination,
        //    //    Adult = searchQuery.NumberOfAdult,
        //    //    Childern = searchQuery.NumberOfChild,
        //    //    Counter = 100,
        //    //    DateOfDepa = searchQuery.DateOfJurney?.ToString("yyyymmdd"),
        //    //    infants = searchQuery.NumberOfInfants
        //    //};

        //    //var response = await VendorApi.SearchFlight(requ);
        //    //if (!string.IsNullOrEmpty(response))
        //    //{
        //    //    flight = JsonConvert.DeserializeObject<Flight>(response);
        //    //}

        //    //return flight;


        //        Hotel hotel = new Hotel()
        //        {
        //            Name = "https://www.goibibo.com/flights/air-BOM-DEL-20180503--1-0-0-E-D/",
        //            Location = "https://www.goibibo.com/flights/air-BOM-DEL-20180503--1-0-0-E-D/",
        //            Rating = random.Next(1, 5),
        //            NumberOfReviews = random.Next(0, 5000),
        //            PriceStarting = random.Next(80, 450),
        //            Image = $"https://placeholdit.imgix.net/~text?txtsize=35&txt=Hotel+{i}&w=500&h=260"
        //        };


        //    return hotels;

        //}

    }
}