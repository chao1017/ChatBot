namespace SendAttachmentBot
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    internal class SendAttachmentDialog : IDialog<object>
    {
        private const string ShowInlineAttachment = "(1) Show inline attachment";
        private const string ShowUploadedAttachment = "(2) Show uploaded attachment";
        private const string ShowInternetAttachment = "(3) Show Internet attachment";

        private readonly IDictionary<string, string> options = new Dictionary<string, string>
        {
            { "1", ShowInlineAttachment },
            { "2", ShowUploadedAttachment },
            { "3", ShowInternetAttachment }
        };

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var welcomeMessage = context.MakeMessage();
            welcomeMessage.Text = "Welcome, here you can see attachment alternatives:";

            await context.PostAsync(welcomeMessage);

            await this.DisplayOptionsAsync(context);
        }

        public async Task DisplayOptionsAsync(IDialogContext context)
        {
            PromptDialog.Choice<string>(
                context,
                this.ProcessSelectedOptionAsync,
                this.options.Keys,
                "What sample option would you like to see?",
                "Ooops, what you wrote is not a valid option, please try again",
                3,
                PromptStyle.PerLine,
                this.options.Values);
        }

        public async Task ProcessSelectedOptionAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var message = await argument;

            var replyMessage = context.MakeMessage();

            Attachment attachment = null;

            switch (message)
            {
                case "1":
                    attachment = GetInlineAttachment();
                    break;
                case "2":
                    attachment = await GetUploadedAttachmentAsync(replyMessage.ServiceUrl, replyMessage.Conversation.Id);
                    break;
                case "3":
                    attachment = GetInternetAttachment();
                    break;
            }

            // The Attachments property allows you to send and receive images and other content
            replyMessage.Attachments = new List<Attachment> { attachment };

            await context.PostAsync(replyMessage);

            await this.DisplayOptionsAsync(context);
        }

        private static Attachment GetInlineAttachment()
        {
            //9.1
        }

        private static async Task<Attachment> GetUploadedAttachmentAsync(string serviceUrl, string conversationId)
        {
            //9.2
        }

        private static Attachment GetInternetAttachment()
        {
            //9.3
        }
    }
}
