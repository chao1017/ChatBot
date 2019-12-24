using Bot_Application1.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Services
{
    public class Luis
    {
        public static async Task<Utterance> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                //LUIS 的Programmantic API ID
                const string subscriptionkey = "9ea803c39a89459f9386c53292504226";

                //C# 6 called Interpolated Strings, {變數名稱} = > {authKey} {message}
                var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/0b13869c-07bc-410a-a4b3-c0fa58d2d43d?verbose=true&timezoneOffset=0&subscription-key={subscriptionkey}&timezoneOffset=0.0&verbose=true&q={message}";

                client.DefaultRequestHeaders.Accept.Clear();
                //Header宣告成Json格式
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url); //呼叫LUIS Service

                if (!response.IsSuccessStatusCode) return null; //失敗直接返回

                var result = await response.Content.ReadAsStringAsync(); //讀取資料

                //將LUIS返回的Json 轉到 Utterance物件
                var js = new DataContractJsonSerializer(typeof(Utterance));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var list = (Utterance)js.ReadObject(ms);

                return list;
            }
        }
    }
}