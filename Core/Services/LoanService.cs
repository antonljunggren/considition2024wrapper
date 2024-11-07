using Core.Models;
using Core.Utils;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public sealed class LoanService
    {
        private readonly InMemoryStorage _storage;

        public LoanService(InMemoryStorage storage)
        {
            _storage = storage;
        }

        [KernelFunction("make_proposals")]
        [Description("Make load proposals for any customer, none or all")]
        public void MakeProposal(List<CustomerLoanRequestProposal> proposals)
        {
            _storage.AddLoanProposals(proposals);
        }

        [KernelFunction("get_proposals")]
        [Description("Get all loan proposals")]
        [return: Description("a list of all loan proposals")]
        public List<CustomerLoanRequestProposal> GetCustomerLoanProposals()
        {
            return _storage.GetCustomerLoanProposals();
        }
    }
}
