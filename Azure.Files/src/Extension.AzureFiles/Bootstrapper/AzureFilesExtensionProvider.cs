namespace Azure.Functions.Extension.AzureFiles
{
    using System;
    using Microsoft.Azure.WebJobs.Host.Config;

    public class AzureFilesExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var fileRule = context.AddBindingRule<AzureFileAttribute>();
            fileRule.BindToInput(new AzureFileManager());
        }
    }
}
