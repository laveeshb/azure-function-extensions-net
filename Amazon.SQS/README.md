# Amazon SQS extension for Azure functions in .NET

- [Homepage](.)
- [NuGet Package](https://www.nuget.org/packages/AzureFunctions.Extension.SQS)
- [Release Notes](https://github.com/laveeshb/azure-function-extensions-net/releases)

This extension helps developers trigger Azure Functions based on [Amazon SQS queues](https://aws.amazon.com/sqs/).

This extension provides the following bindings:

| Binding   | Description | Example |
|------------|------------------|-|
| Trigger on message      | Trigger an Azure function based on messages in an AWS SQS queue | [Trigger on message](samples/Extensions.SQS.Sample.v2/Trigger/QueueMessageTrigger.cs) |

The bindings support the following authentication methods:
* Basic AWS credentials
