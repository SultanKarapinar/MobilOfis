using AutoMapper;
using DTO.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;


        public ProductsController(
     IProductRepository productRepository,
     ICategoryRepository categoryRepository,
     IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _productRepository.GetAllAsync("Category");
            var productDto = _mapper.Map<IEnumerable<ProductListDto>>(product);
            return Ok(productDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _productRepository.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpGet("lowstock")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Get(int id)
        {
            var stock = await _productRepository.GetAllAsync("Category");
            var lowstock = stock.Where(x => x.CurrentStock <= x.ReorderLevel);
            var lowStockDto = _mapper.Map<IEnumerable<ProductListDto>>(lowstock);

            return Ok(lowStockDto);
        }



        [HttpGet("ExportToExcel")]
        // [Authorize(Roles ="Asistan")]

        public async Task<IActionResult> ExportToExcel()

        {
            //var products = await _productRepository.GetAllAsync();
            //var productDtos = _mapper.Map<List<ProductListDto>>(products);

            //var properties = typeof(ProductListDto).GetProperties();//reflection yaptık 
            ////özellikleri listelemek için kullandık

            //using ExcelPackage page = new ExcelPackage(); //boş excel oluştruldu
            //var table = page.Workbook.Worksheets.Add("Products List");




            try
            {
                

                
                var products = await _productRepository.GetAllAsync();
                var productDtos = _mapper.Map<List<ProductListDto>>(products);

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Products List");

                // Eğer veri yoksa  header yaz
                if (productDtos == null || productDtos.Count == 0)
                {
                    worksheet.Cells[1, 1].Value = "Veri bulunamadı";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                }
                else
                {
                    var properties = typeof(ProductListDto).GetProperties();

                    int col = 1;
                    foreach (var prop in properties)
                    {
                        worksheet.Cells[1, col].Value = prop.Name;
                        worksheet.Cells[1, col].Style.Font.Bold = true;
                        col++;
                    }

                    int row = 2;
                    foreach (var item in productDtos)
                    {
                        col = 1;
                        foreach (var prop in properties)
                        {
                            var value = prop.GetValue(item);
                            var cell = worksheet.Cells[row, col];

                            if (value is DateTime dt)
                            {
                                cell.Value = dt;
                                cell.Style.Numberformat.Format = "dd.MM.yyyy HH:mm";
                            }
                            else if (value == null)
                            {
                                cell.Value = ""; // null değerler boş
                            }
                            else
                            {
                                cell.Value = value;
                            }

                            col++;
                        }
                        row++;
                    }

                    // Sütun genişliklerini 
                    if (worksheet.Dimension != null)
                    {
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();//otamatık
                    }
                }

              
                var bytes = package.GetAsByteArray();
                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Products.xlsx"
                );
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("import")]
        //[Authorize(Roles = "Asistan")]

        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null)
                return BadRequest("Lütfen bir Excel dosyası yükleyiniz.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();

            if (sheet == null)
                return BadRequest("Excel sayfası bulunamadı.");

            if (sheet.Dimension == null)
                return BadRequest("Excel sayfası boş.");

            int rowCount = sheet.Dimension.Rows;
            int colCount = sheet.Dimension.Columns;

            // --- Header normalize helpers ---
            string NormalizeHeader(string s) =>
                (s ?? "")
                .Trim()
                .Replace("\u00A0", " ")
                .Trim();

            // Header -> ColumnIndex map
            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int c = 1; c <= colCount; c++)
            {
                var h = NormalizeHeader(sheet.Cells[1, c].Text);
                if (!string.IsNullOrWhiteSpace(h) && !headerMap.ContainsKey(h))
                    headerMap[h] = c;
            }

            int GetCol(string header)
            {
                var key = NormalizeHeader(header);
                return headerMap.TryGetValue(key, out var idx) ? idx : -1;
            }

            // Türkçe başlıklar
            int colId = GetCol("ID");
            int colName = GetCol("Ürün");
            int colPurchasePrice = GetCol("Alış Fiyatı");
            int colCategoryName = GetCol("Kategori");
            int colCategoryId = GetCol("Kategori ID");
            int colUnit = GetCol("Birim");
            int colCurrentStock = GetCol("Güncel Stok");
            int colReorderLevel = GetCol("Min Stok Seviyesi");
            int colCreatedDate = GetCol("Oluşturulma Tarihi");
            int colUpdatedDate = GetCol("Güncelleme Tarihi");

            // Minimum gerekli alanlar
            if (colName <= 0 || colCategoryName <= 0 || colCategoryId <= 0 || colUnit <= 0 || colCurrentStock <= 0 || colReorderLevel <= 0)
                return BadRequest("Excel başlıkları beklenen formatta değil. Gerekli: Ürün, Kategori, Kategori ID, Birim, Güncel Stok, Min Stok Seviyesi");

            var dtoList = new List<ProductListDto>();
            var tr = new System.Globalization.CultureInfo("tr-TR");

            int ReadInt(int row, int col, string fieldName)
            {
                if (col <= 0) return 0;

                var cell = sheet.Cells[row, col];
                var v = cell.Value;
                if (v == null) return 0;

                if (v is double d) return Convert.ToInt32(d);

                var text = cell.Text?.Trim()
                    ?.Replace("\u00A0", " ")
                    ?.Trim();

                if (int.TryParse(text, out var iv)) return iv;

                throw new FormatException($"'{fieldName}' sayı olmalı: '{text}'");
            }

            decimal ReadDecimal(int row, int col, string fieldName)
            {
                if (col <= 0) return 0m;

                var cell = sheet.Cells[row, col];
                var v = cell.Value;
                if (v == null) return 0m;

                if (v is double d) return Convert.ToDecimal(d);

                var text = cell.Text?.Trim()
                    ?.Replace("\u00A0", " ")
                    ?.Trim();

                if (decimal.TryParse(text, System.Globalization.NumberStyles.Any, tr, out var dv))
                    return dv;

                throw new FormatException($"'{fieldName}' sayı olmalı: '{text}'");
            }

            DateTime? ReadDate(int row, int col, string fieldName)
            {
                if (col <= 0) return null;

                var cell = sheet.Cells[row, col];
                var v = cell.Value;
                if (v == null) return null;

                if (v is double d) return DateTime.FromOADate(d);

                var text = cell.Text?.Trim()
                    ?.Replace("\u00A0", " ")
                    ?.Trim();

                if (string.IsNullOrWhiteSpace(text)) return null;

                if (DateTime.TryParseExact(text, "dd.MM.yyyy", tr, System.Globalization.DateTimeStyles.None, out var dt))
                    return dt;

                if (DateTime.TryParse(text, out var dt2))
                    return dt2;

                throw new FormatException($"'{fieldName}' tarih formatı hatalı: '{text}' (ör: 10.01.2026)");
            }

            // --- Excel satırlarını oku ---
            for (int row = 2; row <= rowCount; row++)
            {
                var nameText = sheet.Cells[row, colName].Text?.Trim()
                    ?.Replace("\u00A0", " ")
                    ?.Trim();

                if (string.IsNullOrWhiteSpace(nameText))
                    continue;

                var categoryText = sheet.Cells[row, colCategoryName].Text?.Trim()
                    ?.Replace("\u00A0", " ")
                    ?.Trim();

                if (string.IsNullOrWhiteSpace(categoryText))
                    return BadRequest($"Satır {row}: Kategori boş olamaz.");

                try
                {
                    var dto = new ProductListDto();

                    // ID opsiyonel
                    if (colId > 0)
                    {
                        var idText = sheet.Cells[row, colId].Text?.Trim();
                        if (!string.IsNullOrWhiteSpace(idText) && int.TryParse(idText, out var idVal))
                            dto.Id = idVal;
                    }

                    dto.Name = nameText;
                    dto.CategoryName = categoryText;

                    dto.PurchasePrice = ReadDecimal(row, colPurchasePrice, "Alış Fiyatı");
                    dto.CategoryId = ReadInt(row, colCategoryId, "Kategori ID");

                    // Birim (güvenli mapping)
                    dto.UnitOfMeasure = UnitOfMeasure.Adet; // default
                    var rawUnit = sheet.Cells[row, colUnit].Text;

                    if (!string.IsNullOrWhiteSpace(rawUnit))
                    {
                        var normalized = rawUnit.Trim()
                            .Replace("\u00A0", " ")
                            .Trim()
                            .ToLowerInvariant();

                        switch (normalized)
                        {
                            case "adet":
                                dto.UnitOfMeasure = UnitOfMeasure.Adet;
                                break;
                            case "kg":
                            case "kilogram":
                                dto.UnitOfMeasure = UnitOfMeasure.Kg;
                                break;
                            case "paket":
                                dto.UnitOfMeasure = UnitOfMeasure.Paket;
                                break;
                            case "litre":
                                dto.UnitOfMeasure = UnitOfMeasure.Litre;
                                break;
                            default:
                                return BadRequest($"Satır {row}: Birim geçersiz: '{rawUnit}'. Geçerli değerler: Kg, Paket, Litre, Adet");
                        }
                    }

                    dto.CurrentStock = ReadInt(row, colCurrentStock, "Güncel Stok");
                    dto.ReorderLevel = ReadInt(row, colReorderLevel, "Min Stok Seviyesi");

                    dto.CreatedDate = ReadDate(row, colCreatedDate, "Oluşturulma Tarihi") ?? DateTime.Now;
                    dto.UpdatedDate = ReadDate(row, colUpdatedDate, "Güncelleme Tarihi");

                    dtoList.Add(dto);
                }
                catch (FormatException ex)
                {
                    return BadRequest($"Satır {row}: {ex.Message}");
                }
            }

            // --- DB'ye kaydet ---
            // DB'deki mevcut kategorileri çek (adıyla eşleştireceğiz)
            var categories = await _categoryRepository.GetAllAsync();

            // DB'deki mevcut ürünleri çek (adıyla eşleştireceğiz)
            var existingProducts = await _productRepository.GetAllAsync("Category");

            // Sayaçlar (SADECE 1 KEZ tanımla)
            int inserted = 0;
            int updated = 0;
            int skipped = 0;

            foreach (var dto in dtoList)
            {
                // güvenlik: Name boşsa atla
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    skipped++;
                    continue;
                }

                // CategoryName zorunlu
                if (string.IsNullOrWhiteSpace(dto.CategoryName))
                    return BadRequest($"Kategori boş olamaz. Ürün: '{dto.Name}'");

                // Kategori bul (adıyla)
                var category = categories.FirstOrDefault(c =>
                    string.Equals(c.Name?.Trim(), dto.CategoryName?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (category == null)
                    return BadRequest($"Kategori bulunamadı: '{dto.CategoryName}'. (Ürün: '{dto.Name}')");

                // Ürün var mı? (adıyla)
                var existing = existingProducts.FirstOrDefault(p =>
                    string.Equals(p.Name?.Trim(), dto.Name?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (existing == null)
                {
                    // INSERT
                    var entity = new Product
                    {
                        Name = dto.Name.Trim(),
                        PurchasePrice = dto.PurchasePrice,
                        CategoryId = category.Id,
                        UnitOfMeasure = dto.UnitOfMeasure,
                        CurrentStock = dto.CurrentStock,
                        ReorderLevel = dto.ReorderLevel,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = null
                    };

                    await _productRepository.AddAsync(entity);
                    inserted++;
                }
                else
                {
                    // UPDATE (Excel'deki değerlerle güncelle)
                    existing.PurchasePrice = dto.PurchasePrice;
                    existing.CategoryId = category.Id;
                    existing.UnitOfMeasure = dto.UnitOfMeasure;
                    existing.CurrentStock = dto.CurrentStock;
                    existing.ReorderLevel = dto.ReorderLevel;
                    existing.UpdatedDate = DateTime.Now;

                    await _productRepository.UpdateAsync(existing);
                    updated++;
                }
            }

            return Ok(new
            {
                message = "Excel başarıyla işlendi",
                count = dtoList.Count,
                inserted,
                updated,
                skipped
            });

        }




            [HttpPost]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] ProductAddDto dto)

        {
            if (await _productRepository.ExistsAsync(x => x.Name == dto.Name))
                return BadRequest("Bu ürün zaten mevcut!");//aynı ısme sahıp olmasınlar dıye
            if (dto == null) { return BadRequest(); }
            var data = _mapper.Map<Product>(dto);
            data.CreatedDate = DateTime.Now;
            data.UpdatedDate = null;
            await _productRepository.AddAsync(data);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _productRepository.RemoveAsync(id);
            if (data == null) { return NotFound(); }
            return Ok(data);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) { return NotFound(); }

            _mapper.Map(dto, product);
            product.UpdatedDate = DateTime.Now;

            var updteProduct = await _productRepository.UpdateAsync(product);
            return Ok(updteProduct);
        }


    }

}
