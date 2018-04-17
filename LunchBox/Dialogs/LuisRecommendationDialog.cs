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

        [LuisIntent("GetRecommendation")]
        public async Task GetRecommendationIntent(IDialogContext context, LuisResult result)
        {
            Criteria criteria;
            if (!context.PrivateConversationData.TryGetValue("criteria", out criteria))
            {
                criteria = new Criteria();
                context.PrivateConversationData.SetValue("criteria", criteria);
            }

            var people = new List<string>();
            var cuisines = new List<string>();
            foreach (var entity in result.Entities)
            {
                if (entity.Type == Entities.Cuisine)
                {
                    cuisines.Add(entity.Entity);
                }
                else if (entity.Type == Entities.Person)
                {
                    if (Responses.PersonalRefences.Contains(entity.Entity))
                    {
                        people.Add(_currentUser);
                    }
                    else
                    {
                        people.Add(entity.Entity);
                    }
                }
            }

            criteria.Attendees = people;
            criteria.Cuisines = cuisines;
            criteria.HasTimeRestrictions = false;

            context.PrivateConversationData.SetValue("criteria", criteria);

            if (!criteria.HasEnoughForRecommendation)
            {
                // Redirect to Hungry dialog to fill out the rest of the information
            }
            else
            {
                var recommendation = MakeRecommendation(criteria);
                context.Done(recommendation);
            }
        }

        private Recommendation MakeRecommendation(Criteria criteria)
        {
            var service = new RecommendationService();
            return service.GetRecommendation(criteria);
        }
    }
}