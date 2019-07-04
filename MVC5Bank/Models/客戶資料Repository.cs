using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
	
namespace MVC5Bank.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override void Delete(客戶資料 entity)
        {
            entity.Stat = false;
        }
        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id && p.Stat!= false);
        }
        public IQueryable<客戶資料> Classification(String Keyword)
        {
            return this.All().Where(p => p.客戶分類 == Keyword && p.Stat != false);
        }
        public IQueryable<客戶資料> NamenClassification(String Keyword,string name)
        {
            return this.All().Where(p => p.客戶分類 == Keyword && p.Stat != false && p.客戶名稱 == name);
        }
        public IQueryable<客戶資料> Order(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return this.All().OrderBy(keyword);
            }
            return this.All();
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}