namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class SupportDialog : IDialog<int>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //3.1
            await context.PostAsync($"How can I help you?");
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //3.2
            var message = await result;

            var ticketNumber = new Random().Next(0, 20000);

            await context.PostAsync($"Your message '{message.Text}' was registered. Once we resolve it; we will get back to you.");

            context.Done(ticketNumber);
        }
    }
}
