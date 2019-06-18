using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5Bank.Models;

namespace MVC5Bank.DataTypeAttributes
{
    public class EmailRepeatVerificationAttribute : DataTypeAttribute
    {
        客戶聯絡人Repository repo客戶聯絡人;
        
        public string Email { get; set; }

        public EmailRepeatVerificationAttribute() : base(DataType.Text)
        {
            repo客戶聯絡人 = RepositoryHelper.Get客戶聯絡人Repository();
            ErrorMessage = "E-Mail重覆";
        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            string Email = (string)value;
            if (repo客戶聯絡人.All().Where(x => x.Email.Contains(Email)).Count()>0)
            {
                return false;
            }
           

            return base.IsValid(value);


        }
    }
}