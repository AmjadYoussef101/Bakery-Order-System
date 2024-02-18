using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Bakery;
using Bakery.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bakery.Pages_Products
{
    public class CreateModel : PageModel
    {
        private readonly BakeryContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(BakeryContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        
        [BindProperty, Display(Name = "Product Image")]
        public IFormFile ProductImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Product.ImageName");

            if (!ModelState.IsValid || _context.Products == null || Product == null || ProductImage == null)
            {
                return Page();
            }

            Product.ImageName = ProductImage.FileName;

            // Construct the file path
            var imageDirectory = Path.Combine(_environment.WebRootPath, "images", "products");
            var imageFile = Path.Combine(imageDirectory, ProductImage.FileName);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            // Save the image file
            using (var fileStream = new FileStream(imageFile, FileMode.Create))
            {
                await ProductImage.CopyToAsync(fileStream);
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
