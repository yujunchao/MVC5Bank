using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Bank.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override void Delete(客戶銀行資訊 entity)
        {
            entity.Stat = false;
        }
        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}