using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly BulkDbContext _db;
        public CompanyRepository(BulkDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            _db.Update(obj);
        }
    }
}
