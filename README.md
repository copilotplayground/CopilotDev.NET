# CopilotDev.NET

Unofficial C# developer API for Github Copilot. 

# Terms of Service and Considerations

* This implementation has been written from scratch and does not contain any third party code. The code is fully licenced under MIT by me.
* Calling the endpoints of Github Copilot is governed by the terms of service of Github Copilot. Keep Fair Use in mind.
* The implementation may break at any given time. The called HTTP endpoints **are not** part of a public API.
* This API is not intended to be used for any production cases because it may break any moment and would probably violate the terms of service of Github Copilot.

# Usage

**Nuget.org**
```
Install-Package CopilotDev.NET.Api
```

Take a look into CopilotDev.NET.ConApp/Program.cs. Clone the repository and try to run it.

```cs
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
```

Example 1
```cs
var completions1 = await copilotApi.GetStringCompletionsAsync("public class Em");
Console.WriteLine("Completions for 'public class Em':");
Console.WriteLine(string.Join("",completions1));
Console.WriteLine("---");
  ```          

Example 2
```cs
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
```

