using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.EF;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SlideDao
    {
        InnisfreeShopDbContext db = null;
        public SlideDao()
        {
            db = new InnisfreeShopDbContext();
        }
        public long Insert(Slide entity)
        {
            db.Slides.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
    }
}