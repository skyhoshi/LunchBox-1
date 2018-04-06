using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LunchBox.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (string.IsNullOrEmpty(activity.Text))
            {
                context.Wait(MessageReceivedAsync);
                return;
            }

            if (activity.Text.ToLower().Contains("hungry"))
            {
                await context.PostAsync("You're hungry? Ok, I can help you with that. Who is going to lunch with you?");
            }
            else
            {
                await context.PostAsync("Hey there! Let me know if you end up getting hungry.");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}