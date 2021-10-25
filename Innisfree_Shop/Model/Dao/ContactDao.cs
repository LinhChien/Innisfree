using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        InnisfreeShopDbContext db = null;
        public ContactDao()
        {
            db = new InnisfreeShopDbContext();
        }

        public Contact GetActiveContact()
        {
            return db.Contacts.SingleOrDefault(x => x.Status == true);
        }

        public long InsertFeedBack(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
    }
}