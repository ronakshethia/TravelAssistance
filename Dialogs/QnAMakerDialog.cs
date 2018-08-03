using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TravelBot.Dialogs
{
    //public class QnAMakerDialog : IDialog<IMessageActivity>
    //{

    //    protected readonly IQnAService[] services;
    //    public QnAMakerDialog(params IQnAService[] services);
    //    public IQnAService[] MakeServicesFromAttributes();
    //    public Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument);
    //    protected virtual Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result);
    //    protected virtual bool IsConfidentAnswer(QnAMakerResults qnaMakerResults);
    //    protected virtual Task QnAFeedbackStepAsync(IDialogContext context, QnAMakerResults qnaMakerResults);
    //    protected virtual Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result);
    //}

}