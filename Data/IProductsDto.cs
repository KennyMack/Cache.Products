using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache.Products.Data
{
    public interface IProductsDto
    {
        public Models.Products GetById(Guid pId);
        public IEnumerable<Models.Products> GetAll();
        public Models.Products New(Models.Products pProduct);
        public Models.Products Update(Models.Products pProduct);
        public Models.Products RemoveById(Guid pId);
    }
}
