using System;
using System.Collections.Generic;
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

            if (activity.Text.ToLower().Contains("hungry"))
            {
                await context.Forward(new HungryDialog(), RecommendationReceived, activity);
            }
            else if (activity.Text.ToLower().Contains("feedback"))
            {
                await context.Forward(FormDialog.FromForm(Feedback.BuildForm), ResumeAfterFeedback, activity);
            }
            else
            {
                await context.Forward(new LuisRecommendationDialog(), LuisRecommendationReceived, activity);
            }
        }

        private async Task LuisRecommendationReceived(IDialogContext context, IAwaitable<object> result)
        {
            var recommendation = await result as Recommendation;

            var message = context.MakeMessage();
            message.Text = "I have a recommendation for you.";

            var buttons = new List<CardAction>()
            {
                new CardAction() { Title = "More Info", Type = ActionTypes.OpenUrl, Value= recommendation.Location.Website }
            };

            var images = new List<CardImage>
            {
                new CardImage(url: recommendation.Location.ImageUrl)
            };

            var card = new HeroCard()
            {
                Title = recommendation.Location.Name,
                Images = images,
                Buttons = buttons,
                Tap = buttons[0],
            };

            message.Attachments.Add(card.ToAttachment());
            await context.PostAsync(message);
        }

        private async Task RecommendationReceived(IDialogContext context, IAwaitable<Recommendation> result)
        {
            var recommendation = await result;
            if (recommendation != null)
            {
                await context.PostAsync("I'll check back after lunch to see how everything went.");
            }
            else
            {
                await context.PostAsync("That didn't go as planned. Let me know if your plans change and you want to get some lunch.");
            }

            context.Wait(MessageReceivedAsync);
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