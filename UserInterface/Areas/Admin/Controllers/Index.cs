
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Test12.DataAccess.Repository;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Food;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.Production;
using Test12.Models.Models.ReadyFood;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;
using Utility;

namespace UserInterface.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class IndexController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IndexController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }
        //---------------------------------Get All Users -----------------------------
        [Authorize(Roles = $"{SD.Role_Manager},{SD.Role_Admin}")]
        public IActionResult IndexManager()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }
        [Authorize(Roles = $"{SD.Role_Manager},{SD.Role_Admin}")]
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();

            return Json(new { data = users });
        }
        #endregion
        //============================================================================
        //---------------------------------Get All Brands ----------------------------
        public IActionResult RedirectToIndexBrand(string? ID)
        {
            TempData.Keep("BrandFK");
            HttpContext.Session.SetString("BrandFK", ID);

            return RedirectToAction("IndexBrands");
        }
        public IActionResult IndexBrands()
        {
            string? ID = HttpContext.Session.GetString("BrandFK");

            if( ID == null)
            {
                var modelList = new List<LoginTredMarktViewModel>(); // قائمة من LoginTredMarktViewModel

                var brands = _unitOfWork.brandRepository.GetAll(incloudeProperties: "User").ToList();

                foreach (var brand in brands)
                {
                    var model = new LoginTredMarktViewModel
                    {
                        tredList = new List<Brands> { brand }, // إضافة العلامة التجارية إلى القائمة
                    };
                    modelList.Add(model); // إضافة النموذج إلى القائمة
                }

                return View(modelList); // تمرير قائمة النماذج إلى العرض
            }
            else
            {
                var modelList = new List<LoginTredMarktViewModel>(); // قائمة من LoginTredMarktViewModel

                var brands = _unitOfWork.brandRepository.GetAll().Where(u => u.UserId == ID).ToList();

                foreach (var brand in brands)
                {
                    var model = new LoginTredMarktViewModel
                    {
                        tredList = new List<Brands> { brand }, // إضافة العلامة التجارية إلى القائمة
                    };
                    modelList.Add(model); // إضافة النموذج إلى القائمة
                }

                TempData["ID"] = ID;

                return View(modelList); // إرجاع قائمة النماذج
            }
            
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAllBrands(string? ID)
        {

            var model = new LoginTredMarktViewModel
            {
                tredList = new List<Brands>(),
                // إعداد أي بيانات أخرى تحتاجها
            };

            if (ID == "null")
            {
                model.tredList = _unitOfWork.brandRepository.GetAll(incloudeProperties: "User").ToList();
            }
            else
            {
                model.tredList = _unitOfWork.brandRepository.GetAll().Where(u => u.UserId == ID).ToList();
            }

            return Json(new { data = model.tredList }); // إرجاع البيانات من النموذج في JSON

        }
        #endregion
        //============================================================================       
        //--------------------------------- Update Users -----------------------------
        public IActionResult RedirectToUpdateUsers(string? ID)
        {
            TempData.Keep("BrandFK");
            HttpContext.Session.SetString("BrandFK", ID);

            return RedirectToAction("UpdateUsers");
        }

        public IActionResult UpdateUsers() // After Enter تعديل Display التحضيرات والمكونات...
        {
            string? Id = HttpContext.Session.GetString("BrandFK");
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = _userManager.FindByIdAsync(Id).Result;
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        //---------------------------------Add Brands --------------------------------
        public IActionResult RedirectToAddBrands(int? brandFK, int? mainSectionId)
        {

            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFK ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("AddBrands");

        }
        public IActionResult AddBrands()
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");

            LoginTredMarktViewModel viewModel = new()
            {
                TredMarktVM = new Brands()
            };

            if (brandFK == null || brandFK == 0)
            {
                viewModel.TredMarktVM = new Brands();
            }
            else
            {
                viewModel.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBrands(LoginTredMarktViewModel viewModel, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

            if (ModelState.IsValid)
            {
                var existingBrand = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == viewModel.TredMarktVM.BrandID);

                if (existingBrand == null)
                {
                    var setBrands = new Brands
                    {
                        BrandName = viewModel.TredMarktVM.BrandName,
                        CreatedBY = _userManager.GetUserName(User),
                        UserId = _userManager.GetUserId(User),
                        Date1 = DateTime.Now,

                    };
                    _unitOfWork.TredMarketRepository.Add(setBrands);
                    _unitOfWork.Save();

                    viewModel.TredMarktVM.BrandID = setBrands.BrandID;

                    if (file1 != null)
                    {
                        var brandId = setBrands.BrandID.ToString();

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string brandDirectory = Path.Combine(wwwRootPath, "IMAGES" , brandId);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(brandDirectory))
                        {
                            Directory.CreateDirectory(brandDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);

                        string brandPath = Path.Combine(brandDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(brandPath, FileMode.Create))
                        {
                            await file1.CopyToAsync(stream);
                        }

                        setBrands.BrandCoverImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }

                    if (file2 != null)
                    {
                        var brandId = setBrands.BrandID.ToString();

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string brandDirectory = Path.Combine(wwwRootPath, "IMAGES" , brandId);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(brandDirectory))
                        {
                            Directory.CreateDirectory(brandDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);

                        string brandPath = Path.Combine(brandDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(brandPath, FileMode.Create))
                        {
                            await file2.CopyToAsync(stream);
                        }

                        setBrands.BrandLogoImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }

                    if (file3 != null)
                    {
                        var brandId = setBrands.BrandID.ToString();

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string brandDirectory = Path.Combine(wwwRootPath, "IMAGES", brandId);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(brandDirectory))
                        {
                            Directory.CreateDirectory(brandDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file3.FileName);

                        string brandPath = Path.Combine(brandDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(brandPath, FileMode.Create))
                        {
                            await file3.CopyToAsync(stream);
                        }

                        setBrands.BrandFooterImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }
                    TempData["success"] = "تم إضافة العلامة التجارية بشكل ناجح";
                }
                else
                {
                    if (file1 != null)
                    {
                        var brandId = viewModel.TredMarktVM.BrandID.ToString();

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string brandDirectory = Path.Combine(wwwRootPath, "IMAGES", brandId);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(brandDirectory))
                        {
                            Directory.CreateDirectory(brandDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);

                        string brandPath = Path.Combine(brandDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(brandPath, FileMode.Create))
                        {
                            await file1.CopyToAsync(stream);
                        }

                        viewModel.TredMarktVM.BrandCoverImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }

                    if (file2 != null)
                    {
                        var brandId = viewModel.TredMarktVM.BrandID.ToString();

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string brandDirectory = Path.Combine(wwwRootPath, "IMAGES" , brandId);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(brandDirectory))
                        {
                            Directory.CreateDirectory(brandDirectory);
                        }

                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(viewModel.TredMarktVM.BrandLogoImage))
                        {
                            var oldImagePath = Path.Combine(brandDirectory, viewModel.TredMarktVM.BrandLogoImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                                Console.WriteLine($"File deleted successfully: {oldImagePath}");
                            }
                        }
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);

                        string brandPath = Path.Combine(brandDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(brandPath, FileMode.Create))
                        {
                            await file2.CopyToAsync(stream);
                        }

                        viewModel.TredMarktVM.BrandLogoImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }

                    if (file3 != null)
                    {
                        var brandId = viewModel.TredMarktVM.BrandID.ToString();

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string brandDirectory = Path.Combine(wwwRootPath, "IMAGES" , brandId);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(brandDirectory))
                        {
                            Directory.CreateDirectory(brandDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file3.FileName);

                        string brandPath = Path.Combine(brandDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(brandPath, FileMode.Create))
                        {
                            await file3.CopyToAsync(stream);
                        }

                        viewModel.TredMarktVM.BrandFooterImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }
                    // Update the database
                    _unitOfWork.TredMarketRepository.Update(viewModel.TredMarktVM);
                    _unitOfWork.Save();
                    TempData["success"] = "تم تحديث العلامة التجارية بشكل ناجح";
                }
            }
            return RedirectToAction("RedirectToAddBrands", new { brandFK = viewModel.TredMarktVM.BrandID });
        }
        //============================================================================
        //--------------------------------- Update Brands --------------------------------
        public IActionResult RedirectToUpdateBrands(int? brandFK, int? mainSectionId)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFK ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateBrands");

        }
        public IActionResult UpdateBrands() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");
            TempData.Keep("BrandFK"); // Keep the TempData for further use

            LoginTredMarktViewModel PrVM = new()
            {
                TredMarktVM = new Brands(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };

            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == brandFK);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);

            return View(PrVM);
        }

        [HttpPost] // This is for Add or Update Page.
        public async Task<IActionResult> UpdateBrands(LoginTredMarktViewModel viewModel, IFormFile? file1, IFormFile? file2, IFormFile? file3)
        {
            if (ModelState.IsValid)
            {
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Get the root folder

                if (file1 != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);
                    string folderPath = Path.Combine(wwwRootPath, "IMAGES" , brandId);
                    string BrandPath = Path.Combine(folderPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(viewModel.TredMarktVM.BrandCoverImage))
                    {
                        var oldImagePath = Path.Combine(folderPath, viewModel.TredMarktVM.BrandCoverImage);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            Console.WriteLine($"File deleted successfully: {oldImagePath}");
                        }
                    }

                    // Save the new file
                    using (var stream = new FileStream(BrandPath, FileMode.Create))
                    {
                        await file1.CopyToAsync(stream); // Correctly await the async method
                    }

                    viewModel.TredMarktVM.BrandCoverImage = fileName; // Store only the file name in the database
                }
                if (file2 != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);
                    string folderPath = Path.Combine(wwwRootPath, "IMAGES", brandId);
                    string BrandPath = Path.Combine(folderPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(viewModel.TredMarktVM.BrandLogoImage))
                    {
                        var oldImagePath = Path.Combine(folderPath, viewModel.TredMarktVM.BrandLogoImage);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            Console.WriteLine($"File deleted successfully: {oldImagePath}");
                        }
                    }

                    // Save the new file
                    using (var stream = new FileStream(BrandPath, FileMode.Create))
                    {
                        await file2.CopyToAsync(stream); // Correctly await the async method
                    }

                    viewModel.TredMarktVM.BrandLogoImage = fileName; // Store only the file name in the database
                }
                if (file3 != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file3.FileName);
                    string folderPath = Path.Combine(wwwRootPath, "IMAGES" , brandId);
                    string BrandPath = Path.Combine(folderPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(viewModel.TredMarktVM.BrandFooterImage))
                    {
                        var oldImagePath = Path.Combine(folderPath, viewModel.TredMarktVM.BrandFooterImage);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            Console.WriteLine($"File deleted successfully: {oldImagePath}");
                        }
                    }

                    // Save the new file
                    using (var stream = new FileStream(BrandPath, FileMode.Create))
                    {
                        await file3.CopyToAsync(stream); // Correctly await the async method
                    }

                    viewModel.TredMarktVM.BrandFooterImage = fileName; // Store only the file name in the database
                }

                // Update the database
                _unitOfWork.TredMarketRepository.Update(viewModel.TredMarktVM);
                _unitOfWork.Save();

                TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                return RedirectToAction("UpdateBrands", new { brandID = viewModel.TredMarktVM.BrandID });
            }
            else
            {
                return View(viewModel);
            }
        }
        //============================================================================
        [HttpPost]
        public JsonResult DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "User ID is missing." });
            }

            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var result = _userManager.DeleteAsync(user).Result;
            if (!result.Succeeded)
            {
                return Json(new { success = false, message = "Error deleting user." });
            }

            return Json(new { success = true });
        }

        public IActionResult PrintBrands(int brandfk)
        {
            var model = new LoginTredMarktViewModel
            {
                PreparationList = new List<Preparations>(),
                FoodLoginVMlist = new List<FoodStuffs>(),
                ReadyFoodLoginVMlist = new List<ReadyProducts>(),
                ProductionLoginVMlist = new List<Production>(),
                TredMarktVM = new Brands(),
                MainsectionVM = new MainSections(),
            };

            model.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandfk);
            model.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandfk).ToList();
            model.PreparationListCount = _unitOfWork.PreparationRepository.GetAll(incloudeProperties: "componentsCountPrint,toolsCountPrint,stepsCountPrint")
                .Where(u => u.BrandFK == brandfk).ToList();
            model.DeviceToolsLoginVMlist = _unitOfWork.DevicesAndTools.GetAll(incloudeProperties: "Brand")
                .Where(u => u.BrandFK == brandfk).OrderBy(item => item.DevicesAndToolsOrder).ToList();
            model.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll(incloudeProperties: "Brand")
                .Where(u => u.BrandFK == brandfk).ToList();
            model.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll(incloudeProperties: "Brand")
                .Where(u => u.BrandFK == brandfk).OrderBy(item => item.FoodStuffsOrder).ToList();
            model.ProductionListCount = _unitOfWork.itemsRepository.GetAll(incloudeProperties: "component2,toolsCountPrint2,stepsCountPrint2")
                .Where(u => u.BrandFK == brandfk).ToList();

            return new ViewAsPdf("PrintBrands", model)
            {
                FileName = Uri.EscapeDataString("دليل الوصفات - " + model.TredMarktVM.BrandName + ".pdf"),
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0),
                //CustomSwitches = "--no-stop-slow-scripts --disable-smart-shrinking --viewport-size 1280x1024"
                //CustomSwitches = "--disable-smart-shrinking --no-stop-slow-scripts --zoom 1.0",
                CustomSwitches = "--footer-center [page] --footer-font-size 10 --footer-spacing 5 --no-stop-slow-scripts --disable-smart-shrinking --viewport-size 1280x1024"


            };
        }



    }
}
