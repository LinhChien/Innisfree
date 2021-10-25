using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.Dao
{
    public class ProductDao
    {
        InnisfreeShopDbContext db = null;
        public ProductDao()
        {
            db = new InnisfreeShopDbContext();
        }
        public long Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.Image = entity.Image;
                product.Price = entity.Price;
                product.ModifiedDate = DateTime.Now;
                product.CategoryID = entity.CategoryID;
                product.TopHot = entity.TopHot;
                product.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Code.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Product GetById(string name)
        {
            return db.Products.SingleOrDefault(x => x.Name == name);
        }
        public Product ViewDetail(int id)
        {
            return db.Products.Find(id);
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.Name).ToList();
        }


        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int page = 1, int pageSize = 1)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = (from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.CategoryID == categoryID
                        select new 
                        {
                            CateMetaTitle = b.MetaTitle,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Images = a.Image,
                            Name = a.Name,
                            MetaTitle = a.MetaTitle,
                            Price = a.Price
                        }).AsEnumerable().Select(x => new ProductViewModel()
                        {
                            CateMetaTitle = x.MetaTitle,
                            CateName = x.Name,
                            CreatedDate = x.CreatedDate,
                            ID = x.ID,
                            Images = x.Images,
                            Name = x.Name,
                            MetaTitle = x.MetaTitle,
                            Price = x.Price
                        });
            model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<Product> ListRelatedProduct(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int page = 1, int pageSize = 1)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = (from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.Name.Contains(keyword)
                        select new 
                        {
                            CateMetaTitle = b.MetaTitle,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Images = a.Image,
                            Name = a.Name,
                            MetaTitle = a.MetaTitle,
                            Price = a.Price
                        }).AsEnumerable().Select(x=>new ProductViewModel() {
                            CateMetaTitle = x.MetaTitle,
                            CateName = x.Name,
                            CreatedDate = x.CreatedDate,
                            ID = x.ID,
                            Images = x.Images,
                            Name = x.Name,
                            MetaTitle = x.MetaTitle,
                            Price = x.Price
                        })  ;
            model.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }

    }
}
