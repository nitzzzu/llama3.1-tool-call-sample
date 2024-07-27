using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelTest.Plugins;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    // Add OpenTelemetry as a logging provider
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Warning);
});

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var builder = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion("llama3.1", new Uri("http://127.0.0.1:11434"), "apikey");
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder.Plugins.AddFromType<LightPlugin>();
builder.Plugins.AddFromType<MailPlugin>();
builder.Plugins.AddFromType<TicketPlugin>();
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));

Kernel kernel = builder.Build();

var history = new ChatHistory();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

Console.Write("User > ");
string? userInput;
while ((userInput = Console.ReadLine()) != null)
{
    history.AddUserMessage(userInput);

    OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    Console.WriteLine("Assistant > " + result);

    history.AddMessage(result.Role, result.Content ?? string.Empty);

    Console.Write("User > ");
}
