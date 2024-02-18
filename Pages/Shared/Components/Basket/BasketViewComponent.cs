using Bakery.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bakery.Pages.Shared.Components.Basket
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly BakeryContext _context;

        public BasketViewComponent(BakeryContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var basket = new Bakery.Basket();
            if (HttpContext.Request.Cookies.TryGetValue("Basket", out string basketJson))
            {
                basket = JsonSerializer.Deserialize<Bakery.Basket>(basketJson);
            }
            return View(basket); // Pass a Basket object to the view
        }
    }
}
