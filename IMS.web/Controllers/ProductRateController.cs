﻿using IMS.infrastructure.Irepository;
using IMS.models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class ProductRateController : Controller
    {
        private readonly ICrudServices<ProductInfo> _productInfo;
        private readonly ICrudServices<CategoryInfo> _categoryInfo;
        private readonly ICrudServices<UnitInfo> _unitInfo;
        private readonly ICrudServices<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICrudServices<ProductRateInfo> _productRateInfo;
        private readonly ICrudServices<RackInfo> _rackInfo;
        private readonly ICrudServices<StockInfo> _stockInfo;
        private readonly ICrudServices<TransactionInfo> _transactionInfo;
        private readonly ICrudServices<SupplierInfo> _supplierInfo;

        public ProductRateController(ICrudServices<ProductInfo> productInfo,
            ICrudServices<CategoryInfo> categoryInfo,
            ICrudServices<UnitInfo> unitInfo,
            ICrudServices<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager,
            ICrudServices<ProductRateInfo> productRateInfo,
            ICrudServices<RackInfo> rackInfo,
            ICrudServices<StockInfo> stockInfo,
            ICrudServices<TransactionInfo> transactionInfo,
            ICrudServices<SupplierInfo> supplierInfo)
        {
            _productInfo = productInfo;
            _categoryInfo = categoryInfo;
            _unitInfo = unitInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
            _productRateInfo = productRateInfo;
            _rackInfo = rackInfo;
            _stockInfo = stockInfo;
            _transactionInfo = transactionInfo;
            _supplierInfo = supplierInfo;
        }



        public async Task<IActionResult> Index()
        {
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true);
            var rateRateInfo = await _productRateInfo.GetAllAsync();

            return View(rateRateInfo);
        }

        public async Task<IActionResult> AddEdit(int Id)
        {
            ProductRateInfo productRateInfo = new ProductRateInfo();

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true);
            productRateInfo.PurchasedDate = DateTime.Now;
            productRateInfo.IsActive = true;

            if (Id > 0)
            {
                productRateInfo = await _productRateInfo.GetAsync(Id);
            }
            return View(productRateInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductRateInfo productRateInfo)
        {
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true);
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);



                    if (productRateInfo.Id == 0)
                    {
                        productRateInfo.CreatedDate = DateTime.Now;
                        productRateInfo.CreatedBy = userId;
                        productRateInfo.StoreInfoId = user.StoreId;
                        var rateInfoId = await _productRateInfo.InsertAsync(productRateInfo);

                        TransactionInfo transactionInfo = new TransactionInfo();
                        transactionInfo.TransactionType = "Purchase";
                        transactionInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        transactionInfo.ProductInfoId = productRateInfo.ProductInfoId;
                        transactionInfo.UnitInfoId = productRateInfo.UnitId;
                        transactionInfo.ProductRateInfoId = rateInfoId;
                        transactionInfo.Rate = productRateInfo.CostPrice;
                        transactionInfo.Quantity = productRateInfo.Quantity;
                        transactionInfo.Amount = productRateInfo.CostPrice * productRateInfo.Quantity;
                        transactionInfo.IsActive = true;
                        transactionInfo.CreatedDate = DateTime.Now;
                        transactionInfo.CreatedBy = userId;
                        transactionInfo.StoreInfoId = user.StoreId;
                        await _transactionInfo.InsertAsync(transactionInfo);


                        var stockdet = await _stockInfo.GetAsync(p => p.ProductInfoId == productRateInfo.ProductInfoId);
                        if (stockdet == null)
                        {

                            StockInfo stockInfo = new StockInfo();
                            stockInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                            stockInfo.ProductInfoId = productRateInfo.ProductInfoId;
                            stockInfo.ProductRateInfoId = rateInfoId;
                            stockInfo.Quantity = productRateInfo.Quantity;
                            stockInfo.IsActive = true;
                            stockInfo.CreatedDate = DateTime.Now;
                            stockInfo.CreatedBy = userId;
                            stockInfo.StoreInfoId = user.StoreId;
                            await _stockInfo.InsertAsync(stockInfo);
                        }
                        else
                        {
                            var qty = stockdet.Quantity + productRateInfo.Quantity;
                            stockdet.Quantity = qty;
                            stockdet.ModifiedBy = userId;
                            stockdet.ModifiedDate = DateTime.Now;
                            await _stockInfo.UpdateAsync(stockdet);
                        }

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgproductRateInfo = await _productRateInfo.GetAsync(productRateInfo.Id);
                        OrgproductRateInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        OrgproductRateInfo.ProductInfoId = productRateInfo.ProductInfoId;
                        OrgproductRateInfo.CostPrice = productRateInfo.CostPrice;
                        OrgproductRateInfo.SellingPrice = productRateInfo.SellingPrice;
                        OrgproductRateInfo.Quantity = productRateInfo.Quantity;
                        OrgproductRateInfo.BatchNo = productRateInfo.BatchNo;
                        OrgproductRateInfo.PurchasedDate = productRateInfo.PurchasedDate;
                        OrgproductRateInfo.Expirydate = productRateInfo.Expirydate;
                        OrgproductRateInfo.SupplierInfoId = productRateInfo.SupplierInfoId;
                        OrgproductRateInfo.RackInfoId = productRateInfo.RackInfoId;
                        OrgproductRateInfo.IsActive = productRateInfo.IsActive;
                        OrgproductRateInfo.ModifiedBy = userId;
                        OrgproductRateInfo.ModifiedDate = DateTime.Now;
                        await _productRateInfo.UpdateAsync(OrgproductRateInfo);

                        TransactionInfo transactionInfo = new TransactionInfo();
                        transactionInfo.TransactionType = "Purchase";
                        transactionInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                        transactionInfo.ProductInfoId = productRateInfo.ProductInfoId;
                        transactionInfo.UnitInfoId = productRateInfo.UnitId;
                        transactionInfo.ProductRateInfoId = OrgproductRateInfo.Id;
                        transactionInfo.Rate = productRateInfo.CostPrice;
                        transactionInfo.Quantity = productRateInfo.Quantity;
                        transactionInfo.Amount = productRateInfo.CostPrice * productRateInfo.Quantity;
                        transactionInfo.IsActive = true;
                        transactionInfo.CreatedDate = DateTime.Now;
                        transactionInfo.CreatedBy = userId;
                        transactionInfo.StoreInfoId = user.StoreId;
                        transactionInfo.ModifiedBy = userId;
                        transactionInfo.ModifiedDate = DateTime.Now;

                        await _transactionInfo.InsertAsync(transactionInfo);






                        var stockdet = await _stockInfo.GetAsync(p => p.ProductInfoId == productRateInfo.ProductInfoId);
                        if (stockdet == null)
                        {
                            StockInfo stockInfo = new StockInfo();
                            stockInfo.CategoryInfoId = productRateInfo.CategoryInfoId;
                            stockInfo.ProductInfoId = productRateInfo.ProductInfoId;
                            stockInfo.ProductRateInfoId = OrgproductRateInfo.Id;
                            stockInfo.Quantity = productRateInfo.Quantity;
                            stockInfo.IsActive = true;
                            stockInfo.CreatedDate = DateTime.Now;
                            stockInfo.CreatedBy = userId;
                            stockInfo.StoreInfoId = user.StoreId;
                            await _stockInfo.InsertAsync(stockInfo);
                        }
                        else
                        {
                            var qty = stockdet.Quantity + productRateInfo.Quantity;
                            stockdet.Quantity = qty;
                            stockdet.ModifiedBy = userId;
                            stockdet.ModifiedDate = DateTime.Now;
                            await _stockInfo.UpdateAsync(stockdet);
                        }





                        TempData["success"] = "Data Updated Sucessfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "Something went wrong, please try again later";
                    return RedirectToAction(nameof(AddEdit));
                }
            }

            TempData["error"] = "Please input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }



        [HttpPost]
        [Route("/api/ProductRate/getproduct")]
        public async Task<IActionResult> GetProduct(int CategoryId)
        {
            var productList = await _productInfo.GetAllAsync(p => p.CategoryInfoId == CategoryId);

            return Json(new { productList });
        }

        [HttpPost]
        [Route("/api/ProductRate/getUnit")]
        public async Task<IActionResult> GetUnit(int ProductId)
        {
            var product = await _productInfo.GetAsync(ProductId);

            return Json(new { product });
        }
    }

}