using AutoMapper;
using Data;
using Infrastructure.Repositories;
using iText.Html2pdf;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Enums;
using Util.Functions;

namespace Logic
{
    public class ProductUseCases : IProductUseCases
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProductUseCases(IProductRepository productRepository, ICategoryRepository categoryRepository, IVendorRepository vendorRepository, IPriceRepository priceRepository, IConfiguration configuration, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _vendorRepository = vendorRepository;
            _priceRepository = priceRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ProductDto> Create(Guid userId, ProductForCreationDto productForCreationDto)
        {
            var vendor = await _vendorRepository.GetById(productForCreationDto.VendorId);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {productForCreationDto.VendorId} not found.");

            var category = await _categoryRepository.GetById(productForCreationDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {productForCreationDto.CategoryId} not found.");

            var product = new Product()
            {
                Name = productForCreationDto.Name,
                Description = productForCreationDto.Description,
                Code = productForCreationDto.Code,
                CategoryId = productForCreationDto.CategoryId,
                VendorId = productForCreationDto.VendorId,
                Brand = productForCreationDto.Brand,
                MinimumStock = productForCreationDto.MinimumStock,
                Stock = 0,
                UnitOfMeasurement = productForCreationDto.UnitOfMeasurement,
                CreatedBy = userId
            };

            product = await _productRepository.Add(product);

            //PurchasePrice
            var purchasePrice = new Price()
            {
                Value = productForCreationDto.PurchasePrice.Value,
                DateTime = DateTime.Now,
                ProductId = product.Id,
                PriceType = ePriceTypes.PurchasePrice
            };

            await _priceRepository.Add(purchasePrice);

            //SalePrice
            var salePrice = new Price()
            {
                Value = productForCreationDto.SalePrice.Value,
                DateTime = DateTime.Now,
                ProductId = product.Id,
                PriceType = ePriceTypes.SalePrice
            };

            await _priceRepository.Add(salePrice);

            await _productRepository.CommitAsync();
            await _priceRepository.CommitAsync();

            var productDto = _mapper.Map<Product, ProductDto>(product);

            productDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);
            productDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);

            return productDto;
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var product = await _productRepository.Delete(userId, id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {id} not found.");

            await _productRepository.CommitAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = (await _productRepository.GetAll()).OrderBy(x => x.Name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var purchasePrice = (await _priceRepository.GetAll())
                    .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.PurchasePrice)
                    .OrderByDescending(x => x.DateTime).FirstOrDefault();            
                prodDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);

                var salePrice = (await _priceRepository.GetAll())
                    .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.SalePrice)
                    .OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);
            }
            return productsDto;
        }

        public async Task<int> GetTotalQty()
        {
            return (await _productRepository.GetAll()).Count();
        }

        public async Task<IEnumerable<ProductDto>> GetByPageAndQty(int skip, int qty)
        {
            var products = (await _productRepository.GetAll()).OrderBy(x => x.Name).Skip(skip).Take(qty);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var purchasePrice = (await _priceRepository.GetAll())
                   .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.PurchasePrice)
                   .OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);

                var salePrice = (await _priceRepository.GetAll())
                    .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.SalePrice)
                    .OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);
            }
            return productsDto;
        }
        
        public async Task<IEnumerable<ProductDto>> GetByFilters(ProductFiltersDto filters)
        {
            var products = (await _productRepository.GetAll()).AsQueryable().Where(filters.GetExpresion()).ToList().OrderBy(x => x.Name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var purchasePrice = (await _priceRepository.GetAll())
                   .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.PurchasePrice)
                   .OrderByDescending(x => x.DateTime).FirstOrDefault(x => filters.Date == null || x.DateTime <= filters.Date);
                prodDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);

                var salePrice = (await _priceRepository.GetAll())
                    .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.SalePrice)
                    .OrderByDescending(x => x.DateTime).FirstOrDefault(x => filters.Date == null || x.DateTime <= filters.Date);
                prodDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);
            }
            return productsDto;
        }

        public async Task<int> GetTotalQtyByFilters(ProductFiltersDto filters)
        {
            return (await _productRepository.GetAll()).AsQueryable().Where(filters.GetExpresion()).Count();
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredByPageAndQty(ProductFiltersDto filters, int skip, int qty)
        {
            var tt = filters.GetExpresion();
            var products = (await _productRepository.GetAll()).AsQueryable().Where(tt).ToList().OrderBy(x => x.Name).Skip(skip).Take(qty);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var purchasePrice = (await _priceRepository.GetAll())
                   .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.PurchasePrice)
                   .OrderByDescending(x => x.DateTime).FirstOrDefault(x => filters.Date == null || x.DateTime <= filters.Date);
                prodDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);

                var salePrice = (await _priceRepository.GetAll())
                    .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.SalePrice)
                    .OrderByDescending(x => x.DateTime).FirstOrDefault(x => filters.Date == null || x.DateTime <= filters.Date);
                prodDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);
            }
            return productsDto;
        }

        public async Task<IEnumerable<ProductDto>> GetByCategory(Guid categoryId)
        {
            var products = (await _productRepository.GetAll()).Where(x => x.CategoryId == categoryId).OrderBy(x => x.Name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            foreach (var prodDto in productsDto)
            {
                var purchasePrice = (await _priceRepository.GetAll())
                   .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.PurchasePrice)
                   .OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);

                var salePrice = (await _priceRepository.GetAll())
                    .Where(p => p.ProductId == prodDto.Id && p.PriceType == ePriceTypes.SalePrice)
                    .OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);
            }

            return productsDto;
        }

        public async Task<ProductDto> GetOne(Guid id)
        {
            var product = await _productRepository.GetById(id, x => x.Include(p => p.Category).Include(p => p.Vendor));
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {id} not found.");

            var productDto = _mapper.Map<Product, ProductDto>(product);

            var purchasePrice = (await _priceRepository.GetAll())
                   .Where(p => p.ProductId == id && p.PriceType == ePriceTypes.PurchasePrice)
                   .OrderByDescending(x => x.DateTime).FirstOrDefault();
            productDto.PurchasePrice = _mapper.Map<Price, PriceDto>(purchasePrice);

            var salePrice = (await _priceRepository.GetAll())
                .Where(p => p.ProductId == id && p.PriceType == ePriceTypes.SalePrice)
                .OrderByDescending(x => x.DateTime).FirstOrDefault();
            productDto.SalePrice = _mapper.Map<Price, PriceDto>(salePrice);

            return productDto;
        }

        public async Task Update(Guid id, ProductDto productDto)
        {
            var vendor = await _vendorRepository.GetById(productDto.VendorId);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {productDto.VendorId} not found.");

            var category = await _categoryRepository.GetById(productDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {productDto.CategoryId} not found.");

            var product = await _productRepository.GetById(id);
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Code = productDto.Code;
            product.CategoryId = productDto.CategoryId;
            product.VendorId = productDto.VendorId;
            product.Brand = productDto.Brand;
            product.MinimumStock = productDto.MinimumStock;
            product.UnitOfMeasurement = productDto.UnitOfMeasurement;
            product.LastModificationBy = productDto.LastModificationBy;

            await _productRepository.Update(product);

            //PurchasePrice
            var lastPurchasePrice = (await _priceRepository.Find(x => x.ProductId == productDto.Id && x.PriceType == ePriceTypes.PurchasePrice))
                .OrderByDescending(x => x.DateTime).FirstOrDefault();
            
            if (lastPurchasePrice == null || lastPurchasePrice.Value != productDto.PurchasePrice.Value)
            {
                var price = new Price()
                {
                    Value = productDto.PurchasePrice.Value,
                    DateTime = DateTime.Now,
                    ProductId = productDto.Id,
                    CreatedBy = productDto.LastModificationBy.Value,
                    PriceType = ePriceTypes.PurchasePrice,
                };

                await _priceRepository.Add(price);
            }

            //SalePrice
            var latSalePrice = (await _priceRepository.Find(x => x.ProductId == productDto.Id && x.PriceType == ePriceTypes.PurchasePrice))
                .OrderByDescending(x => x.DateTime).FirstOrDefault();

            if (latSalePrice == null || latSalePrice.Value != productDto.SalePrice.Value)
            {
                var price = new Price()
                {
                    Value = productDto.SalePrice.Value,
                    DateTime = DateTime.Now,
                    ProductId = productDto.Id,
                    CreatedBy = productDto.LastModificationBy.Value,
                    PriceType = ePriceTypes.SalePrice,
                };

                await _priceRepository.Add(price);
            }

            await _productRepository.CommitAsync();
            await _priceRepository.CommitAsync();
        }

        public async Task<decimal> GetPriceByDate(Guid productId, DateTime date)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {productId} not found.");

            var price = (await _priceRepository.GetAll())
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault(x => x.ProductId == productId && x.DateTime <= date);
            if (price == null)
                throw new Exception("Price not found.");

            return price.Value;
        }

        public async Task<MemoryStream> CreatePdf(ProductFiltersDto filtersDto)
        {
            string directory = _configuration["PdfRoute"];
            string finalDestination = directory + "/" + DateTime.Now.ToString().Replace("/", "_").Substring(0, 11) + ".pdf";


            string source = PdfBasicSchema.GenerateHeader(
                "LISTADO DE PRODUCTOS",
                "sdfsdf", //codigo form
                DateTime.Now, //fecha form
                "01", //version form
                _configuration["ResourcesRoute"],
                "todoHogarLogo.png"
            );

            string htmlStr =
                "<html>" +
                    "<head>" +
                        "<link rel='stylesheet' type='text/css' href='" + _configuration["ResourcesRoute"] + eRoutes.Html + "/style.css'>" +
                        "</head>" +
                        "<body style='font-family: Arial, Helvetica, sans-serif; margin-top: 4.1cm;'>";

            //htmlStr = htmlStr.Insert(htmlStr.Count(),
            //    "<table class='no-border mb-1'>" +
            //        "<tr>" +
            //            "<td class='no-border'></td>" +
            //            "<td class='no-border'></td>" +
            //            "<td class='align-right no-border i' ><span class='mr-05 bold'>Fecha:</span></td>" +
            //            "<td style='box-sizing: border-box; width: 25%'><span class='ml-05'>{valorFecha}</span></td>" +
            //        "</tr>" +
            //    "</table>");

            htmlStr = htmlStr.Insert(htmlStr.Count(),
                "<table class='no-border mb-1'>" +
                    "<tr class='align-center bold'>" +
                        "<td>Nombre</td>" +
                        "<td>Descripción</td>" +
                        "<td>Código</td>" +
                        "<td>Categoría</td>" +
                        "<td>Proveedor</td>" +
                        "<td>Marca</td>" +
                        "<td>Precio de venta</td>" +
                    "</tr>" +
                    "<dinamic></dinamic>" +
                "</table>");

            var products = (await _productRepository.GetAll(x => x.Include(p => p.Category).Include(p => p.Vendor)))
                .AsQueryable().Where(filtersDto.GetExpresion()).OrderBy(x => x.Name);
            foreach (var prod in products)
            {
                var salePrice = (await _priceRepository.GetAll()).Where(p => p.ProductId == prod.Id && p.PriceType == ePriceTypes.SalePrice).OrderByDescending(x => x.DateTime).FirstOrDefault();
                htmlStr = AppendDynamicField(htmlStr, prod.Name, prod.Description, prod.Code, prod.Category.Name, prod.Vendor.Name, prod.Brand, salePrice.Value);
            }
            htmlStr.Replace("<dinamic></dinamic>", "");

            //string saltoPagina = "<div style='page-break-after: always;'></div>";

            htmlStr = htmlStr.Insert(htmlStr.Count(), "</body></html>");

            FinalizePdf(htmlStr, source, finalDestination);

            return await ReturnResponse(finalDestination);
        }

        private void FinalizePdf(string htmlStr, string source, string finalDestination)
        {
            ConverterProperties converterProperties = new ConverterProperties();
            string intermediateDestination = System.IO.Path.GetTempFileName().Split(".")[0] + ".pdf";
            PdfWriter writerIntermedio = new PdfWriter(intermediateDestination);
            PdfDocument pdf = new PdfDocument(writerIntermedio);
            pdf.SetDefaultPageSize(PageSize.A4.Rotate());
            IEventHandler handler = new PdfBasicSchema(pdf, source, _configuration["ResourcesRoute"]);
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handler);
            HtmlConverter.ConvertToPdf(htmlStr, pdf, converterProperties);
            pdf.Close();
            writerIntermedio.Close();

            PdfBasicSchema.GenerateFooter(intermediateDestination, finalDestination);
        }

        private string AppendDynamicField(string htmlStr, string name, string description, string code, string category, string vendor, string brand, decimal salePrice)
        {
            string campoDinamico = "<tr class='font-size2 align-center {b}'> <td style='text-align: left;'>{name}</td> <td>{description}</td> <td>{code}</td> <td>{category}</td> <td>{vendor}</td> <td>{brand}</td> <td>$ {salePrice}</td> </tr>";

            campoDinamico = campoDinamico.Replace("{name}", name);
            campoDinamico = campoDinamico.Replace("{description}", description);
            campoDinamico = campoDinamico.Replace("{code}", code);
            campoDinamico = campoDinamico.Replace("{category}", category);
            campoDinamico = campoDinamico.Replace("{vendor}", vendor);
            campoDinamico = campoDinamico.Replace("{brand}", brand);
            campoDinamico = campoDinamico.Replace("{salePrice}", salePrice.ToString("N", CultureInfo.CreateSpecificCulture("es-ES")));
            int index = htmlStr.IndexOf("<dinamic></dinamic>");
            if (index >= 0)
            {
                htmlStr = htmlStr.Insert(index, campoDinamico);
            }
            return htmlStr;
        }

        private async Task<MemoryStream> ReturnResponse(string finalDestination)
        {
            MemoryStream stream = new MemoryStream();
            using (FileStream pdfFile = File.Open(finalDestination, FileMode.Open))
            {
                await pdfFile.CopyToAsync(stream);
                pdfFile.Close();
            }

            return stream;
        }

    }
}
