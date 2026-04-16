using AutoMapper;
using DTO.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NLog;
using OfficeOpenXml;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
 IProductRepository productRepository,
 ICategoryRepository categoryRepository,
 IMapper mapper, ILogger<ProductsController> logger) : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Tüm ürünler listeleniyor.");
            var product = await productRepository.GetAllAsync("Category");
            var productDto = mapper.Map<IEnumerable<ProductListDto>>(product);
            _logger.LogInformation("Toplam {Count} ürün getirildi.", productDto.Count());
            return Ok(productDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await productRepository.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpGet("lowstock")]
        
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Kritik stok listesi istendi.");

            var stock = await productRepository.GetAllAsync("Category");
            var lowstock = stock.Where(x => x.CurrentStock <= x.ReorderLevel);
            var lowStockDto = mapper.Map<IEnumerable<ProductListDto>>(lowstock);
            _logger.LogInformation("Kritik stokta {Count} ürün bulundu.", lowStockDto.Count());
            return Ok(lowStockDto);
        }



        [HttpGet("ExportToExcel")]
       

        public async Task<IActionResult> ExportToExcel()

        {

            try
            {
                
                var products = await productRepository.GetAllAsync();
                var productDtos = mapper.Map<List<ProductListDto>>(products);

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Ürün Listesi");

                string[] headers = { "ID", "Ürün", "Kategori", "Kategori ID", "Birim", "Güncel Stok", "Min Stok Seviyesi", "Alış Fiyatı", "Oluşturulma Tarihi" };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cells[1, i + 1];
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                }

                // verileri Satır Satır Yazma
                int row = 2;
                foreach (var item in productDtos)
                {
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.Name;
                    worksheet.Cells[row, 3].Value = item.CategoryName;
                    worksheet.Cells[row, 4].Value = item.CategoryId;

                    // birim Enum değerini metin  (Kg, Adet
                    worksheet.Cells[row, 5].Value = item.UnitOfMeasure.ToString();

                    worksheet.Cells[row, 6].Value = item.CurrentStock;
                    worksheet.Cells[row, 7].Value = item.ReorderLevel;
                    worksheet.Cells[row, 8].Value = item.PurchasePrice;

                    var dateCell = worksheet.Cells[row, 9];
                    dateCell.Value = item.CreatedDate;
                    dateCell.Style.Numberformat.Format = "dd.MM.yyyy HH:mm";

                    row++;
                }

           
                if (worksheet.Dimension != null)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                var bytes = package.GetAsByteArray();
                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Guncel_Urun_Listesi.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Excel dışa aktarma hatası: " + ex.Message });
            }
        }  

        [HttpGet("DownloadTemplate")]
        public IActionResult DownloadTemplate()
        {
            try
            {
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Urun Yukleme Sablonu");

                
                var headers = new string[] { "Ürün", "Kategori", "Kategori ID", "Birim", "Güncel Stok", "Min Stok Seviyesi", "Alış Fiyatı" };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                
                worksheet.Cells[2, 1].Value = "Örnek Ürün";
                worksheet.Cells[2, 2].Value = "Mutfak";
                worksheet.Cells[2, 3].Value = 1; 
                worksheet.Cells[2, 4].Value = "Adet";
                worksheet.Cells[2, 5].Value = 10;
                worksheet.Cells[2, 6].Value = 5;
                worksheet.Cells[2, 7].Value = 150.50;

                if (worksheet.Dimension != null)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                var bytes = package.GetAsByteArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Urun_Yukleme_Sablonu.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Şablon hatası: " + ex.Message });
            }
        }

        [HttpPost("import")]
       [Authorize(Roles = "Asistan")]

        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            _logger.LogInformation("Excel import işlemi başlatıldı. Dosya: {FileName}", file?.FileName);
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

            
            string NormalizeHeader(string s) =>
                (s ?? "")
                .Trim()
                .Replace("\u00A0", " ")
                .Trim();

           
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
            
            if (colName <= 0)
            {
                return BadRequest("Excel başlıkları hatalı! 'Ürün' sütunu mutlaka bulunmalıdır.");
            }

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
                    _logger.LogError(ex, "Excel import sırasında hata oluştu.");

                    return BadRequest($"Satır {row}: {ex.Message}");
                }
            }

           
            var categories = await categoryRepository.GetAllAsync();

            
            var existingProducts = await productRepository.GetAllAsync("Category");

            int inserted = 0;
            int updated = 0;
            int skipped = 0;

            foreach (var dto in dtoList)
            {
                
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    skipped++;
                    continue;
                }

               
                if (string.IsNullOrWhiteSpace(dto.CategoryName))
                    return BadRequest($"Kategori boş olamaz. Ürün: '{dto.Name}'");

                
                var category = categories.FirstOrDefault(c =>
                    string.Equals(c.Name?.Trim(), dto.CategoryName?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (category == null)
                    return BadRequest($"Kategori bulunamadı: '{dto.CategoryName}'. (Ürün: '{dto.Name}')");

                
                var existing = existingProducts.FirstOrDefault(p =>
                    string.Equals(p.Name?.Trim(), dto.Name?.Trim(), StringComparison.OrdinalIgnoreCase));

                if (existing == null)
                {
                   
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

                    await productRepository.AddAsync(entity);
                    inserted++;
                }
                else
                {
                    // UPDATE 
                    existing.PurchasePrice = dto.PurchasePrice;
                    existing.CategoryId = category.Id;
                    existing.UnitOfMeasure = dto.UnitOfMeasure;
                    existing.CurrentStock = dto.CurrentStock;
                    existing.ReorderLevel = dto.ReorderLevel;
                    existing.UpdatedDate = DateTime.Now;

                    await productRepository.UpdateAsync(existing);
                    updated++;
                }
            }
            _logger.LogInformation(
    "Excel import tamamlandı. Toplam: {Total}, Inserted: {Inserted}, Updated: {Updated}, Skipped: {Skipped}",
    dtoList.Count, inserted, updated, skipped);
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
            _logger.LogInformation("Ürün ekleme isteği alındı. Kullanıcı: {User}", User.Identity?.Name);
            if (await productRepository.ExistsAsync(x => x.Name == dto.Name && !x.IsDeleted))
            {
                _logger.LogWarning("Aynı isimde ürün mevcut: {ProductName}", dto.Name);
                return BadRequest("Bu ürün zaten mevcut!"); // sadece aktif ürünlerde aynı isim olmasın
            }


            if (dto == null) {
                _logger.LogWarning("Ürün ekleme null dto ile geldi."); 
                return BadRequest(); }
            var data = mapper.Map<Product>(dto);
            data.CreatedDate = DateTime.Now;
            data.UpdatedDate = null;
            await productRepository.AddAsync(data);

            _logger.LogInformation("Yeni ürün eklendi. Id: {Id}, Name: {Name}", data.Id, data.Name);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Ürün silme isteği alındı. Id: {Id}", id);
            var data = await productRepository.RemoveAsync(id);
            if (data == null) {
                _logger.LogWarning("Silinmek istenen ürün bulunamadı. Id: {Id}", id);   
                return NotFound(); }
            _logger.LogInformation("Ürün silindi. Id: {Id}", id);

            return Ok(data);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            _logger.LogInformation("Tedarikçi güncelleme isteği alındı. Id: {Id}", id);
            var product = await productRepository.GetByIdAsync(id);
            if (product == null) { _logger.LogWarning("Güncellenmek istenen ürün bulunamadı. Id: {Id}", id);
                return NotFound(); }

            mapper.Map(dto, product);
              product.UpdatedDate = DateTime.Now;

            var updteProduct = await productRepository.UpdateAsync(product);
            _logger.LogInformation("Tedarikçi güncellendi. Id: {Id}", id);
            return Ok(updteProduct);
        }


    }

}
