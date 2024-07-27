using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernelTest.Plugins;
public class LightPlugin
{
    public bool IsOn { get; set; } = false;

#pragma warning disable CA1024 // Use properties where appropriate
    [KernelFunction]
    [Description("Gets the state of the light.")]
    public string GetState() => IsOn ? "on" : "off";
#pragma warning restore CA1024 // Use properties where appropriate

    [KernelFunction]
    [Description("Changes the state of the light.'")]
    public string ChangeState(bool newState)
    {
        IsOn = newState;
        var state = GetState();

        // Print the state to the console
        Console.WriteLine($"[Light is now {state}]");

        return state;
    }
}