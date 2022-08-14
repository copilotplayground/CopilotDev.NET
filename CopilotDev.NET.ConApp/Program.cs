using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CopilotDev.NET.Api;
using CopilotDev.NET.Api.Entity;
using CopilotDev.NET.Api.Impl;

namespace CopilotDev.NET.ConApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build Api in your main class
            var httpClient = new HttpClient();
            var copilotConfiguration = new CopilotConfiguration();
            // Should be where your application stores files. You can also implement CopilotDev.NET.Api.Contract.IDataStore instead to store it in your own storage (e.g. database).
            var dataStore = new FileDataStore(".\\mytokens.json"); 
            var copilotAuthentication = new CopilotAuthentication(copilotConfiguration, dataStore, httpClient);
            copilotAuthentication.OnEnterDeviceCode += data =>
            {
                // You may open a browser window here instead.
                Console.WriteLine($"Open URL {data.Url} to enter the device code: {data.UserCode}");
            };

            // Use this ICopilotApi instance in your application e.g. Dependency Injection of ICopilotApi
            ICopilotApi copilotApi = new CopilotApi(copilotConfiguration, copilotAuthentication, httpClient);

            // Sample Usage 1
            var completions1 = await copilotApi.GetStringCompletionsAsync("public class Em");
            Console.WriteLine("Completions for 'public class Em':");
            Console.WriteLine(string.Join("",completions1));
            Console.WriteLine("---");

            // Sample Usage 2
            var completions2 = await copilotApi.GetCompletionsAsync(new CopilotParameters
            {
                // Q: How do I get specific programming language suggestions? A: Unknown, however you can always add more context to your prompt like 'in Java'.
                Prompt = "/**  " +
                         "Returns the next 5 Fibonacci numbers in Java. " +
                         "@return Array of integer." +
                         "**/",
                MaxTokens = 200
            });
            Console.WriteLine("Completions for a method description:");
            Console.WriteLine(string.Join("", completions2.Select(e => e.Choices[0].Text)));

            Console.ReadKey();
        }
    }
}
