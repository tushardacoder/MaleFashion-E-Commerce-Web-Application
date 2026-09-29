namespace MaleFashion.Web.Areas.Customer.Models
{
    //public class ShopViewModel
    //{
    //    // ==========================================
    //    // PRODUCTS
    //    // ==========================================

    //    public IList<ShopProductViewModel> Products { get; set; }
    //        = new List<ShopProductViewModel>();


    //    // ==========================================
    //    // FILTERS
    //    // ==========================================

    //    public IList<ShopCategoryViewModel> Categories { get; set; }
    //        = new List<ShopCategoryViewModel>();

    //    public IList<string> Brands { get; set; }
    //        = new List<string>();

    //    public IList<string> Sizes { get; set; }
    //        = new List<string>();

    //    public IList<string> Colors { get; set; }
    //        = new List<string>();

    //    public IList<string> Tags { get; set; }
    //        = new List<string>();


    //    // ==========================================
    //    // SEARCH
    //    // ==========================================

    //    public string? Search { get; set; }


    //    // ==========================================
    //    // FILTER VALUES
    //    // ==========================================

    //    public Guid? CategoryId { get; set; }

    //    public string? Brand { get; set; }

    //    public string? Size { get; set; }

    //    public string? Color { get; set; }

    //    public string? Tag { get; set; }

    //    public decimal? MinPrice { get; set; }

    //    public decimal? MaxPrice { get; set; }


    //    // ==========================================
    //    // SORTING
    //    // ==========================================

    //    public string? Sort { get; set; }


    //    // ==========================================
    //    // PAGING
    //    // ==========================================

    //    public int CurrentPage { get; set; } = 1;

    //    public int PageSize { get; set; } = 12;

    //    public int TotalProducts { get; set; }

    //    public int TotalPages =>
    //        PageSize <= 0
    //            ? 0
    //            : (int)Math.Ceiling(
    //                (double)TotalProducts / PageSize);
    //}


    //// =====================================================
    //// PRODUCT
    //// =====================================================

    //public class ShopProductViewModel
    //{
    //    public Guid Id { get; set; }

    //    public string ProductName { get; set; }
    //        = string.Empty;

    //    public string Branding { get; set; }
    //        = string.Empty;

    //    public decimal Price { get; set; }

    //    public string? PrimaryImage { get; set; }

    //    public IList<string> Colors { get; set; }
    //        = new List<string>();

    //    public IList<string> Sizes { get; set; }
    //        = new List<string>();

    //    public IList<string> Tags { get; set; }
    //        = new List<string>();

    //    public bool IsInStock { get; set; }

    //    public bool IsActive { get; set; }
    //}


    //// =====================================================
    //// CATEGORY
    //// =====================================================

    //public class ShopCategoryViewModel
    //{
    //    public Guid Id { get; set; }

    //    public string Name { get; set; }
    //        = string.Empty;

    //    public int ProductCount { get; set; }
    //}

    public class ShopViewModel
    {
        // ==========================================
        // PRODUCTS
        // ==========================================

        public IList<ShopProductViewModel> Products { get; set; }
            = new List<ShopProductViewModel>();


        // ==========================================
        // FILTERS
        // ==========================================

        public IList<ShopCategoryViewModel> Categories { get; set; }
            = new List<ShopCategoryViewModel>();

        public IList<string> Brands { get; set; }
            = new List<string>();

        public IList<string> Sizes { get; set; }
            = new List<string>();

        public IList<string> Colors { get; set; }
            = new List<string>();

        public IList<string> Tags { get; set; }
            = new List<string>();


        // ==========================================
        // SEARCH
        // ==========================================

        public string? Search { get; set; }


        // ==========================================
        // FILTER VALUES
        // ==========================================

        public Guid? CategoryId { get; set; }

        public string? Brand { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public string? Tag { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }


        // ==========================================
        // SORTING
        // ==========================================

        public string? Sort { get; set; }


        // ==========================================
        // PAGING
        // ==========================================

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 12;

        public int TotalProducts { get; set; }

        public int TotalPages =>
            PageSize <= 0
                ? 0
                : (int)Math.Ceiling(
                    (double)TotalProducts / PageSize);
    }


    // =====================================================
    // PRODUCT
    // =====================================================

    public class ShopProductViewModel
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }
            = string.Empty;

        public string Branding { get; set; }
            = string.Empty;

        public decimal Price { get; set; }

        public string? PrimaryImage { get; set; }


        // IMPORTANT:
        // Color + images belonging to that color
        public IList<ShopProductColorViewModel> Colors { get; set; }
            = new List<ShopProductColorViewModel>();


        public IList<string> Sizes { get; set; }
            = new List<string>();

        public IList<string> Tags { get; set; }
            = new List<string>();

        public bool IsInStock { get; set; }

        public bool IsActive { get; set; }
    }


    // =====================================================
    // PRODUCT COLOR
    // =====================================================

    public class ShopProductColorViewModel
    {
        public string Color { get; set; }
            = string.Empty;

        public IList<string> Images { get; set; }
            = new List<string>();
    }


    // =====================================================
    // CATEGORY
    // =====================================================

    public class ShopCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
            = string.Empty;

        public int ProductCount { get; set; }
    }
}
