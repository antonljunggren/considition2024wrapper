using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [Description("Map data of a scenario")]
    public sealed class MapData
    {
        [Description("The map name")]
        [JsonProperty("name")]
        public string Name { get; private set; }

        [Description("The bank's budget")]
        [JsonProperty("budget")]
        public int Budget { get; private set; }

        [Description("The games length in months")]
        [JsonProperty("game_length_in_months")]
        public int GameLengthInMonths { get; private set; }

        [Description("The map's available customers who wants a loan'")]
        [JsonProperty("customers")]
        public List<Customer> Customers { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private MapData() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public MapData(string name, int budget, int gameLengthInMonths, List<Customer> customers)
        {
            Name = name;
            Budget = budget;
            GameLengthInMonths = gameLengthInMonths;
            Customers = customers;
        }
    }
}
