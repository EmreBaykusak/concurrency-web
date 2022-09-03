using Concurreny.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concurreny.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext context;

        public ProductsController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> List()
        {
            return View(await context.Products.ToListAsync());
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await context.Products.FindAsync(id);
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            
            try
            {
                context.Products.Update(product);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(List));
            }
            catch (DbUpdateConcurrencyException exception)
            {

                var exceptionEntry = exception.Entries.First();

                var currentProduct = exceptionEntry.Entity as Product;

                var databaseValues = exceptionEntry.GetDatabaseValues();

                Product? databaseProduct = databaseValues.ToObject() as Product;

                var clientValues = exceptionEntry.CurrentValues;

                if(databaseValues == null)
                {
                    ModelState.AddModelError(string.Empty, "Bu ürün başka bir kullanıcı tarafından silindi");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bu ürün başka bir kullanıcı tarafından güncellendi");
                    ModelState.AddModelError(string.Empty, errorMessage: $"{databaseProduct.Name}, {databaseProduct.Price}, {databaseProduct.Stock}");
                }

                return View(product);
            }
        }
    }
}
