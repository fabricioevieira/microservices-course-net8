using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        // VERIFICAR NO GITHUB DO PROJETO ESSE SEED
        return new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Samsung Galaxy S23 Ultra",
                Category = new List<string> { "Mobile", "Electronics" },
                Description = "The Samsung Galaxy S23 Ultra features a 6.8-inch Dynamic AMOLED display, Snapdragon 8 Gen 2 processor, and a versatile camera system for stunning photos and videos.",
                ImageFile = "https://m.media-amazon.com/images/I/61jLiK+q9sL._AC_SX679_.jpg",
                Price = 1199.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Google Pixel 7 Pro",
                Category = new List<string> { "Mobile", "Electronics" },
                Description = "The Google Pixel 7 Pro features a 6.7-inch LTPO OLED display, Google Tensor G2 processor, and an advanced camera system for stunning photos and videos.",
                ImageFile = "https://m.media-amazon.com/images/I/61jLiK+q9sL._AC_SX679_.jpg",
                Price = 899.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "OnePlus 11 Pro",
                Category = new List<string> { "Mobile", "Electronics" },
                Description = "The OnePlus 11 Pro features a 6.7-inch Fluid AMOLED display, Snapdragon 8 Gen 2 processor, and a versatile camera system for stunning photos and videos.",
                ImageFile = "https://m.media-amazon.com/images/I/61jLiK+q9sL._AC_SX679_.jpg",
                Price = 799.99m
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sony Xperia 1 IV",
                Category = new List<string> { "Mobile", "Electronics" },
                Description = "The Sony Xperia 1 IV features a 6.5-inch 4K OLED display, Snapdragon 8 Gen 1 processor, and an advanced camera system for stunning photos and videos.",
                ImageFile = "https://m.media-amazon.com/images/I/61jLiK+q9sL._AC_SX679_.jpg",
                Price = 1199.99m
            }
        };
    }
}
