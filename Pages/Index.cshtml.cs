using Bakery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BakeryContext _context;

    public IndexModel(ILogger<IndexModel> logger, BakeryContext context)
    {
        _logger = logger;
        _context = context;
    }
    public List<Product> Products { get; set; } = new();
    public async Task OnGetAsync()
    {
        Products = await _context.Products.ToListAsync();
    }
}
