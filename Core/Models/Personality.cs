using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Description("A customer's personality")]
    public sealed class Personality
    {
        [Description("The customer's personality name")]
        [JsonProperty("name")]
        public string Name { get; private set; }

        [Description("The customer's personality happiness multiplier")]
        [JsonProperty("happiness_multiplier")]
        public float HappinessMultiplier { get; private set; }

        [Description("The customer's minimum accepted interest rate. Getting a lower interest might make them unhappy.")]
        [JsonProperty("accepted_min_interest")]
        public float acceptedMinInterest { get; private set; }

        [Description("The customer's maximum accepted interest rate. Getting a higher interest might make them unhappy.")]
        [JsonProperty("accepted_max_interest")]
        public float acceptedMaxInterest { get; private set; }

        [Description("The customer's living standard multiplier that affects their happiness")]
        [JsonProperty("living_standard_sultiplier")]
        public float livingStandardMultiplier { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Personality() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Personality(string name, float happinessMultiplier, float acceptedMinInterest, float acceptedMaxInterest, float livingStandardMultiplier)
        {
            Name = name;
            HappinessMultiplier = happinessMultiplier;
            this.acceptedMinInterest = acceptedMinInterest;
            this.acceptedMaxInterest = acceptedMaxInterest;
            this.livingStandardMultiplier = livingStandardMultiplier;
        }
    }
}
