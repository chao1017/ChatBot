using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Serialization
{
    /// <summary>
    /// 意圖物件
    /// </summary>
    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }
}