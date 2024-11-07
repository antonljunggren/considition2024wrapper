using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Models.Award;
using static Core.Models.CustomerAction;

namespace Core.Models
{
    public sealed class GameInput
    {
        public required string MapName { get; init; }
        public required List<CustomerLoanRequestProposal> Proposals { get; init; }
        public required List<Dictionary<string, CustomerAction>> Iterations { get; init; }
    }

    [Description("The customer action input")]
    public sealed class CustomerActionInput
    {
        [Description("The customer who gets the action")]
        [JsonProperty("CustomerName")]
        public string CustomerName { get; private set; }

        [Description("The action type")]
        [JsonProperty("ActionType")]
        public CustomerActionType ActionType { get; private set; }

        [Description("The award type")]
        [JsonProperty("AwardType")]
        public AwardType? AwardType { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private CustomerActionInput() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public CustomerActionInput(string customerName, CustomerActionType actionType, AwardType? awardType)
        {
            CustomerName = customerName;
            ActionType = actionType;
            AwardType = awardType;
        }
    }

    [Description("A action that can give a award or just skip")]
    public sealed class CustomerAction
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CustomerActionType
        {
            Skip,
            Punish,
            Award
        }

        [Description("The action's type")]
        [JsonProperty("type")]
        public CustomerActionType Type { get; private set; }

        [Description("The action's award given to the customer for the month game turn")]
        [JsonProperty("award")]
        public AwardType Award { get; private set; }

        private CustomerAction() { }
        public CustomerAction(CustomerActionType type, AwardType award)
        {
            Type = type;
            Award = award;
        }
    }

    [Description("The loan proposed to the customer for the game")]
    public sealed class CustomerLoanRequestProposal
    {
        [Description("The name of the customer to get the loan proposal")]
        [JsonProperty("customer_name")]
        public string CustomerName { get; private set; }

        [Description("The yearly interest rate of the loan proposal, in decimal")]
        [JsonProperty("yearly_interest_rate")]
        public decimal YearlyInterestRate { get; private set; }

        [Description("The number of months to pay back the loan. It can be longer or shorter than the game map months")]
        [JsonProperty("months_to_pay_back_loan")]
        public int MonthsToPayBackLoan { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private CustomerLoanRequestProposal() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public CustomerLoanRequestProposal(string customerName, decimal yearlyInterestRate, int monthsToPayBackLoan)
        {
            CustomerName = customerName;
            YearlyInterestRate = yearlyInterestRate;
            MonthsToPayBackLoan = monthsToPayBackLoan;
        }
    }
}
