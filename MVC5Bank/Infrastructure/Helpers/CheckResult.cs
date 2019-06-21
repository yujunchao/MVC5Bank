using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogSample.Infrastructure.Helpers
{
    public class CheckResult
    {
        public Guid ID { get; set; }

        public bool Success { get; set; }

        public int RowCount { get; set; }

        public int ErrorCount { get; set; }

        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string 客戶名稱 { get; set; }
        public string 統一編號 { get; set; }
        public string 電話 { get; set; }
        public string 傳真 { get; set; }
        public string 地址 { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Stat { get; set; }
        public string 客戶分類 { get; set; }
        public string 帳號 { get; set; }
        public string 密碼 { get; set; }

    }
}