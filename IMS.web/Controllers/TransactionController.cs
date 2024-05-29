using IMS.infrastructure.Irepository;
using IMS.models.Entity;
using IMS.models.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ICrudServices<ProductInfo> _productInfo;
        private readonly ICrudServices<CategoryInfo> _categoryInfo;
        private readonly ICrudServices<UnitInfo> _unitInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICrudServices<ProductRateInfo> _productRateInfo;
        private readonly ICrudServices<RackInfo> _rackInfo;
        private readonly ICrudServices<StockInfo> _stockInfo;
        private readonly ICrudServices<TransactionInfo> _transactionInfo;
        private readonly ICrudServices<SupplierInfo> _supplierInfo;
        private readonly ICrudServices<CustomerInfo> _customerInfo;
        private readonly ICrudServices<ProductInvoiceInfo> _productInvoiceInfo;
        private readonly ICrudServices<ProductInvoiceDetailInfo> _productInvoiceDetailInfo;

        public TransactionController(ICrudServices<ProductInfo> productInfo,
            ICrudServices<CategoryInfo> categoryInfo,
            ICrudServices<UnitInfo> unitInfo,
            UserManager<ApplicationUser> userManager,
            ICrudServices<ProductRateInfo> productRateInfo,
            ICrudServices<RackInfo> rackInfo,
            ICrudServices<StockInfo> stockInfo,
            ICrudServices<TransactionInfo> transactionInfo,
            ICrudServices<SupplierInfo> supplierInfo,
            ICrudServices<CustomerInfo> customerInfo,
            ICrudServices<ProductInvoiceInfo> productInvoiceInfo,
            ICrudServices<ProductInvoiceDetailInfo> productInvoiceDetailInfo
            )
        {
            _productInfo = productInfo;
            _categoryInfo = categoryInfo;
            _unitInfo = unitInfo;
            _userManager = userManager;
            _productRateInfo = productRateInfo;
            _rackInfo = rackInfo;
            _stockInfo = stockInfo;
            _transactionInfo = transactionInfo;
            _supplierInfo = supplierInfo;
            _customerInfo = customerInfo;
            _productInvoiceInfo = productInvoiceInfo;
            _productInvoiceDetailInfo = productInvoiceDetailInfo;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var transactioinfo = await _productInvoiceInfo.GetAllAsync();
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            return View(transactioinfo);
        }

        public async Task<IActionResult> Transaction()
        {
            TransactionViewModel transactionViewModel = new TransactionViewModel();
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);

            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.CustomerInfos = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(transactionViewModel);
        }


        [HttpPost]
        [Route("/api/Transaction/getproduct")]
        public async Task<IActionResult> GetProduct(int CategoryId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var productList = await _productInfo.GetAllAsync(p => p.CategoryInfoId == CategoryId && p.StoreInfoId == user.StoreId);

            return Json(new { productList });
        }

        [HttpPost]
        [Route("/api/Transaction/getUnit")]
        public async Task<IActionResult> GetUnit(int ProductId)
        {
            var product = await _productInfo.GetAsync(ProductId);

            return Json(new { product });
        }



        [HttpPost]
        [Route("/api/Transaction/getBatch")]
        public async Task<IActionResult> GetBatch(int ProductId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var batchList = await _productRateInfo.GetAllAsync(p => p.ProductInfoId == ProductId && p.StoreInfoId == user.StoreId);

            return Json(new { batchList });
        }

        [HttpPost]
        [Route("/api/Transaction/getProductRate")]
        public async Task<IActionResult> GetProductRate(int ProductRateId)
        {
            var productDetail = await _productRateInfo.GetAsync(ProductRateId);

            return Json(new { productDetail });
        }

    }
}