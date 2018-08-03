using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using System;

namespace TravelBot.Dialogs
{
    public class SupportDialog : IDialog<int>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("posting");
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            var ticketNumber = message.Text;
            await context.PostAsync($"Your message '{message.Text}' was registered. Once we resolve it; we will get back to you.");
           // context.Done(ticketNumber);
            context.Wait(MessageReceivedAsync);
        }
    }
}