using Core.Models;
using Core.Utils;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Models.Award;

namespace Core.Services
{
    public sealed class GameService
    {
        private readonly InMemoryStorage _storage;

        public GameService(InMemoryStorage storage)
        {
            _storage = storage;
        }

        [KernelFunction("submit_game_data")]
        [Description("Submit the game data to the server")]
        [return: Description("The response from the server telling us how our game went, score wise")]
        public async Task<string> SubmitGameData()
        {
            var monthNotAdded = _storage.GetMapData().GameLengthInMonths -  _storage.GetAllMonthlyActions().Count();

            var skipMonth = new Dictionary<string, CustomerAction>();

            foreach (var loanProposals in _storage.GetCustomerLoanProposals())
            {
                skipMonth.Add(loanProposals.CustomerName, new CustomerAction(CustomerAction.CustomerActionType.Skip, AwardType.None));
            }

            while (monthNotAdded > 0)
            {
                _storage.AddActionsForOneMonth(skipMonth);
            }

            GameInput gameInput = new GameInput()
            {
                Iterations = _storage.GetAllMonthlyActions(),
                Proposals = _storage.GetCustomerLoanProposals(),
                MapName = _storage.GetMapData().Name
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(gameInput));
            content.Headers.Add("x-api-key", _storage.GetConsidtionApiKey());
            var response = await httpClient.PostAsync("https://api.considition.com/game", content);

            response.EnsureSuccessStatusCode();

            var serverResponse = await response.Content.ReadAsStringAsync();
            _storage.SetLastResponseFromServer(serverResponse);
            return serverResponse;
        }

        [KernelFunction("make_month_actions")]
        [Description("Make a decision on what to do with each customer for one month at a time")]
        public void MakeMonthlyDecisions(
            [Description("A list of customer actions inputs for each customer in the shape of CustomerActionInput object")]
            string actionsJson,
            [Description("The month's number")]
            int monthNumber)
        {
            var actions = JsonConvert.DeserializeObject<List<CustomerActionInput>>(actionsJson);

            if(actions is null || actions.Count <= 0)
            {
                var skipMonth = new Dictionary<string, CustomerAction>();

                foreach(var loanProposals in _storage.GetCustomerLoanProposals())
                {
                    skipMonth.Add(loanProposals.CustomerName, new CustomerAction(CustomerAction.CustomerActionType.Skip, AwardType.None));
                }

                _storage.AddActionsForOneMonth(skipMonth);
            }
            else
            {
                _storage.AddActionsForOneMonth(actions.ToDictionary(a => a.CustomerName, a => new CustomerAction(a.ActionType, a.AwardType ?? AwardType.None)));
            }
        }

        [KernelFunction("get_map_data")]
        [Description("Gets the map data containing information about the scenario, like game months, the bank's capital")]
        [return: Description("The map data")]
        public MapData GetMapData()
        {
            return _storage.GetMapData();
        }

        [KernelFunction("get_available_awards")]
        [Description("Gets available awards to give to a customer as part of a customer action")]
        [return: Description("All available awards")]
        public List<Award> GetAwards()
        {
            return _storage.GetAwards();
        }

        [KernelFunction("get_available_customer_action_types")]
        [Description("Gets available customer action types")]
        [return: Description("All customer action types")]
        public List<string> GetCustomerActionTypes()
        {
            return new() { CustomerAction.CustomerActionType.Skip.ToString(), CustomerAction.CustomerActionType.Punish.ToString(), CustomerAction.CustomerActionType.Award.ToString() };
        }
    }
}
