using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
//using System.Collections.Generic;

namespace Bot_Application1.Serialization
{
    /// <summary>
    /// 表示整個Luis傳回Json物件
    /// </summary>
    [DataContract]
    public class Utterance
    {
        [DataMember]
        public string query { get; set; }
        [DataMember]
        public List<Intent> intents { get; set; }
        [DataMember]
        public List<Entity> entities { get; set; }
    }
}