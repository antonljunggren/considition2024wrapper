using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Description("A customer's loan proposal")]
    public sealed class Loan
    {
        [Description("What the customer to loan for")]
        [JsonProperty("product")]
        public string Product { get; private set; }

        [Description("The enviromental impact of the loan, the less the better")]
        [JsonProperty("environmental_impact")]
        public decimal EnvironmentalImpact { get; private set; }

        [Description("The amount the customer wants to loan")]
        [JsonProperty("amount")]
        public decimal Amount { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Loan() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        
        public Loan(string product, decimal environmentalImpact, decimal amount)
        {
            Product = product;
            EnvironmentalImpact = environmentalImpact;
            Amount = amount;
        }
    }
}
