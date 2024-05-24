using IMS.infrastructure.Irepository;
using IMS.infrastructure.Repository.CRUD;
using IMS.models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;


namespace IMS.web.Controllers
{
	[Authorize(Roles = "ADMIN")]
	public class ProductController : Controller
	{
		private readonly ICrudServices<ProductInfo> _productInfo;
		private readonly ICrudServices<StoreInfo> _storeInfo;
		private readonly ICrudServices<CategoryInfo> _categoryInfo;
        private readonly ICrudServices<UnitInfo> _unitInfo;
        private readonly UserManager<ApplicationUser> _userManager;

		public ProductController(ICrudServices<ProductInfo> productInfo,
			ICrudServices<StoreInfo> storeInfo, ICrudServices<CategoryInfo> categoryInfo, 
			ICrudServices<UnitInfo> unitInfo, UserManager<ApplicationUser> userManager)
		{
			_productInfo = productInfo;
			_storeInfo = storeInfo;
			_categoryInfo = categoryInfo;
            _unitInfo = unitInfo;
            _userManager = userManager;
		}



		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var userId = _userManager.GetUserId(HttpContext.User);
			var user = await _userManager.FindByIdAsync(userId);

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);

            var productInfos = await _productInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(productInfos);
        }

		[HttpGet]
		public async Task<IActionResult> AddEdit(int Id)
		{
			ProductInfo productInfo = new ProductInfo();
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            productInfo.IsActive = true;
			if (Id > 0)
			{
				productInfo = await _productInfo.GetAsync(Id);

			}

			return View(productInfo);
		}


		[HttpPost]
		public async Task<IActionResult> AddEdit(ProductInfo productInfo)
		{
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            if (ModelState.IsValid)
			{
				try
				{
					var userId = _userManager.GetUserId(HttpContext.User);
					var user = await _userManager.FindByIdAsync(userId);

                    if (productInfo.ImageFile != null)
                    {
                        string fileDirectory = $"wwwroot/ProductImage";

                        if (!Directory.Exists(fileDirectory))
                        {
                            Directory.CreateDirectory(fileDirectory);
                        }
                        string uniqueFileName = Guid.NewGuid() + "_" + productInfo.ImageFile.FileName;
                        string filePath = Path.Combine(Path.GetFullPath($"wwwroot/ProductImage"), uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productInfo.ImageFile.CopyToAsync(fileStream);
                            productInfo.ImageUrl = $"ProductImage/" + uniqueFileName;

                        }

                    }

                    if (productInfo.Id == 0)
					{
						productInfo.CreatedDate = DateTime.Now;
						productInfo.CreatedBy = userId;
						productInfo.StoreInfoId = user.StoreId;
						await _productInfo.InsertAsync(productInfo);

						TempData["success"] = "Data Added Successfully!";
					}
					else
					{
						var OrgProductInfo = await _productInfo.GetAsync(productInfo.Id);
						OrgProductInfo.CategoryInfoId = productInfo.CategoryInfoId;
						OrgProductInfo.ProductName = productInfo.ProductName;
						OrgProductInfo.ProductDescription = productInfo.ProductDescription;
						OrgProductInfo.UnitInfoId = productInfo.UnitInfoId;
						OrgProductInfo.StoreInfoId = productInfo.StoreInfoId;
                        OrgProductInfo.ImageUrl = productInfo.ImageUrl;                     
                        OrgProductInfo.CreatedBy = productInfo.CreatedBy;
						OrgProductInfo.CreatedDate = DateTime.Now;
						OrgProductInfo.ModifiedDate = DateTime.Now;
						OrgProductInfo.ModifiedBy = userId;
						OrgProductInfo.IsActive = productInfo.IsActive;
                        if (productInfo.ImageFile != null)
                        {
                            OrgProductInfo.ImageUrl = productInfo.ImageUrl;
                        }
                        await _productInfo.UpdateAsync(OrgProductInfo);
						TempData["success"] = "Data Updated Successfully";
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


	}
}

