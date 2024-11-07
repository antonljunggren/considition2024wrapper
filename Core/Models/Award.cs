using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Description("A award the bank can give a customer that affects the customer's happiness comes at a monetary cost to the bank")]
    public sealed class Award
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum AwardType
        {
            None,
            IkeaCheck,
            IkeaFoodCoupon,
            IkeaDeliveryCheck,
            NoInterestRate,
            GiftCard,
            HalfInterestRate,
        }

        [Description("The award type")]
        [JsonProperty("type")]
        public AwardType Type { get; private set; }

        [Description("The cost of the award to the bank")]
        [JsonProperty("cost")]
        public float Cost { get; private set; }

        [Description("The award's base hapiness affect on the customer it is given to")]
        [JsonProperty("base_happiness")]
        public int BaseHappiness { get; private set; }

        private Award() { }

        public Award(AwardType type, float cost, int baseHappiness)
        {
            Type = type;
            Cost = cost;
            BaseHappiness = baseHappiness;
        }
    }
}
