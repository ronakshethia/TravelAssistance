using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TravelBot.Model;

namespace TravelBot.Dialog
{
 

    [Serializable]
    public class MyTrainClass : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Allow Me To Ask Some Details Please");
            //   PromptDialog.Confirm(context, OnConfirmationOfOk, "Should I Procced ?", "Didn't get that", 3, PromptStyle.Auto);
            var mytraindialog = FormDialog.FromForm(TrainNewInquiry.BuildForm, FormOptions.PromptInStart);
            context.Call(mytraindialog,this.ResumeAfterTrainDialog );
        }

        private async Task ResumeAfterTrainDialog(IDialogContext context, IAwaitable<TrainNewInquiry> result)
        {
            await context.PostAsync("fgfd");
        }




        //public static IForm<TrainNewInquiry> BuildForm()
        //{
        //    return new FormBuilder<TrainNewInquiry>()
               
        //        .Build();
                    
        //}
    }
}