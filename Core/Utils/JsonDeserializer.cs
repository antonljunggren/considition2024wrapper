using Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Models.Award;

namespace Core.Utils
{
    public static class JsonDeserializer
    {
        /// <summary>
        /// Loads personalities from the given JSON file into a dictionary.
        /// </summary>
        public static Dictionary<string, Personality> LoadPersonalities(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var jsonObj = JObject.Parse(json);
            var personalitiesDict = new Dictionary<string, Personality>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in jsonObj["Personalities"]!.Children<JProperty>())
            {
                var key = prop.Name; // e.g., "conservative"
                var value = prop.Value;

                // Extract properties
                float happinessMultiplier = value["happinessMultiplier"]!.Value<float>();
                float acceptedMinInterest = value["acceptedMinInterest"]!.Value<float>();
                float acceptedMaxInterest = value["acceptedMaxInterest"]!.Value<float>();
                float livingStandardMultiplier = value["livingStandardMultiplier"]!.Value<float>();

                // Create Personality instance with the key as the Name
                var personality = new Personality(
                    name: key,
                    happinessMultiplier: happinessMultiplier,
                    acceptedMinInterest: acceptedMinInterest,
                    acceptedMaxInterest: acceptedMaxInterest,
                    livingStandardMultiplier: livingStandardMultiplier
                );

                personalitiesDict.Add(key, personality);
            }

            return personalitiesDict;
        }

        /// <summary>
        /// Loads awards from the given JSON file into a list.
        /// </summary>
        public static List<Award> LoadAwards(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var jsonObj = JObject.Parse(json);
            var awardsList = new List<Award>();

            foreach (var prop in jsonObj["Awards"]!.Children<JProperty>())
            {
                if (Enum.TryParse<AwardType>(prop.Name, ignoreCase: true, out var awardType))
                {
                    var cost = prop.Value["cost"]!.Value<float>();
                    var baseHappiness = prop.Value["baseHappiness"]!.Value<int>();
                    var award = new Award(awardType, cost, baseHappiness);
                    awardsList.Add(award);
                }
                else
                {
                    throw new Exception($"Unknown AwardType: {prop.Name}");
                }
            }

            return awardsList;
        }

        /// <summary>
        /// Loads map data from the given JSON file, resolving personalities using the provided dictionary.
        /// </summary>
        public static MapData LoadMapData(string filePath, Dictionary<string, Personality> personalities)
        {
            var json = File.ReadAllText(filePath);
            var jsonObj = JObject.Parse(json);

            string name = jsonObj["name"]!.Value<string>()!;
            int budget = jsonObj["budget"]!.Value<int>();
            int gameLengthInMonths = jsonObj["gameLengthInMonths"]!.Value<int>();

            var customers = new List<Customer>();

            foreach (var customerToken in jsonObj["customers"]!)
            {
                string customerName = customerToken["name"]!.Value<string>()!;

                // Deserialize Loan
                var loan = customerToken["loan"]!.ToObject<Loan>();

                // Get Personality
                string personalityName = customerToken["personality"]!.Value<string>()!;
                if (!personalities.TryGetValue(personalityName, out var personality))
                {
                    throw new Exception($"Personality '{personalityName}' not found.");
                }

                decimal capital = customerToken["capital"]!.Value<decimal>();
                decimal income = customerToken["income"]!.Value<decimal>();
                decimal monthlyExpenses = customerToken["monthlyExpenses"]!.Value<decimal>();
                int numberOfKids = customerToken["numberOfKids"]!.Value<int>();
                decimal homeMortgage = customerToken["homeMortgage"]!.Value<decimal>();
                bool hasStudentLoan = customerToken["hasStudentLoan"]!.Value<bool>();

                var customer = new Customer(
                    customerName!,
                    loan!,
                    personality,
                    capital,
                    income,
                    monthlyExpenses,
                    numberOfKids,
                    homeMortgage,
                    hasStudentLoan
                );

                customers.Add(customer);
            }

            return new MapData(name!, budget, gameLengthInMonths, customers);
        }
    }
}
