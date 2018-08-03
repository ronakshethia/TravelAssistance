using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using TravelBot.Model;
using TravelBot.Dialogs;
using TravelBot.Dialog;

namespace TravelBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        internal static IDialog<HotelBooking> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(HotelBooking.BuildForm));
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new GreetingDialogPerform());
            }
            else
            {
                HandleSystemMessageAsync(activity);

               
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
              ////  ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
               // Activity reply = message.CreateReply("Hello from my simple Bot!");
             ///   connector.Conversations.ReplyToActivityAsync(reply);
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if  (message.Type == ActivityTypes.Typing)
            {
                //var reply = message.CreateReply("I AM Typing");
              //  reply.Type = ActivityTypes.Typing;
             //   await message.SendResponse(reply);
                // Handle knowing tha the user is typing

            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}