using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using TravelBot.Model;

namespace TravelBot.Dialog
{
    [Serializable]
    public class BusFormFlow : IDialog<object>
    {
        public static string types;
        public static string sources;
        public static string destinations;

        public BusFormFlow()
        {
                
        }

        public BusFormFlow(string type, string source,string destination)
        {
            types = type;
           sources = source;
            destinations = destination;
        }

        public enum FromCity
        {
            Bangalore, Chennai, Madurai, Trichy, Coimbatore, Tanjore, Pudukkottai
        }
        /// <summary>
        /// To City Enum
        /// </summary>
        public enum ToCity
        {
            Bangalore, Chennai, Madurai, Trichy, Coimbatore, Tanjore, Pudukkottai
        }

        

        public enum Food
        {
            [Describe("Yes, I want South indian meal")]
            SMeal = 1,

            [Describe("Yes, I want South North Indain meal")]
            NMeal = 2,

            [Describe("Yes , I want Fruits")]
            Fruts = 3,
            [Describe("Thanks , I dont want any Food")]
            No = 4
        }


        /// <summary>
        /// Bus Type Enum
        /// </summary>
        public enum TrainsType
        {
            AC, Slepper, General
        }
        /// <summary>
        /// Gender
        /// </summary>
        public enum Gender
        {
            [Terms("M", "boy", "Male")]
            Male,
            [Terms("F", "girl", "Female")]
            Female
        }
        public enum MealDecision
        {
            Yes, No
        }
        public enum JourneyType
        {
            OneWay, TwoWay
        }

        public enum NoOfPeople
        {
            [Describe("Yes I am all Alone")]
            Yes = 1,
            [Describe("No I Have Someone With Me")]
            No = 2
        }

        [Prompt("May I Know Your Full Name?")]
        public string FullName;

        [Prompt("May I Know Your Gender? {&} {||}  ")]
        public Gender? gender;

        [Prompt("May I Know Your Age")]
        public int age;

        [Prompt("May I Know Your Plan For Journey {&}  {||}")]
        public JourneyType? journytype { get; set; }

        [Prompt("I Would Need {||}")]
        public TrainsType? trainclass { get; set; }

        [Prompt("On What Date Would I Book Your Journey?")]
        public DateTime? journydate { get; set; }

        [Prompt("You can Select {&}  {||}")]
        public FromCity? FromAddress;

        [Prompt("You can Select {&}  {||}")]
        public ToCity? ToAddress;

        [Prompt("Are you travelling Alone ? {||}")]
        public NoOfPeople? Peopleno;

        [Prompt("Oh.! Tell me how many tickets should i book ?")]
        public int nooftickets;

        [Prompt("On What Date Would You Like to Travel?")]
        public DateTime? DateOfJurney;

        [Prompt("On What Date Would You Like to Return?")]
        public DateTime? DateOfReturn;

        [Prompt("Where Can We Contact You ?")]
        public double phoneno { get; set; }

        [Prompt("Do You Want Add Meal To Your Journey? {&}  {||}")]
        public MealDecision? meal { get; set; }

        [Template(TemplateUsage.NotUnderstood, "Sorry , \"{0}\" Not avilable .", "Try again, I don't get \"{0}\".")]
        [Template(TemplateUsage.EnumSelectOne, "What kind of {&} would you like ? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
        public Food LunchFood;

        [Prompt("Your Arrival Location.. ? ")]
        public string source;

        [Prompt("Your Destination Location.. ? ")]
        public string destination;


        // [Prompt("You can Select {&}  {||}")]
        // public FromCity? FromAddress;
        //[Prompt("You can Select {&}  {||}")]
        //  public ToCity? ToAddress;
        // [Prompt("When you are satrting from")]
        //public DateTime? StartDate;
        //public BusType? BusTypes;
        //[Numeric(1, 5)]
        //public int? NumberofSeat;
        //[Optional]
        //public string Address;
        //[Pattern(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        //public string Email;

        //public Gender? SelectGender;
        //[Prompt("What is Your Name")]
        //public string Name;
        //public int Age;


        public static IForm<BusFormFlow> BuildForm()
        {
            return new FormBuilder<BusFormFlow>()
                    .Message("Welcome to the BotChat Train Ticket Booking !")
                    .Field(nameof(FullName))
                    .Field(nameof(gender))
                    .Field(nameof(trainclass))
                    .Field(nameof(journytype))
                    .Field(nameof(Peopleno))
                    .Field(new FieldReflector<BusFormFlow>(nameof(nooftickets)).SetActive((state)=>state.Peopleno==NoOfPeople.No))
               //     .Field(new FieldReflector<BusFormFlow>(nameof(FromAddress)))
                     //  context.ConversationData.GetValue<string>(ContextConstants.source);
                    .Field(nameof(source))
                    .Field(nameof(destination))
                    .Field(nameof(DateOfJurney))
                    .Field(new FieldReflector<BusFormFlow>(nameof(DateOfReturn)).SetActive((state)=>state.journytype==JourneyType.TwoWay))
                    .Field(nameof(phoneno))
                    .Field(nameof(meal))
                    .Field(new FieldReflector<BusFormFlow>(nameof(LunchFood)).SetActive((state)=>state.meal==MealDecision.Yes))
                
                    .Confirm(async(state)=> { return new PromptAttribute($"Is This Your Selection : {{*}} {{||}}"); })
                    
              //      .Message("We Processed Your Information")
                    //.AddRemainingFields()
                   // .Message("You will get confirmation email and SMS .Thanks for using Chat Bot Bus Booking")
                    .OnCompletion(async (context, profileForm) =>
                    {
                        await context.PostAsync("We Are Processing Your Ticket");
                    })
                    .Build();

        }

       

        private static bool CheckValueFromLuis(BusFormFlow state)
        {
            if(sources!= "source")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("We Started Processing");
            var TrainFLow = FormDialog.FromForm(BusFormFlow.BuildForm, FormOptions.PromptInStart);
            //var hotelsFormDialog = FormDialog.FromForm(this.BuildHotelsForm, FormOptions.PromptInStart);
            context.Call(TrainFLow, this.ResumeTrainDialogOnCompletion);
        }
        int no;
        private async Task ResumeTrainDialogOnCompletion(IDialogContext context, IAwaitable<BusFormFlow> result)
        {
          
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
            if (Res.journytype==JourneyType.OneWay)
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
                    Total = (120 * no ).ToString(),
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
    }
}