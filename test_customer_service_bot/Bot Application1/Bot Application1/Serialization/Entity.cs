using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Serialization
{
    /// <summary>
    /// 關鍵字物件
    /// </summary>
    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public double score { get; set; }
    }
}