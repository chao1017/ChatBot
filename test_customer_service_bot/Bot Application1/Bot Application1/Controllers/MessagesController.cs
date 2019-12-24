using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Bot_Application1.Services;
using Bot_Application1.Serialization;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity message)
        {
            if (message.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                // calculate something for us to return
                int length = (message.Text ?? string.Empty).Length;

                var resposta = await Response(message); //跟LUIS取得辩試結果，並準備回覆訊息

                if (resposta != null)
                {
                    var msg = message.CreateReply(resposta, "zh-TW");
                    await connector.Conversations.ReplyToActivityAsync(msg); //回傳訊息
                }

                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted); //回傳狀態
            }
            else
            {
                HandleSystemMessage(message);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;            
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private static async Task<string> Response(Activity message)
        {
            Activity resposta = null; //回覆給客戶訊息
            if (message != null)
            {
                switch (message.GetActivityType())
                {
                    case ActivityTypes.Message:
                        resposta = await GetLuisResponse(message);
                        break;
                    case ActivityTypes.ConversationUpdate:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    default:
                        //Trace.TraceError($"Unknown activity type ignored: {message.GetActivityType()}");
                        break;

                }
            }

            if (resposta != null)
                return resposta.Text;
            else
                return null;
        }

        private static async Task<Activity> GetLuisResponse(Activity message)
        {
            Activity resposta = new Activity();
            var luisResponse = await Luis.GetResponse(message.Text); //Call LUIS Service
            if (luisResponse != null)
            {
                var intent = new Intent();
                var entity = new Serialization.Entity();

                string acao = string.Empty;
                string telcomm = string.Empty; //電信公司
                string phone = string.Empty; //手機種類

                string entityType = string.Empty;
                int replaceStartPos = 0;
                foreach (var item in luisResponse.entities)
                {
                    entityType = item.type;
                    replaceStartPos = entityType.IndexOf("::");
                    if (replaceStartPos > 0)
                        entityType = entityType.Substring(0, replaceStartPos);
                    switch (entityType)
                    {
                        case "電信公司":
                            telcomm = item.entity;
                            break;
                        case "手機類型":
                            phone = item.entity;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(telcomm))
                {
                    if (!string.IsNullOrEmpty(phone))
                        resposta = message.CreateReply($"好的! 這裡是您的 {telcomm}  {phone}方案");
                    else
                        resposta = message.CreateReply("你要 " + telcomm + "的方案，也請提供手機型號.");
                }
                else
                    resposta = message.CreateReply("對不起不了解您問的，請輸入> [電信公司](ex:中華電信)的[手機型號](ex: iphone7)資費方案.");
            }
            return resposta;
        }
    }
}