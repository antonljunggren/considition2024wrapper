using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public sealed class InMemoryStorage
    {
        private MapData _mapData;
        private List<Award> _awards = new List<Award>();
        private List<CustomerLoanRequestProposal> _proposals = new List<CustomerLoanRequestProposal>();
        private List<Dictionary<string, CustomerAction>> _monthlyActions = new List<Dictionary<string, CustomerAction>>();
        private readonly string _considitionApiKey;
        private string _lastResponseFromServer = "";

        public InMemoryStorage(string mapPath, string personalitiesPath, string awardsPath, string considitionApiKey)
        {
            var mapData = JsonDeserializer.LoadMapData(mapPath, JsonDeserializer.LoadPersonalities(personalitiesPath));
            var awards = JsonDeserializer.LoadAwards(awardsPath);

            if (mapPath is null)
            {
                throw new Exception("Map data could not be parsed!");
            }

            if (awards.Count <= 0)
            {
                throw new Exception("Awards data could not be parsed!");
            }

            _mapData = mapData;
            _awards = awards;
            _considitionApiKey = considitionApiKey;
        }

        public void SetLastResponseFromServer(string lastResponseFromServer)
        {
            _lastResponseFromServer = lastResponseFromServer;
        }

        public string GetLastResponseFromServer()
        {
            return _lastResponseFromServer;
        }

        public string GetConsidtionApiKey()
        {
            return _considitionApiKey;
        }

        public MapData GetMapData()
        {
            return _mapData;
        }

        public List<Customer> GetAllCustomers()
        {
            return _mapData.Customers;
        }

        public List<Award> GetAwards()
        {
            return _awards;
        }

        public void AddLoanProposals(List<CustomerLoanRequestProposal> proposals)
        {
            _proposals.AddRange(proposals);
        }

        public List<CustomerLoanRequestProposal> GetCustomerLoanProposals()
        {
            return _proposals;
        }

        public void AddActionsForOneMonth(Dictionary<string, CustomerAction> actions)
        {
            _monthlyActions.Add(actions);
        }

        public List<Dictionary<string, CustomerAction>> GetAllMonthlyActions()
        {
            return _monthlyActions;
        }
    }
}
