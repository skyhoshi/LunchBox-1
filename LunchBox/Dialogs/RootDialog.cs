using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
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

            //context.ConversationData.SetValue("cats", "dogs")

            if (activity.Text.ToLower().Contains("hungry"))
            {
                await context.PostAsync("You're hungry? Ok, I can help you with that. Who is going to lunch with you?");
            }
            else if (activity.Text.ToLower().Contains("feedback"))
            {
                await context.Forward(FormDialog.FromForm(Feedback.BuildForm), ResumeAfterFeedback, activity);
            }
            else
            {
                await context.PostAsync("Hey there! Let me know if you end up getting hungry.");
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterFeedback(IDialogContext context, IAwaitable<Feedback> result)
        {
            var feedback = await result;
            if (feedback != null)
            {
                if ((int)feedback.FoodRating < 3)
                {
                    await context.PostAsync("Sorry to hear the food wasn't great. I'll remember that for next time. Thanks for the feedback.");
                }
                else
                {
                    await context.PostAsync("Got it! I'll remember that for next time. Thanks for the feedback.");
                }
            }
            else
            {
                await context.PostAsync("OK. Well... I guess we can talk about it later.");
            }
            context.Wait(this.MessageReceivedAsync);
        }
    }
}