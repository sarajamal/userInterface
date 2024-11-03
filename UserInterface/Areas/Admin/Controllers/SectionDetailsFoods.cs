using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Food;
using Test12.Models.Models.Production;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;

namespace UserInterface.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SectionDetailsFoods : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionDetailsFoods(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;
        }

        //=========================GET LIST ===================================================
        public IActionResult RedirectToFoodAdminList(int brandFK, int? mainSectionId)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFK);
            TempData.Keep("BrandFK");
            return RedirectToAction("FoodListAdmin");

        }
        public IActionResult FoodListAdmin() //this for display List Of التحضيرات Page1
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");
            TempData.Keep("BrandFK"); // Keep the TempData for further use

            LoginTredMarktViewModel FDVM = new()
            {

                FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll()
                .Where(u => u.BrandFK == brandFK).OrderBy(item => item.FoodStuffsOrder).ToList(),
                WelcomTredmarketFood = new LoginTredMarktViewModel()

            };
            FDVM.WelcomTredmarketFood.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            FDVM.WelcomTredmarketFood.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFK);
            FDVM.WelcomTredmarketFood.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFK);
            FDVM.WelcomTredmarketFood.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFK);
            FDVM.WelcomTredmarketFood.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFK);
            FDVM.WelcomTredmarketFood.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFK);
            FDVM.WelcomTredmarketFood.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            FDVM.WelcomTredmarketFood.MainsectionVM = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == mainSectionId);
            FDVM.WelcomTredmarketFood.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            FDVM.WelcomTredmarketFood.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            FDVM.WelcomTredmarketFood.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            FDVM.WelcomTredmarketFood.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            FDVM.WelcomTredmarketFood.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            FDVM.WelcomTredmarketFood.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            FDVM.WelcomTredmarketFood.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();
            FDVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            // Store the FK value in TempData
            TempData["ID"] = brandFK;
            // Display the updated list
            return View(FDVM);
        }
        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll(int? id)
        {

            IEnumerable<FoodStuffs> objfoodList = _unitOfWork.FoodRepository.GetAll()
                .Where(u => u.BrandFK == id).OrderBy(item => item.FoodStuffsOrder).ToList();

            return Json(new { data = objfoodList });
        }
        #endregion
        //============================================================================
        //------------------------------صفحة الإضافة-----------------------------------
        public IActionResult CreateFoodAdmin(int? id)
        {
            LoginTredMarktViewModel FooVM = new()
            {
                FoodLoginVM = new FoodStuffs(),
                FoodLoginVMlist = new List<FoodStuffs>(),
                FoodsLoginVMorder = new List<FoodStuffs>(),
                tredMaeketFoodsVM = new Brands(),

            };

            FooVM.tredMaeketFoodsVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == id);
            FooVM.FoodsLoginVMorder = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == id);
            FooVM.FoodLoginVM = new FoodStuffs();
            FooVM.FoodLoginVMlist = new List<FoodStuffs>();

            return View(FooVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoodAdmin(LoginTredMarktViewModel FoodsVM, int selectFoodvalue)
        {
            if (ModelState.IsValid)
            {
                int foodFK = FoodsVM.tredMaeketFoodsVM.BrandID;

                for (int i = 0; i < FoodsVM.FoodLoginVMlist.Count; i++)
                {
                    var foods = FoodsVM.FoodLoginVMlist[i];

                    // Fetch the last FoodStuffs entry based on FoodStuffsNum
                    var lastFoodStuffs = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == foodFK)
                                        .OrderByDescending(u => u.FoodStuffsNum)
                                        .FirstOrDefault();

                    // Set the FoodStuffsName based on the last entry's FoodStuffsNum
                    int? newFoodStuffsNum = lastFoodStuffs.FoodStuffsNum;

                    if (newFoodStuffsNum != null && lastFoodStuffs.FoodStuffsNum.HasValue)
                    {
                        newFoodStuffsNum = lastFoodStuffs.FoodStuffsNum.Value + 1;
                    }
                    else
                    {
                        newFoodStuffsNum = 1;
                    }

                    var newfoods = new FoodStuffs
                    {
                        FoodStuffsID = foods.FoodStuffsID,
                        BrandFK = foodFK,
                        FoodStuffsName = foods.FoodStuffsName,
                        FoodStuffsNum = newFoodStuffsNum,

                    };

                    string wwwRootFoodPath = _webHostEnvironment.WebRootPath; // get root folder
                    var file1Name1 = $"file1_{newfoods.FoodStuffsID}";
                    var file1ForFood1 = HttpContext.Request.Form.Files[file1Name1];

                    string FoodStuffsID = newfoods.FoodStuffsID.ToString();
                    string BrandFK = newfoods.BrandFK.ToString();
                    var FoodPath1 = Path.Combine(wwwRootFoodPath, "IMAGES", FoodStuffsID);

                    if (file1ForFood1 != null && file1ForFood1.Length > 0)
                    {
                        string fileName11 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForFood1.FileName);

                        if (!Directory.Exists(FoodPath1))
                        {
                            Directory.CreateDirectory(FoodPath1);
                        }

                        using (var fileStream = new FileStream(Path.Combine(FoodPath1, fileName11), FileMode.Create)) //save images
                        {
                            await file1ForFood1.CopyToAsync(fileStream);
                        }
                        newfoods.FoodStuffsImage = fileName11;

                    }
                    //// reOrder2 
                    if (selectFoodvalue == 0)
                    {
                        int IDfoods = newfoods.FoodStuffsID;
                        newfoods.FoodStuffsOrder = IDfoods;
                        //// Get the maximum order value in the existing list
                        //double maxOrder = _unitOfWork.FoodRepository.GetAll()
                        //    .Max(item => item.FoodStuffsOrder) ?? 0.0f; // Default to 0.0f if there are no existing items

                        //// Round down the maxOrder value to the nearest integer
                        //int maxOrderAsInt = (int)Math.Floor(maxOrder);

                        //// Set the new order value for the "اخرى" (Other) item
                        //double newOrder = maxOrderAsInt + 1.0f;
                        //newfoods.FoodStuffsOrder = newOrder;
                    }
                    else
                    {
                        var getIdOrder = _unitOfWork.FoodRepository.Get(u => u.FoodStuffsID == selectFoodvalue);
                        double? OldOrder = getIdOrder.FoodStuffsOrder; // Default to 0.0f if Order is null
                        double? newOrder = OldOrder + 0.1;
                        newfoods.FoodStuffsOrder = newOrder;
                    }

                    var existingFoods = _unitOfWork.FoodRepository.Get(u => u.FoodStuffsID == foods.FoodStuffsID);
                    if (existingFoods != null)
                    {
                        existingFoods.FoodStuffsName = foods.FoodStuffsName;
                        existingFoods.FoodStuffsOrder = newfoods.FoodStuffsOrder;
                        existingFoods.FoodStuffsImage = newfoods.FoodStuffsImage ?? existingFoods.FoodStuffsImage;
                        existingFoods.FoodStuffsNum = newfoods.FoodStuffsNum;

                        _unitOfWork.FoodRepository.Update(existingFoods);
                    }
                    else
                    {
                        _unitOfWork.FoodRepository.Add(newfoods);
                    }

                    _unitOfWork.Save();

                    List<FoodStuffs> obdeviceToolsList = _unitOfWork.FoodRepository.GetAll().OrderBy(item => item.FoodStuffsOrder).ToList();
                    _unitOfWork.Save();
                }

                TempData["success"] = "تم إضافة المواد الغذائية بشكل ناجح";
                return RedirectToAction("RedirectToFoodAdminList", new { brandFK = FoodsVM.tredMaeketFoodsVM.BrandID });
            }

            return View(FoodsVM);
        }
        //============================================================================
        //=============================POST Add ID ===================================
        [HttpPost]
        public IActionResult GetAddID(int BrandFK, LoginTredMarktViewModel FoodsVM)
        {
            // Fetch the production and steps associated with the given ProductionFK
            FoodsVM.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == BrandFK);
            FoodsVM.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll(incloudeProperties: "Brand").Where(u => u.BrandFK == BrandFK).ToList();

            // Create a new step
            var newDevice = new FoodStuffs
            {
                BrandFK = BrandFK,
            };

            // Save the new step to the database
            _unitOfWork.FoodRepository.Add(newDevice);
            _unitOfWork.Save();

            // Return the new step's ID
            return Json(newDevice.FoodStuffsID);
        }
        //============================================================================
        //---------------------------------صفحة التعديل -----------------------------
        public IActionResult UpdateFoodAdmin(int? id)
        {
            TempData.Keep("BrandFK"); // Keep the TempData for further use

            LoginTredMarktViewModel FDVM = new()
            {
                FoodLoginVM = new FoodStuffs(),
                FoodLoginVMlist = new List<FoodStuffs>(),
                tredMaeketFoodsVM = new Brands(),

            };

            FDVM.tredMaeketFoodsVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == id);
            FDVM.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.FoodStuffsID == id);
            FDVM.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll(incloudeProperties: "Brand").Where(u => u.FoodStuffsID == id).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view

            return View(FDVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFoodAdmin(LoginTredMarktViewModel foodViewModel)
        {
            if (ModelState.IsValid)
            {
                if (foodViewModel.FoodLoginVMlist != null)
                {
                    for (int i = 0; i < foodViewModel.FoodLoginVMlist.Count; i++)
                    {
                        var foods = foodViewModel.FoodLoginVMlist[i];

                        string FoodStuffsID = foods.FoodStuffsID.ToString();
                        string BrandFK = foods.BrandFK.ToString();

                        string wwwRootPathSteps = _webHostEnvironment.WebRootPath; // get the root folder
                        var FoodPath = Path.Combine(wwwRootPathSteps, "IMAGES", FoodStuffsID);


                        var file1Name = $"file1_{foods.FoodStuffsID}";
                        var file1Forfoods = HttpContext.Request.Form.Files[file1Name];

                        if (file1Forfoods != null)
                        {
                            if (!string.IsNullOrEmpty(foods.FoodStuffsImage)) // Check if there's an existing image path
                            {
                                var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", FoodStuffsID, foods.FoodStuffsImage);

                                if (System.IO.File.Exists(OldImagePath1))
                                {
                                    System.IO.File.Delete(OldImagePath1); // Delete old image if it exists
                                }
                            }
                            string fileNamefood1 = Guid.NewGuid().ToString() + Path.GetExtension(file1Forfoods.FileName);

                            //اذا المسار مش موجود سو مسار جديد 
                            if (!Directory.Exists(FoodPath))
                            {
                                Directory.CreateDirectory(FoodPath);
                            }

                            using (var fileStream1 = new FileStream(Path.Combine(FoodPath, fileNamefood1), FileMode.Create))
                            {
                                await file1Forfoods.CopyToAsync(fileStream1);
                            }
                            foods.FoodStuffsImage = fileNamefood1; // Update the image path
                        }
                        var existingFoods = _unitOfWork.FoodRepository.Get(u => u.FoodStuffsID == foods.FoodStuffsID);
                        if (existingFoods != null)
                        {

                            existingFoods.FoodStuffsName = foods.FoodStuffsName;
                            existingFoods.FoodStuffsImage = foods.FoodStuffsImage;

                            _unitOfWork.FoodRepository.Update(existingFoods);
                        }
                        else
                        {
                            _unitOfWork.FoodRepository.Add(foods);
                        }
                        _unitOfWork.Save();
                    }
                }
            }
            TempData["success"] = "تم تحديث المواد الغذائية بشكل ناجح";
            return RedirectToAction("RedirectToFoodAdminList", new { brandFK = foodViewModel.FoodLoginVM.BrandFK });
        }
        //============================================================================
        //----------زر الحذف في صفحة قائمة  المواد الغذائية --------------------------
        #region
        //[HttpDelete]
        public IActionResult DelteFooodAdmin(int? id)
        {
            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            var deleteFoodPicture = _unitOfWork.FoodRepository.Get(u => u.FoodStuffsID == id);

            string FoodStuffsID = deleteFoodPicture.FoodStuffsID.ToString();
            string BrandFK = deleteFoodPicture.BrandFK.ToString();

            // Delete the associated image file
            if (!string.IsNullOrEmpty(deleteFoodPicture.FoodStuffsImage))
            {
                string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", FoodStuffsID, deleteFoodPicture.FoodStuffsImage);
                if (System.IO.File.Exists(imagePath1))
                {
                    System.IO.File.Delete(imagePath1);
                }
            }

            _unitOfWork.FoodRepository.Remove(deleteFoodPicture);
            _unitOfWork.Save();

            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToFoodAdminList", new { brandFK = BrandFK }) });
        }
        #endregion
        //============================================================================

        //زر الحذف في صفحة تعديل الأقسام الرئيسية ------------------------------------------------*/
        #region
        //[HttpDelete]
        public IActionResult DeleteSectionFoodUpdate(int? id)
        {
            var DeleteMainSection = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == id);
            if (DeleteMainSection == null)
            {
                ModelState.AddModelError(string.Empty, "لايوجد قسم هنا .");
                return View();
            }
            var BrandFk = DeleteMainSection.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            var deleteFoodPicture = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == BrandFk).ToList();

            if (deleteFoodPicture != null)
            {
                for (int i = 0; i < deleteFoodPicture.Count(); i++)
                {
                    var delet = deleteFoodPicture[i];

                    string FoodStuffsID = delet.FoodStuffsID.ToString();

                    // Delete the associated image file
                    if (!string.IsNullOrEmpty(delet.FoodStuffsImage))
                    {
                        string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", FoodStuffsID, delet.FoodStuffsImage);
                        if (System.IO.File.Exists(imagePath1))
                        {
                            System.IO.File.Delete(imagePath1);
                        }
                    }

                    _unitOfWork.FoodRepository.Remove(delet);
                    _unitOfWork.Save();
                }

            }
            if (DeleteMainSection != null)
            {
                // Delete the associated image file
                if (!string.IsNullOrEmpty(DeleteMainSection.SectionsImage))
                {
                    string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DeleteMainSection.MainSectionsID.ToString(), DeleteMainSection.SectionsImage);
                    if (System.IO.File.Exists(imagePath1))
                    {
                        System.IO.File.Delete(imagePath1);
                    }
                }
                _unitOfWork.MainsectionRepository.Remove(DeleteMainSection);

            }
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateMainSection", "Sections", new { area = "Admin", brandFK = BrandFk }) });

        }
        #endregion
        //============================================================================
        //زر الحذف في صفحة إضافة الأقسام الرئيسية -------------------------------------
        #region
        //[HttpDelete]
        public IActionResult DeleteSectionFoodAdd(int? id)
        {
            var DeleteMainSection = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == id);
            if (DeleteMainSection == null)
            {
                ModelState.AddModelError(string.Empty, "لايوجد قسم هنا .");
                return View();
            }
            var BrandFk = DeleteMainSection.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            var deleteFoodPicture = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == BrandFk).ToList();

            if (deleteFoodPicture != null)
            {
                for (int i = 0; i < deleteFoodPicture.Count(); i++)
                {
                    var delet = deleteFoodPicture[i];

                    string FoodStuffsID = delet.FoodStuffsID.ToString();

                    // Delete the associated image file
                    if (!string.IsNullOrEmpty(delet.FoodStuffsImage))
                    {
                        string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", FoodStuffsID, delet.FoodStuffsImage);
                        if (System.IO.File.Exists(imagePath1))
                        {
                            System.IO.File.Delete(imagePath1);
                        }
                    }

                    _unitOfWork.FoodRepository.Remove(delet);
                    _unitOfWork.Save();
                }

            }
            if (DeleteMainSection != null)
            {
                // Delete the associated image file
                if (!string.IsNullOrEmpty(DeleteMainSection.SectionsImage))
                {
                    string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DeleteMainSection.MainSectionsID.ToString(), DeleteMainSection.SectionsImage);
                    if (System.IO.File.Exists(imagePath1))
                    {
                        System.IO.File.Delete(imagePath1);
                    }
                }
                _unitOfWork.MainsectionRepository.Remove(DeleteMainSection);
            }

            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToAddMainSection", "Sections", new { area = "Admin", brandFK = BrandFk }) });
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> RedirectToFoodAdminUpdate(LoginTredMarktViewModel viewModel)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                var sectionsList = viewModel.MainsectionVMlist.ToList(); // Convert to list to allow indexing
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                for (int i = 0; i < sectionsList.Count; i++)
                {
                    var sections = sectionsList[i];

                    // تحقق إذا كان اسم القسم هو "المواد الغذائية"
                    if (sections.SectionsName == "المواد الغذائية")
                    {
                        sections.IsChecked = true;
                        sections.MainSectionsOrder = sections.MainSectionsID;
                        var SectionPath = Path.Combine(wwwRootPath, "IMAGES", sections.MainSectionsID.ToString());
                        var fileName = $"file_{i}";
                        var fileSections = HttpContext.Request.Form.Files[fileName];

                        if (fileSections != null) // Check if the section is checked
                        {
                            if (fileSections != null)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(sections.SectionsImage))
                                    {
                                        var oldImagePath = Path.Combine(SectionPath, sections.SectionsImage);
                                        if (System.IO.File.Exists(oldImagePath))
                                        {
                                            System.IO.File.Delete(oldImagePath);
                                        }
                                    }

                                    string fileNamesection = Guid.NewGuid().ToString() + Path.GetExtension(fileSections.FileName);

                                    if (!Directory.Exists(SectionPath))
                                    {
                                        Directory.CreateDirectory(SectionPath);
                                    }

                                    using (var fileStream = new FileStream(Path.Combine(SectionPath, fileNamesection), FileMode.Create))
                                    {
                                        await fileSections.CopyToAsync(fileStream);
                                    }

                                    sections.SectionsImage = fileNamesection;
                                    _unitOfWork.MainsectionRepository.Update(sections);
                                    _unitOfWork.Save();
                                }
                                catch (Exception ex)
                                {
                                    // Log the exception
                                    ModelState.AddModelError("", "Error while uploading the image.");
                                    return View(viewModel);
                                }
                            }
                        }
                        else
                        {
                            var existingSections = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == sections.MainSectionsID);
                            if (existingSections != null)
                            {
                                existingSections.SectionsImage = sections.SectionsImage;
                                existingSections.MainSectionsOrder = sections.MainSectionsID;
                                existingSections.IsChecked = true;
                                _unitOfWork.MainsectionRepository.Update(existingSections);
                                _unitOfWork.Save();
                            }
                        }

                        // الخروج من اللوب بعد تنفيذ العمل للقسم "المواد الغذائية"
                        break;
                    }
                }
                _unitOfWork.Save();
                //TempData["success"] = "تم إضافة قسم المواد الغذائية بنجاح";
                return RedirectToAction("RedirectToFoodAdminList", new { brandFk = viewModel.TredMarktVM.BrandID });
            }

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddSectionFoodUpdate(string sectionName, int BrandID)
        {
            var newSection = new MainSections
            {
                SectionsName = sectionName,
                BrandFK = BrandID ,
                IsChecked = true
            };
            _unitOfWork.MainsectionRepository.Add(newSection);
            _unitOfWork.Save();
            newSection.MainSectionsOrder = newSection.MainSectionsID;
            _unitOfWork.Save();
            // منطق إضافة القسم هنا
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateMainSection", "Sections", new { area = "Admin", brandFK = BrandID, mainSectionId = newSection.MainSectionsID }) });
        }

        [HttpPost]
        public IActionResult AddSectionFood(string sectionName, int BrandID)
        {
            var newSection = new MainSections
            {
                SectionsName = sectionName,
                BrandFK = BrandID,
                IsChecked = true
            };
            _unitOfWork.MainsectionRepository.Add(newSection);
            _unitOfWork.Save();
            newSection.MainSectionsOrder = newSection.MainSectionsID;
            _unitOfWork.Save();
            // منطق إضافة القسم هنا
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToAddMainSection", "Sections", new { area = "Admin", brandFK = BrandID, mainSectionId = newSection.MainSectionsID }) });
        }
    }
}
