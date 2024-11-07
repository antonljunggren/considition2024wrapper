using Core.Services;
using Core.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public sealed class AIChatService
    {
        private readonly Kernel _kernel;
        private readonly HttpClient _httpClient;
        private readonly IChatCompletionService _chatCompletionService;
        public OpenAIPromptExecutionSettings OpenAIPromptExecutionSettings;
        public ChatHistory ChatHistory;

#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0001
        public AIChatService(string modelId, InMemoryStorage inMemoryStorage, string? openAIApiKey, string? mapPathUri, string? awardsPathUri, string? personalitiesPathUri)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string mapPath = mapPathUri ?? Path.Combine(baseDirectory, "map.json");
            string awardsPath = awardsPathUri ?? Path.Combine(baseDirectory, "awards.json");
            string personalitiesPath = personalitiesPathUri ?? Path.Combine(baseDirectory, "personalities.json");

            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(10),
            };

            var kernelBuilder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: modelId,
                    apiKey: openAIApiKey ?? "",
                    httpClient: _httpClient);

            kernelBuilder.Services.AddSingleton<InMemoryStorage>(provider => inMemoryStorage);

            kernelBuilder.Plugins.AddFromType<CustomerService>("customers");
            kernelBuilder.Plugins.AddFromType<LoanService>("loans");
            kernelBuilder.Plugins.AddFromType<GameService>("game");

            _kernel = kernelBuilder.Build();

            OpenAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                //Temperature = 0.1,
            };

            _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory = new ChatHistory();
        }

        public void ClearHistory()
        {
            ChatHistory.Clear();
        }

        public async Task<string> Chat(string message, bool showTerminalDotProgress = false)
        {
            using var cancellationToken = new CancellationTokenSource();
            Task? printDotsTask = null;

            if (showTerminalDotProgress)
            {
                printDotsTask = PrintMinuteDot(cancellationToken.Token);
            }

            ChatHistory.AddUserMessage(message);

            try
            {
                var response = await _chatCompletionService.GetChatMessageContentAsync(ChatHistory, OpenAIPromptExecutionSettings, _kernel);

                if (printDotsTask is not null)
                {
                    cancellationToken.Cancel();
                    await printDotsTask;
                }

                return response.Content ?? "NO AI RESPONSE!!!!";
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static async Task PrintMinuteDot(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.Write(".");
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
