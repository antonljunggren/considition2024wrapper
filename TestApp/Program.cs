using Azure;
using Core;
using Core.Utils;

namespace TestApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Considition 2024 Wrapper!");

#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0001

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string mapPath = Path.Combine(baseDirectory, "map.json");
            string awardsPath = Path.Combine(baseDirectory, "awards.json");
            string personalitiesPath = Path.Combine(baseDirectory, "personalities.json");

            InMemoryStorage inMemoryStorage = new InMemoryStorage(
                mapPath, 
                personalitiesPath, 
                awardsPath, "");

            AIChatService aIChatService = new AIChatService("gpt-4o", inMemoryStorage,
                "",
                mapPath, awardsPath, personalitiesPath);

            var input = "";

            /**
             * You are a bank and in this game your task is to approve or decline customer loan proposals and make monthly decisions on what action to take with the customers. You need to consider the bank's capital, the game months. Remember that the bank should make a profit and that a loan cannot be longer than the game months. Remember when deciding on what actions to take that they impact the customer happiness but also at a cost. Every month does not need to have a concrete action made, some can be skipped. Start with deciding who gets their loan approved and wait for further instructions.
                Good now move on to deciding what action to take with each customer that got their loan approved. Do this for month number 1. Remember that the customer action consists of a CustomerName, ActionType and AwardType.
                Good now continue with new actions for month number 2
             * 
             */

            string initPrompt = "You are a bank and in this game your task is to approve or decline customer loan proposals and make monthly decisions on what action to take with the customers. You need to consider the bank's capital, the game months. Remember that the bank should make a profit and that a loan cannot be longer than the game months. Remember when deciding on what actions to take that they impact the customer happiness but also at a cost. Every month does not need to have a concrete action made, some can be skipped. Wait for further instructions.";

            var initResp = await aIChatService.Chat(
                initPrompt, 
                true);

            Console.WriteLine("Init prompt");
            Console.WriteLine(initResp);

            var initActionResp = await aIChatService.Chat(
                "Start with deciding who gets their loan approved and wait for further instructions.",
                true);

            Console.WriteLine("Ask for loan approvals:");
            Console.WriteLine(initResp);

            var firstActionResp = await aIChatService.Chat(
                "Good now move on to deciding what action to take with each customer that got their loan approved. Do this for month number 1. Remember that the customer action consists of a CustomerName, ActionType and AwardType. Call make_month_actions",
                true);

            aIChatService.ClearHistory();

            Console.WriteLine("Ask First month:");
            Console.WriteLine(firstActionResp);

            for (int i = 2; i <= inMemoryStorage.GetMapData().GameLengthInMonths; i++)
            {
                /*var resp = await aIChatService.Chat(
                $"Good now decide what action to take with each customer that got their loan approved. Do this for month number {i}. Remember that the customer action consists of a CustomerName, ActionType and AwardType.",
                true);*/
                string resp;

                if(i % 5 == 0 && i >= 10)
                {
                    aIChatService.ClearHistory();
                    var initRespLoop = await aIChatService.Chat(
                        initPrompt,
                        true);

                    Console.WriteLine("Clear, new init prompt:");
                    Console.WriteLine(initRespLoop);

                    resp = await aIChatService.Chat(
                        $"Good now decide what action to take with each customer that got their loan approved. Do this for month number {i}. Remember that the customer action consists of a CustomerName, ActionType and AwardType.",
                        true);
                }
                else
                {
                    resp = await aIChatService.Chat(
                        $"Good now continue with new actions for month number {i} calling make_month_actions",
                        true);
                }

                Console.WriteLine();
                Console.WriteLine(resp);
            }

            while (true)
            {
                Console.WriteLine();
                Console.Write("Write: ");
                input = Console.ReadLine() ?? "";

                if (input.Equals("/bye"))
                {
                    break;
                }

                var aiResponse = await aIChatService.Chat(input, true);

                Console.WriteLine();
                Console.WriteLine(aiResponse);
            }
        }
    }
}
