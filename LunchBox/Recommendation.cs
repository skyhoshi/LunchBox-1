using System;
using System.Collections.Generic;

namespace LunchBox
{
    public class Criteria
    {
        public IEnumerable<string> Attendees { get; set; }
        public bool? HasTimeRestrictions { get; set; }
        public TimeSpan? LunchDuration { get; set; }
    }

    public class Recommendation
    {
        
    }
}