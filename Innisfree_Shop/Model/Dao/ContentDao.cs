using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using PagedList;
using System.Web;

//27'30
namespace Model.Dao
{
    public class ContentDao
    {
        InnisfreeShopDbContext db = null;
        public static string USER_SESSION = " USER_SESSION";
        public ContentDao()
        {
            db = new InnisfreeShopDbContext();
        }
        public bool Update(Content entity)
        {
            try
            {
                var content = db.Contents.Find(entity.ID);
                content.Name = entity.Name;
                content.Description = entity.Description;
                content.CategoryID = entity.CategoryID;
                content.Image = entity.Image;
                content.Detail = entity.Detail;
                content.ModifiedDate = DateTime.Now;
                content.Tags = entity.Tags;
                content.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

        }
        // Lấy ra tất cả các content để up lên client
        public IEnumerable<Content> ListAllPaging(int page, int pageSize)
        {
            IQueryable<Content> model = db.Contents;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

        }
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.ID equals b.ContentID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Image,
                             Description = a.Description,
                             CreatedDate = a.CreatedDate,
                             CreatedBy = a.CreatedBy,
                             ID = a.ID

                         }).AsEnumerable().Select(x => new Content()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Image = x.Image,
                             Description = x.Description,
                             CreatedDate = x.CreatedDate,
                             CreatedBy = x.CreatedBy,
                             ID = x.ID
                         });
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Content GetByID(long id)
        {
            return db.Contents.Find(id);
        }
        public Content ViewDetail(int id)
        {
            return db.Contents.Find(id);
        }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
        //Tạo mới content
        public long Create(Content content)
        {
            // đổi tên content từ có dấu sang k dấu
            if (!string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            content.ViewCount = 0;
            db.Contents.Add(content);
            db.SaveChanges();
            // Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existerTag = this.CheckTag(tagId);
                    // insert vào bảng tag trong CSDL
                    if (!existerTag)
                    {
                        this.InsertTag(tagId, tag);
                    }
                    //insert vảo bảng Content Tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }
            return content.ID;
        }
        //Sửa Content
        public long Edit(Content content)
        {
            // đổi tên content từ có dấu sang k dấu
            if (!string.IsNullOrEmpty(content.MetaTitle))
            {
                content.MetaTitle = StringHelper.ToUnsignString(content.Name);
            }
            content.CreatedDate = DateTime.Now;
            db.SaveChanges();
            // Xử lý tag
            if (!string.IsNullOrEmpty(content.Tags))
            {
                this.RemoveAllContent(content.ID);
                string[] tags = content.Tags.Split(',');
                foreach (var tag in tags)
                {
                    var tagId = StringHelper.ToUnsignString(tag);
                    var existerTag = this.CheckTag(tagId);
                    // insert vào bảng tag trong CSDL
                    if (!existerTag)
                    {
                        this.InsertTag(tagId, tag);
                    }
                    //insert vảo bảng Content Tag
                    this.InsertContentTag(content.ID, tagId);
                }
            }
            return content.ID;
        }
        public void RemoveAllContent(long contentId)
        {
            db.ContentTags.RemoveRange(db.ContentTags.Where(x => x.ContentID == contentId));
            db.SaveChanges();
        }
        public void InsertTag(string id, string name)
        {
            var tag = new Tag();
            tag.ID = id;
            tag.Name = name;
            db.Tags.Add(tag);
            db.SaveChanges();
        }
        public void InsertContentTag(long contentId, string tagId)
        {
            var contentTag = new ContentTag();
            contentTag.ContentID = contentId;
            contentTag.TagID = tagId;
            db.ContentTags.Add(contentTag);
            db.SaveChanges();
        }
        // KTra tag đã tồn tại chưa
        public bool CheckTag(string id)
        {
            return db.Tags.Count(x => x.ID == id) > 0;
        }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.ID
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var content = db.Contents.Find(id);
                db.Contents.Remove(content);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
