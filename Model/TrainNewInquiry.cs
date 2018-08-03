using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelBot.Model
{
    public enum Gender
    {
        Male, Female, Other
    }

    public enum MealDecision
    {
        Yes, No
    }
    public enum JourneyType
    {
        OneWay, TwoWay
    }
    public class TrainNewInquiry
    {
        [Prompt("May I Know Your Full Name?")]
        public string FullName;
        [Prompt("May I Know Your Gender? {&} {||}  ")]
        public Gender? gender;
        [Prompt("May I Know Your Age")]
        public int age { get; set; }
        [Prompt("May I Know Your Plan For Journey? {&}  {||}")]
    public JourneyType? journytype { get; set; }
        [Prompt("On What Date Would I Book Your Journey?")]
        public DateTime? DateOfJurney;
        [Prompt("On What Date Would You Like to Return?")]
        public DateTime? DateOfReturn;
        [Prompt("Where Can We Contact You ?")]
        public double phoneno { get; set; }
        [Prompt("Do You Want Add Meal To Your Journey? {&}  {||}")]
     public MealDecision? meal { get; set; }


        public static IForm<TrainNewInquiry> BuildForm()
        {
            var form = new FormBuilder<TrainNewInquiry>();
            List<string> quitCommands = new List<string>();
            quitCommands.Add("cancel");
            quitCommands.Add("quit");

            form.Configuration.Commands[FormCommand.Quit].Terms = quitCommands.ToArray();

            return form
            
              //  .Field(nameof(FullName))
                //.Field(nameof(gender))
              //  .Field(nameof(age))
              // .Field(nameof(journytype))
              //  .Field(nameof(DateOfJurney))
              //  .Field(nameof(DateOfReturn))
              //  .Field(nameof(phoneno))
               //.Field(nameof(meal))
             //   .OnCompletion(async (context, profileForm) =>
              //   {
                     // Tell the user that the form is complete
              //      await context.PostAsync("done");
              //   })
                .Build();
        }


    }
}