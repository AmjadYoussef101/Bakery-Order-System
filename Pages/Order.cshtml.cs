using System.ComponentModel.DataAnnotations;
using Bakery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Bakery.Pages
{
    public class OrderModel : PageModel
    {
        private readonly ILogger<OrderModel> _logger;
        private readonly BakeryContext _context;

        public OrderModel(ILogger<OrderModel> logger, BakeryContext context)
        {
            _logger = logger;
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Product Product { get; set; }
        [BindProperty, Range(1, int.MaxValue, ErrorMessage = "You must order at least one item")]
        public int Quantity { get; set; } = 1;
        [BindProperty]
        public decimal UnitPrice { get; set; }
        [TempData]
        public string Confirmation { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Products.FindAsync(Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                Basket basket = new();
                if(Request.Cookies[nameof(Basket)] is not null)
                {
                    basket = JsonSerializer.Deserialize<Basket>(Request.Cookies[nameof(Basket)]);
                }
                basket.Items.Add(new OrderItem {
                    ProductId = Id,
                    UnitPrice = UnitPrice,
                    Quantity = Quantity
                });
                var json = JsonSerializer.Serialize(basket);
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                };
                Response.Cookies.Append(nameof(Basket), json, cookieOptions);
                return RedirectToPage("/Checkout");
            }
            Product = await _context.Products.FindAsync(Id);
            return Page();
        }
    }
}
