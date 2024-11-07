using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Description("A customer that wants a loan")]
    public sealed class Customer
    {
        [Description("The customer's name")]
        [JsonProperty("name")]
        public string Name { get; private set; }

        [Description("The customer's loan that they want")]
        [JsonProperty("loan")]
        public Loan Loan { get; private set; }

        [Description("The customer's personality that affects their spending habits and their likelihood of accepting your proposal")]
        [JsonProperty("personality")]
        public Personality Personality { get; private set; }

        [Description("The customer's total capital")]
        [JsonProperty("capital")]
        public decimal Capital { get; private set; }

        [Description("The customer's monthly income. This, combined with their capital, determines whether they will be able to repay their loan each month")]
        [JsonProperty("income")]
        public decimal Income { get; private set; }

        [Description("The customer's monthly expenses, what they spend each month")]
        [JsonProperty("monthly_expenses")]
        public decimal MonthlyExpenses { get; private set; }

        [Description("The customer's number of kids. For every child a customer has, it incurs a set cost of 2000 SEK each month")]
        [JsonProperty("number_of_kids")]
        public int NumberOfKids { get; private set; }

        [Description("The customer's mortgage. If the customer has a mortgage, they will pay 0.1% of it every month")]
        [JsonProperty("home_mortgage")]
        public decimal HomeMortgage { get; private set; }

        [Description("The customer's student loan. If the customer has student loans, they will subtract 2,000 SEK every third month")]
        [JsonProperty("has_student_loan")]
        public bool HasStudentLoan { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private Customer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Customer(string name, Loan loan, Personality personality, decimal capital, decimal income, 
            decimal monthlyExpenses, int numberOfKids, decimal homeMortgage, bool hasStudentLoan)
        {
            Name = name;
            Loan = loan;
            Personality = personality;
            Capital = capital;
            Income = income;
            MonthlyExpenses = monthlyExpenses;
            NumberOfKids = numberOfKids;
            HomeMortgage = homeMortgage;
            HasStudentLoan = hasStudentLoan;
        }
    }
}
