﻿using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using Microsoft.SemanticKernel.Plugins.OpenApi;
using SemanticKernel.Plugins.Plugins.UnitedStatesPlugin;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets("4ef9f3ca-be0f-43ef-9cb0-eac313050d99")
    .Build();

string apiKey = configuration["AzureOpenAI:ApiKey"];
string deploymentName = configuration["AzureOpenAI:DeploymentName"];
string endpoint = configuration["AzureOpenAI:Endpoint"];


var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey)
    .Build();

#pragma warning disable SKEXP0042 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
//const string pluginManifestUrl = "https://semantickernel-unitedstatesdata.azurewebsites.net/api/.well-known/ai-plugin.json";
//await kernel.ImportPluginFromOpenAIAsync("UnitedStatesPlugin", new Uri(pluginManifestUrl));

kernel.ImportPluginFromType<UnitedStatesPlugin>();


var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "MailPlugin");
kernel.ImportPluginFromPromptDirectory(pluginsDirectory, "MailPlugin");

#pragma warning disable SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var planner = new HandlebarsPlanner();

var ask = "Write a mail to share the number of the United States population in 2015 for a research program.";

// Create the plan
var originalPlan = await planner.CreatePlanAsync(kernel, ask);
Console.WriteLine(originalPlan);

// Execute the plan
var originalPlanResult = await originalPlan.InvokeAsync(kernel, []);
Console.WriteLine(originalPlanResult);
#pragma warning restore SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0042 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.