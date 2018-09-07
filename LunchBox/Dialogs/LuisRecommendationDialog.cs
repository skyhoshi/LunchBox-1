using LunchBox.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LunchBox.Dialogs
{
    [Serializable]
    public class LuisRecommendationDialog : LuisDialog<object>
    {
        private string _currentUser = "steve";

        public LuisRecommendationDialog() : base(new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"])))
        {
        }

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(@"Hmm, I'm not familiar with that. ¯\_(ツ)_/¯");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(@"Hey! Getting hungry? Let me know a bit about who's going and what you're in the mood for and I'll give you a recommendation.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("GetRecommendation")]
        public async Task GetRecommendationIntent(IDialogContext context, LuisResult result)
        {
            var criteria = new Criteria();

            foreach (var entity in result.Entities)
            {
                if (entity.Type == Entities.Cuisine)
                {
                    criteria.Cuisines.Add(entity.Entity);
                }
                else if (entity.Type == Entities.Person)
                {
                    if (Responses.PersonalRefences.Contains(entity.Entity))
                    {
                        criteria.Attendees.Add(_currentUser);
                    }
                    else
                    {
                        criteria.Attendees.Add(entity.Entity);
                    }
                }
            }

            if (!criteria.HasEnoughForRecommendation)
            {
                // Redirect to Hungry dialog to fill out the rest of the information
            }

            var recommendation = MakeRecommendation(criteria);
            context.Done(recommendation);
        }

        private Recommendation MakeRecommendation(Criteria criteria)
        {
            var service = new RecommendationService();
            return service.GetRecommendation(criteria);
        }
    }
}