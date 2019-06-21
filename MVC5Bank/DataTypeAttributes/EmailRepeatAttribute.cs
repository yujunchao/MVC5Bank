using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5Bank.Models;

namespace MVC5Bank.DataTypeAttributes
{[AttributeUsage(AttributeTargets.Class)]
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
            
            //var Id = repo客戶聯絡人.Where(x => x.Email == value.GetType.Id).Select(x =>x.Id);
            var typeInfo = value.GetType();
            var propertyInfo = typeInfo.GetProperties();

            if (value == null)
            {
                return true;
            }
            //string Email = (string)value;
            //是否為編輯狀態
            string Email = propertyInfo.Where(p => p.Name == "Email").First().GetValue(value, null).ToString();
            string Id = propertyInfo.Where(p => p.Name == "Id").First().GetValue(value, null).ToString();
            if (Id == "0")
            {
                if (repo客戶聯絡人.All().Where(x => x.Email.Contains(Email)).Count() > 0)
                {
                    return false;
                }
            }
            
           

            return base.IsValid(value);


        }
    }
}