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
    public sealed class CustomerService
    {
        private readonly InMemoryStorage _storage;

        public CustomerService(InMemoryStorage storage)
        {
            _storage = storage;
        }

        [KernelFunction("get_customers")]
        [Description("Gets all the customers for the game")]
        [return: Description("A list of customers")]
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return _storage.GetAllCustomers();
        }
    }
}
