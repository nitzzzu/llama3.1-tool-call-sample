using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernelTest.Plugins
{
    public class MailPlugin
    {
        private const string DESCRIPTION = "Write a professional mail starting from a given text.";
        private const string TEMPLATE = @"﻿Take the following text and turn it into a professional mail. Use a professional tone, be clear and concise.
        Always end up the mail with the message ""Written by an AI Assistant"".

        Text: {{$input}}";
        internal const string FUNCTIONNAME = "WriteProfessionalEmail";

        private readonly KernelFunction _func;

        public MailPlugin()
        {
            PromptExecutionSettings settings = new()
            {
                ExtensionData = new Dictionary<string, object>()
                {
                    { "Temperature", 0 },
                    { "MaxTokens", 1000 }
                }
            };

            _func = KernelFunctionFactory.CreateFromPrompt(TEMPLATE,
                functionName: FUNCTIONNAME,
                description: DESCRIPTION,
                executionSettings: settings);
        }

        [KernelFunction]
        [Description("Write a professional mail starting from a given text.")]
        public async Task<string> WriteProfessionalEmail([Description("The text to turn into a professional mail.")] string input, Kernel kernel)
        {
            var result = await _func.InvokeAsync(kernel, new() { ["input"] = input }).ConfigureAwait(false);

            return result.ToString();
        }
    }
}
