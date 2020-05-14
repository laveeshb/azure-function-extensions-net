# Amazon Simple Queue Service (SQS) extension

- [NuGet Package](https://www.nuget.org/packages/AzureFunctions.Extension.SQS)
- [Release Notes](https://github.com/laveeshb/azure-function-extensions-net/releases)

This extension helps developers trigger Azure Functions based on [Amazon SQS queues](https://aws.amazon.com/sqs/).

This extension provides the following bindings:

| Binding   | Description | Example |
|------------|------------------|-|
| Trigger on message      | Trigger an Azure function based on messages in an AWS SQS queue | [Trigger on message](samples/Extensions.SQS.Sample.v2/Trigger/QueueMessageTrigger.cs) |
| Push message to queue      | Push a message to an AWS SQS queue from your Azure function | [Push message](samples/Extensions.SQS.Sample.v3/Output/QueueMessageOutput.cs) |

The bindings support the following authentication methods:
* Basic AWS credentials
