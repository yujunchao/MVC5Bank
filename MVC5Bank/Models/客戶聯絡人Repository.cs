using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Bank.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override void Delete(客戶聯絡人 entity)
        {
            entity.Stat = false;
        }
        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}