using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LunchBox
{
    [Serializable]
    public class Feedback
    {
        public Rating FoodRating;
        public Rating ServiceRating;
        [Optional]
        public string Comments;
         
        public static IForm<Feedback> BuildForm()
        {
            return new FormBuilder<Feedback>()
                .Field(nameof(FoodRating))
                .Field(nameof(ServiceRating))
                .Field(nameof(Comments))
                .Build();
        }
    }
}