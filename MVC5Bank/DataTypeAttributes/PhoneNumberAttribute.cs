using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC5Bank.DataTypeAttributes
{
    public class PhoneNumberVerificationAttribue : DataTypeAttribute
    {
        public int PhoneNuber { get; set; }

        public PhoneNumberVerificationAttribue() : base(DataType.Text)
        {
            ErrorMessage = "手機號碼必須符合前四碼-後六碼格式";
        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            string phonenumber = (string)value;

            string pattern = @"(\d{4}-\d{6})";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);



            if (!regex.IsMatch(phonenumber))
            {
                return false;
                // doSomething
            }

            return base.IsValid(value);


        }
    }
}