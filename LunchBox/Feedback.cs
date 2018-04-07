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
        [Prompt("Where did you end up going to lunch?")]
        public string Location;
        [Prompt("How was the food? {||}")]
        public Rating FoodRating;
        [Prompt("How was the service? {||}")]
        public Rating ServiceRating;
        [Optional]
        [Prompt("Do you have anything else to add?")]
        public string Comments;

        private static string[] _negativeResponses = new[]
        {
            "no",
            "nope",
            "nay",
            "nada",
            "nothing",
            "no response",
            "no comment",
            "n/a",
            "I don't",
            "I do not"
        };

        public static IForm<Feedback> BuildForm()
        {
            return new FormBuilder<Feedback>()
                .Message("Tell me about your lunch.")
                .Field(nameof(Location))
                .Field(nameof(FoodRating))
                .Field(nameof(ServiceRating))
                .Field(nameof(Comments), validate: async (state, value) =>
                {
                    var comment = (string)value;
                    // The bot framework already handles a bunch of negative response and leaves the "value" null.
                    // This catches more of those.
                    if (!string.IsNullOrEmpty(comment) && _negativeResponses.Contains(comment.ToLower()))
                    {
                        // Then make the comment blank
                        return new ValidateResult { IsValid = true, Value = null };
                    }

                    // Otherwise just bypass the validation and accept the comment the user gave.
                    return new ValidateResult { IsValid = true, Value = comment };
                })
                .Build();
        }
    }
}