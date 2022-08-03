using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ApplicationUserRepository:Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly BulkDbContext _db;
        public ApplicationUserRepository(BulkDbContext db):base(db)
        {
            _db = db;
        }
    }
}
