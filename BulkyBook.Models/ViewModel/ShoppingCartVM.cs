using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModel
{
   
    public class ShoppingCartVM
    {
       
        public IEnumerable<ShoppingCart> ListCart { get; set; }

    }
    

}
