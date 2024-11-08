﻿
using Microsoft.AspNetCore.Mvc;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models.Food;
using Test12.Models.Models;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;
using Rotativa.AspNetCore;


namespace UserInterface.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PreparationController : Controller
    {
        /* private readonly ApplicationDbContext _context;*/

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PreparationController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;

        }
        //للإرسال FK الى صفحة القائمة بدون رقم ف URL
        public IActionResult RedirectToPreparation(int brandFK, int mainSectionId)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId);
            HttpContext.Session.SetInt32("BrandFK", brandFK);
            TempData.Keep("BrandFK");
            return RedirectToAction("PreparationList");
        }
        public IActionResult PreparationList() //this for display List Of التحضيرات Page1
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");
            TempData.Keep("BrandFK"); // Keep the TempData for further use
            LoginTredMarktViewModel PrVM = new LoginTredMarktViewModel
            {
                PreparationList = _unitOfWork.PreparationRepository.GetAll(incloudeProperties: "componentsCountPrint,toolsCountPrint")
                            .Where(u => u.BrandFK == brandFK).OrderBy(item => item.PreparationsOrder).ToList(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };
            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVM = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == mainSectionId);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();

            TempData["ID"] = brandFK;
            // Assuming you handle the header through layout or another mechanism
            return View(PrVM);
        }
        //============================================================================

        //الانتقال الى صفحة المعلومات1 
        public IActionResult RedirectToInormation(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Informations");
        }

        // الانتقال الى صفحة المعلومات 2 
        public IActionResult Informations() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");

            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                ToolsVarityVMList = new List<PreparationTools>(),
                stepsVM = new List<PreparationSteps>(),
                TredMarktVM = new Brands(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };

            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.componontVMList = _unitOfWork.ComponentRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            PrVM.PreparationsTools = _unitOfWork.PrepaToolsVarietyRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList();


            return View(PrVM);
        }

        [HttpPost] // This is for Add or Update Page.
        public async Task<IActionResult> Informations(LoginTredMarktViewModel PrepaVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // For update
                int preparationID = PrepaVM.PreparationVM.PreparationsID;

                string wwwRootPath = _webHostEnvironment.WebRootPath; // Get the root folder

                string PreparationsID = PrepaVM.PreparationVM.PreparationsID.ToString();

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string folderPath = Path.Combine(wwwRootPath, "IMAGES", PreparationsID);
                    string PreparationPath = Path.Combine(folderPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(PrepaVM.PreparationVM.prepareImage))
                    {
                        var oldImagePath = Path.Combine(folderPath, PrepaVM.PreparationVM.prepareImage);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                            Console.WriteLine($"File deleted successfully: {oldImagePath}");
                        }
                    }

                    // Save the new file
                    using (var stream = new FileStream(PreparationPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); // Correctly await the async method
                    }

                    PrepaVM.PreparationVM.prepareImage = fileName; // Store only the file name in the database
                }

                // Update the database
                _unitOfWork.PreparationRepository.Update(PrepaVM.PreparationVM);
                _unitOfWork.Save();

                TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                return RedirectToAction("RedirectToInormation", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            else
            {
                return View(PrepaVM);
            }
        }
        //============================================================================

        // الانتقال الى صفحة المكوانات 1:
        public IActionResult RedirectToComponent(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Components");
        }

        //للانتقال الى صفحة المكونات2 :
        public IActionResult Components() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");

            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                TredMarktVM = new Brands(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };

            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.componontVMList = _unitOfWork.ComponentRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view

            return View(PrVM);
        }

        [HttpPost] // This is for Add or Update Page.
        public IActionResult Components(LoginTredMarktViewModel PrepaVM)
        {
            if (ModelState.IsValid)
            {
                // For update
                int preparationID = PrepaVM.PreparationVM.PreparationsID;

                if (PrepaVM.componontVMList != null) // تحديث المكونات
                {
                    for (int i = 0; i < PrepaVM.componontVMList.Count; i++)
                    {
                        var Components = PrepaVM.componontVMList[i];

                        //int lastIdComponents = _unitOfWork.ComponentRepository.GetLastComponentId();
                        //int LastId1Components = lastIdComponents + 1;

                        var existingComponent = _unitOfWork.ComponentRepository.Get(u => u.PrepIngredientsID == Components.PrepIngredientsID, incloudeProperties: "Preparation");
                        if (existingComponent == null)
                        {
                            var newComponent = new PreparationIngredients
                            {
                                //PrepIngredientsID = LastId1Components,
                                PreparationsFK = preparationID,
                                PrepQuantity = Components.PrepQuantity,
                                PrepUnit = Components.PrepUnit,
                                PrepIngredientsName = Components.PrepIngredientsName
                            };
                            _unitOfWork.ComponentRepository.Add(newComponent);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            existingComponent.PrepQuantity = Components.PrepQuantity;
                            existingComponent.PrepUnit = Components.PrepUnit;
                            existingComponent.PrepIngredientsName = Components.PrepIngredientsName;

                            _unitOfWork.ComponentRepository.Update(existingComponent);
                            _unitOfWork.Save();
                        }
                    }
                }
                TempData["success"] = "تم تحديث المكونات بشكل ناجح";
                return RedirectToAction("RedirectToComponent", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================

        //الانتقال الى صفحة الأدوات1 
        public IActionResult RedirectToTools(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Tools");
        }
        //الانتقال الى صفحة الأدوات1 
        public IActionResult Tools() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");

            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                ToolsVarityVMList = new List<PreparationTools>(),
                stepsVM = new List<PreparationSteps>(),
                TredMarktVM = new Brands(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };

            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.componontVMList = _unitOfWork.ComponentRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            PrVM.ToolsVarityVMList = _unitOfWork.PrepaToolsVarietyRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList();

            return View(PrVM);
        }
        //Tools POST 
        [HttpPost] // This is for Add or Update Page.
        public IActionResult Tools(LoginTredMarktViewModel PrepaVM)
        {
            if (ModelState.IsValid)
            {
                // For update
                int preparationID = PrepaVM.PreparationVM.PreparationsID;

                if (PrepaVM.ToolsVarityVMList != null) //تحديث الأدوات.
                {
                    for (int i = 0; i < PrepaVM.ToolsVarityVMList.Count; i++)
                    {
                        var Tools = PrepaVM.ToolsVarityVMList[i];

                        //int lastIdTools = _unitOfWork.PrepaToolsVarietyRepository.GetLastToolsId();
                        //int LastId1Tools = lastIdTools + 1;

                        var existingtoolvariety = _unitOfWork.PrepaToolsVarietyRepository.Get(u => u.PrepToolsID == Tools.PrepToolsID, incloudeProperties: "Preparation");
                        if (existingtoolvariety == null)
                        {
                            var firstRowToolAdd = new PreparationTools
                            {
                                //PrepToolsID = LastId1Tools,
                                PreparationsFK = preparationID,
                                PrepTools = Tools.PrepTools,
                            };
                            _unitOfWork.PrepaToolsVarietyRepository.Add(firstRowToolAdd);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            existingtoolvariety.PrepTools = Tools.PrepTools;
                            _unitOfWork.PrepaToolsVarietyRepository.Update(existingtoolvariety);
                            _unitOfWork.Save();
                        }
                    }

                }
                TempData["success"] = "تم تحديث الأدوات بشكل ناجح";
                return RedirectToAction("RedirectToTools", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================

        //الانتقال الى صفحة الخطوات1 
        public IActionResult RedirectToSteps(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Steps");
        }
        //الانتقال الى صفحة الخطوات2 
        public IActionResult Steps() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");

            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                ToolsVarityVMList = new List<PreparationTools>(),
                stepsVM = new List<PreparationSteps>(),
                TredMarktVM = new Brands(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };

            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.stepsVM = _unitOfWork.StepsPreparationRepository.GetAll().Where(c => c.PreparationsFK == PreparationID).OrderBy(item => item.PrepStepsNum).ToList();
            return View(PrVM);
        }


        [HttpPost] // This is for Add or Update Page.
        public async Task<IActionResult> Steps(LoginTredMarktViewModel PrepaVM)
        {
            if (ModelState.IsValid)
            {
                // For update
                int preparationID = PrepaVM.PreparationVM.PreparationsID;
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Get the root folder
                int stepsID = PrepaVM.PreparationVM.PreparationsID;

                if (PrepaVM.stepsVM != null)
                {
                    for (int i = 0; i < PrepaVM.stepsVM.Count; i++)
                    {
                        var Steps = PrepaVM.stepsVM[i];

                        string wwwRootPathSteps = _webHostEnvironment.WebRootPath; // get the root folder

                        //int LastId = _unitOfWork.StepsPreparationRepository.GetLastStepId();
                        //int LastId1 = LastId + 1;

                        var existingSteps9 = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == Steps.PrepStepsID, incloudeProperties: "Preparation");
                        if (existingSteps9 == null)
                        {
                            var newStep = new PreparationSteps
                            {
                                //PrepStepsID = LastId1,
                                PreparationsFK = Steps.PreparationsFK,
                                PrepText = Steps.PrepText,
                                PrepStepsNum = Steps.PrepStepsNum

                            };

                            string IDstep = newStep.PrepStepsID.ToString();
                            string preparationVMFk = PrepaVM.PreparationVM.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{newStep.PrepStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.PrepImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", IDstep, newStep.PrepImage);

                                    if (System.IO.File.Exists(OldImagePath1))
                                    {
                                        System.IO.File.Delete(OldImagePath1); // Delete old image if it exists
                                    }
                                }

                                string fileNameSteps1 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForStep.FileName);

                                //اذا المسار مش موجود سو مسار جديد 
                                if (!Directory.Exists(StepsPath))
                                {
                                    Directory.CreateDirectory(StepsPath);
                                }

                                using (var fileStream1 = new FileStream(Path.Combine(StepsPath, fileNameSteps1), FileMode.Create))
                                {
                                    await file1ForStep.CopyToAsync(fileStream1);
                                }

                                newStep.PrepImage = fileNameSteps1; // Update the image path
                                _unitOfWork.StepsPreparationRepository.Add(newStep);
                                _unitOfWork.Save();
                            }
                        }
                        else
                        {
                            string IDstep = Steps.PrepStepsID.ToString();
                            string preparationVMFk = PrepaVM.PreparationVM.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{Steps.PrepStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.PrepImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", IDstep, Steps.PrepImage);

                                    if (System.IO.File.Exists(OldImagePath1))
                                    {
                                        System.IO.File.Delete(OldImagePath1); // Delete old image if it exists
                                    }
                                }

                                string fileNameSteps1 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForStep.FileName);

                                //اذا المسار مش موجود سو مسار جديد 
                                if (!Directory.Exists(StepsPath))
                                {
                                    Directory.CreateDirectory(StepsPath);
                                }

                                using (var fileStream1 = new FileStream(Path.Combine(StepsPath, fileNameSteps1), FileMode.Create))
                                {
                                    await file1ForStep.CopyToAsync(fileStream1);
                                }
                                Steps.PrepImage = fileNameSteps1;
                            }

                            // Save or update Steps data to the database
                            if (Steps.PreparationsFK == stepsID) // int stepsID = PrepaVM.PreparationVM.التحضير_ID;
                            {
                                var existingSteps = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == Steps.PrepStepsID, incloudeProperties: "Preparation");

                                if (existingSteps != null)
                                {

                                    existingSteps.PrepText = Steps.PrepText;
                                    existingSteps.PrepImage = Steps.PrepImage;
                                    existingSteps.PrepStepsNum = Steps.PrepStepsNum;

                                    _unitOfWork.StepsPreparationRepository.Update(existingSteps);
                                }
                                else
                                {
                                    _unitOfWork.StepsPreparationRepository.Add(Steps);
                                }
                                _unitOfWork.Save();
                            }
                        }
                    }

                }
                TempData["success"] = "تم تحديث الخطوات بشكل ناجح";
                return RedirectToAction("RedirectToSteps", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================

        //صفحة إنشاء المعلومات 
        public IActionResult RedirectToCreateInformations(int? PreparationID, int brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk);

            TempData.Keep("BrandFK");
            return RedirectToAction("CreateInformations");
        }
        public IActionResult CreateInformations() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");

            TempData.Keep("BrandFK"); // Keep the TempData for further use
            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                ToolsVarityVMList = new List<PreparationTools>(),
                stepsVM = new List<PreparationSteps>(),
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
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();
            if (PreparationID == 0 || PreparationID == null)
            {
                PrVM.PreparationVM = new Preparations();
            }
            else
            {
                PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            }
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.componontVMList = new List<PreparationIngredients>();
            PrVM.ToolsVarityVMList = new List<PreparationTools>();
            PrVM.stepsVM = new List<PreparationSteps>();
            PrVM.PreparationList = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK);

            return View(PrVM);
        }

        [HttpPost] //This for Add Or Update Page . 
        public async Task<IActionResult> CreateInformations(LoginTredMarktViewModel PrepaVM, IFormFile? file, int selectedValue) // should insert name in Upsert view
        {
            if (ModelState.IsValid)
            {
                var FK = PrepaVM.TredMarktVM.BrandID;
                //for update .. 
                var existingPreparation = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PrepaVM.PreparationVM.PreparationsID);

                if (existingPreparation == null)  // if Add 
                {

                    var setFK = new Preparations
                    {
                        BrandFK = FK,
                        prepareName = PrepaVM.PreparationVM.prepareName,
                        PreparationTime = PrepaVM.PreparationVM.PreparationTime,
                        VersionNumber = PrepaVM.PreparationVM.VersionNumber,
                        Expiry = PrepaVM.PreparationVM.Expiry,
                        NetWeight = PrepaVM.PreparationVM.NetWeight,
                        Station = PrepaVM.PreparationVM.Station,

                    };
                    _unitOfWork.PreparationRepository.Add(setFK);
                    _unitOfWork.Save();

                    PrepaVM.PreparationVM.PreparationsID = setFK.PreparationsID;
                    //this code for image if add or update.
                    string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                    if (file != null)
                    {

                        // Convert numeric values to strings
                        string PreparationsID = setFK.PreparationsID.ToString(); // Convert to string
                        string preparationVMFk = PrepaVM.TredMarktVM.BrandID.ToString(); // Convert to string

                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        // Combine paths using Path.Combine, ensuring all arguments are strings
                        string PreparationDirectory = Path.Combine(wwwRootPath, "IMAGES", PreparationsID);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(PreparationDirectory))
                        {
                            Directory.CreateDirectory(PreparationDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        string PreparationPath = Path.Combine(PreparationDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(PreparationPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        setFK.prepareImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                        //return RedirectToAction("RedirectToUpsert", new { id = setFK.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
                    }

                    //// reOrder2 
                    if (selectedValue == 0)
                    {
                        int IdPreparation = setFK.PreparationsID;
                        setFK.PreparationsOrder = IdPreparation;
                        //// Get the maximum order value in the existing list
                        //double maxOrder = _unitOfWork.PreparationRepository.GetAll()
                        //    .Max(item => item.PreparationsOrder) ?? 0.0f; // Default to 0.0f if there are no existing items

                        //// Round down the maxOrder value to the nearest integer
                        //int maxOrderAsInt = (int)Math.Floor(maxOrder);

                        //// Set the new order value for the "اخرى" (Other) item
                        //double newOrder = maxOrderAsInt + 1.0f;
                        //setFK.PreparationsOrder = newOrder;
                    }
                    else
                    {
                        var getIdOrder = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == selectedValue);
                        double? OldOrder = getIdOrder.PreparationsOrder; // Default to 0.0f if Order is null
                        double? newOrder = OldOrder + 0.1;
                        setFK.PreparationsOrder = newOrder;
                    }

                    List<Preparations> objPreparationList = _unitOfWork.PreparationRepository.GetAll().OrderBy(item => item.PreparationsOrder).ToList();
                    _unitOfWork.Save();
                    TempData["success"] = "تم إضافة التحضيرات بشكل ناجح";
                }
                else
                {
                    int preparationID = PrepaVM.PreparationVM.PreparationsID;

                    string wwwRootPath = _webHostEnvironment.WebRootPath; // Get the root folder

                    string PreparationsID = PrepaVM.PreparationVM.PreparationsID.ToString();

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string folderPath = Path.Combine(wwwRootPath, "IMAGES", PreparationsID);
                        string PreparationPath = Path.Combine(folderPath, fileName);

                        // Ensure the directory exists
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(PrepaVM.PreparationVM.prepareImage))
                        {
                            var oldImagePath = Path.Combine(folderPath, PrepaVM.PreparationVM.prepareImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                                Console.WriteLine($"File deleted successfully: {oldImagePath}");
                            }
                        }

                        // Save the new file
                        using (var stream = new FileStream(PreparationPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream); // Correctly await the async method
                        }

                        PrepaVM.PreparationVM.prepareImage = fileName; // Store only the file name in the database
                    }

                    // Update the database
                    _unitOfWork.PreparationRepository.UpdateAdmin(PrepaVM.PreparationVM);
                    List<Preparations> objPreparationList = _unitOfWork.PreparationRepository.GetAll().OrderBy(item => item.PreparationsOrder).ToList();
                    _unitOfWork.Save();
                    TempData["success"] = "تم تحديث التحضيرات بشكل ناجح";
                }
            }
            return RedirectToAction("RedirectToCreateInformations", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFK = PrepaVM.TredMarktVM.BrandID });
        }
        //============================================================================

        //صفحة إنشاء المكونات 
        public IActionResult RedirectToCreateComponent(int? PreparationID, int brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateComponent");
        }
        public IActionResult CreateComponent() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");
            TempData.Keep("BrandFK"); // Keep the TempData for further use
            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                componontVM = new PreparationIngredients(),
                stepsVM = new List<PreparationSteps>(),
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
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.componontVM = _unitOfWork.ComponentRepository.Get(u => u.PreparationsFK == PreparationID);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.componontVMList = _unitOfWork.ComponentRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList();
            PrVM.PreparationList = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK);

            return View(PrVM);
        }
        [HttpPost] //This for Add Or Update Page . 
        public IActionResult CreateComponent(LoginTredMarktViewModel PrepaVM) // should insert name in Upsert view
        {
            if (ModelState.IsValid)
            {
                var PreparationFk = PrepaVM.PreparationVM.PreparationsID;

                //int lastIdComponents = _unitOfWork.ComponentRepository.GetLastComponentId();
                //int LastId1Components = lastIdComponents + 1;

                if (PrepaVM.componontVM != null)
                {
                    var firstComponent = new PreparationIngredients
                    {
                        //PrepIngredientsID = LastId1Components,
                        PreparationsFK = PreparationFk,
                        PrepIngredientsName = PrepaVM.componontVM.PrepIngredientsName, // Retrieve data from form
                        PrepUnit = PrepaVM.componontVM.PrepUnit,
                        PrepQuantity = PrepaVM.componontVM.PrepQuantity,
                    };

                    _unitOfWork.ComponentRepository.Add(firstComponent);
                    _unitOfWork.Save();
                }
                if (PrepaVM.componontVMList != null && PrepaVM.componontVMList.Any())
                { // if condition checks whether the PrepaVM.componontVMList is not null and contains at least one item. 
                    for (int i = 0; i < PrepaVM.componontVMList.Count; i++)
                    {
                        var Components = PrepaVM.componontVMList[i];
                        //int lastIdComponents1 = _unitOfWork.ComponentRepository.GetLastComponentId();
                        //int LastId1Components1 = lastIdComponents + 1;

                        var existingComponent = _unitOfWork.ComponentRepository.Get(u => u.PrepIngredientsID == Components.PrepIngredientsID, incloudeProperties: "Preparation");
                        if (existingComponent == null)
                        {
                            //LastId1Components++;
                            var componentId = PrepaVM.PreparationVM.PreparationsID;

                            var newComponent = new PreparationIngredients
                            {
                                //PrepIngredientsID = LastId1Components,
                                PreparationsFK = PreparationFk,
                                PrepQuantity = Components.PrepQuantity,
                                PrepUnit = Components.PrepUnit,
                                PrepIngredientsName = Components.PrepIngredientsName
                            };
                            _unitOfWork.ComponentRepository.Add(newComponent);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            existingComponent.PrepQuantity = Components.PrepQuantity;
                            existingComponent.PrepUnit = Components.PrepUnit;
                            existingComponent.PrepIngredientsName = Components.PrepIngredientsName;

                            _unitOfWork.ComponentRepository.Update(existingComponent);
                            _unitOfWork.Save();
                        }
                    }
                }
                _unitOfWork.Save();
                TempData["success"] = "تم إضافة المكونات بشكل ناجح";
                return RedirectToAction("RedirectToCreateComponent", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFK = PrepaVM.TredMarktVM.BrandID });
            }
            return View(PrepaVM);
        }
        //============================================================================

        // صفحة إنشاء الأدوات
        public IActionResult RedirectToCreateTools(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateTools");
        }
        public IActionResult CreateTools() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");
            TempData.Keep("BrandFK"); // Keep the TempData for further use
            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                ToolsVarityVM = new PreparationTools(),
                ToolsVarityVMList = new List<PreparationTools>(),
                stepsVM = new List<PreparationSteps>(),
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
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.ToolsVarityVM = _unitOfWork.PrepaToolsVarietyRepository.Get(u => u.PreparationsFK == PreparationID);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.ToolsVarityVMList = _unitOfWork.PrepaToolsVarietyRepository.GetAll(incloudeProperties: "Preparation").Where(c => c.PreparationsFK == PreparationID).ToList();
            PrVM.PreparationList = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK);
            return View(PrVM);
        }
        [HttpPost] // This is for Add or Update Page.
        public IActionResult CreateTools(LoginTredMarktViewModel PrepaVM)
        {
            if (ModelState.IsValid)
            {
                // For update
                int preparationFK = PrepaVM.PreparationVM.PreparationsID;
                //int lastIdTools = _unitOfWork.PrepaToolsVarietyRepository.GetLastToolsId();
                //int LastId1Tools = lastIdTools + 1;
                if (PrepaVM.ToolsVarityVM != null)
                {
                    var firstRowToolAdd = new PreparationTools
                    {
                        //PrepToolsID = LastId1Tools,
                        PreparationsFK = preparationFK,
                        PrepTools = PrepaVM.ToolsVarityVM.PrepTools,
                    };
                    _unitOfWork.PrepaToolsVarietyRepository.Add(firstRowToolAdd);
                    _unitOfWork.Save();
                }

                if (PrepaVM.ToolsVarityVMList != null && PrepaVM.ToolsVarityVMList.Any())
                {
                    for (int i = 0; i < PrepaVM.ToolsVarityVMList.Count; i++)
                    {
                        var Tools = PrepaVM.ToolsVarityVMList[i];
                        //int lastIdTools1 = _unitOfWork.PrepaToolsVarietyRepository.GetLastToolsId();
                        //int LastId1Tools1 = lastIdTools1 + 1;

                        var existingtoolvariety = _unitOfWork.PrepaToolsVarietyRepository.Get(u => u.PrepToolsID == Tools.PrepToolsID, incloudeProperties: "Preparation");
                        if (existingtoolvariety == null)
                        {
                            var firstRowToolAdd = new PreparationTools
                            {
                                //PrepToolsID = LastId1Tools1,
                                PreparationsFK = preparationFK,
                                PrepTools = Tools.PrepTools,
                            };
                            _unitOfWork.PrepaToolsVarietyRepository.Add(firstRowToolAdd);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            existingtoolvariety.PrepTools = Tools.PrepTools;
                            _unitOfWork.PrepaToolsVarietyRepository.Update(existingtoolvariety);
                            _unitOfWork.Save();
                        }
                    }
                }
                TempData["success"] = "تم تحديث الأدوات بشكل ناجح";
                return RedirectToAction("RedirectToCreateTools", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================

        //الانتقال الى صفحة الخطوات1 
        public IActionResult RedirectToCreateSteps(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateSteps");
        }
        //الانتقال الى صفحة الخطوات2 
        public IActionResult CreateSteps() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");

            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                stepsVM = new List<PreparationSteps>(),
                TredMarktVM = new Brands(),
                WelcomTredMarketPrecomponent = new LoginTredMarktViewModel()

            };
            PrVM.WelcomTredMarketPrecomponent.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.WelcomTredMarketPrecomponent.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.WelcomTredMarketPrecomponent.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.WelcomTredMarketPrecomponent.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.WelcomTredMarketPrecomponent.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.stepsVM = _unitOfWork.StepsPreparationRepository.GetAll().Where(c => c.PreparationsFK == PreparationID).OrderBy(item => item.PrepStepsNum).ToList();
            return View(PrVM);
        }
        [HttpPost] // This is for Add or Update Page.
        public async Task<IActionResult> CreateSteps(LoginTredMarktViewModel PrepaVM)
        {
            if (ModelState.IsValid)
            {
                // For update
                int preparationFK = PrepaVM.PreparationVM.PreparationsID;
                string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                if (PrepaVM.stepsVM != null)
                {
                    for (int i = 0; i < PrepaVM.stepsVM.Count; i++)
                    {
                        var Steps = PrepaVM.stepsVM[i];

                        string wwwRootPathSteps = _webHostEnvironment.WebRootPath; // get the root folder

                        //int LastId = _unitOfWork.StepsPreparationRepository.GetLastStepId();
                        //int LastId1 = LastId + 1;

                        var existingSteps9 = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == Steps.PrepStepsID, incloudeProperties: "Preparation");
                        if (existingSteps9 == null)
                        {
                            var newStep = new PreparationSteps
                            {
                                //PrepStepsID = LastId1,
                                PreparationsFK = Steps.PreparationsFK,
                                PrepText = Steps.PrepText,
                                PrepStepsNum = Steps.PrepStepsNum

                            };

                            string IDstep = newStep.PrepStepsID.ToString();
                            string preparationVMFk = PrepaVM.PreparationVM.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{newStep.PrepStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.PrepImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", IDstep, newStep.PrepImage);

                                    if (System.IO.File.Exists(OldImagePath1))
                                    {
                                        System.IO.File.Delete(OldImagePath1); // Delete old image if it exists
                                    }
                                }

                                string fileNameSteps1 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForStep.FileName);

                                //اذا المسار مش موجود سو مسار جديد 
                                if (!Directory.Exists(StepsPath))
                                {
                                    Directory.CreateDirectory(StepsPath);
                                }

                                using (var fileStream1 = new FileStream(Path.Combine(StepsPath, fileNameSteps1), FileMode.Create))
                                {
                                    await file1ForStep.CopyToAsync(fileStream1);
                                }

                                newStep.PrepImage = fileNameSteps1; // Update the image path
                                _unitOfWork.StepsPreparationRepository.Add(newStep);
                                _unitOfWork.Save();
                            }
                        }
                        else
                        {
                            string IDstep = Steps.PrepStepsID.ToString();
                            int stepsID = PrepaVM.PreparationVM.PreparationsID;
                            string preparationVMFk = PrepaVM.PreparationVM.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{Steps.PrepStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.PrepImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", IDstep, Steps.PrepImage);

                                    if (System.IO.File.Exists(OldImagePath1))
                                    {
                                        System.IO.File.Delete(OldImagePath1); // Delete old image if it exists
                                    }
                                }

                                string fileNameSteps1 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForStep.FileName);

                                //اذا المسار مش موجود سو مسار جديد 
                                if (!Directory.Exists(StepsPath))
                                {
                                    Directory.CreateDirectory(StepsPath);
                                }

                                using (var fileStream1 = new FileStream(Path.Combine(StepsPath, fileNameSteps1), FileMode.Create))
                                {
                                    await file1ForStep.CopyToAsync(fileStream1);
                                }
                                Steps.PrepImage = fileNameSteps1;
                            }

                            // Save or update Steps data to the database
                            if (Steps.PreparationsFK == stepsID) // int stepsID = PrepaVM.PreparationVM.التحضير_ID;
                            {
                                var existingSteps = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == Steps.PrepStepsID, incloudeProperties: "Preparation");

                                if (existingSteps != null)
                                {

                                    existingSteps.PrepText = Steps.PrepText;
                                    existingSteps.PrepImage = Steps.PrepImage;
                                    existingSteps.PrepStepsNum = Steps.PrepStepsNum;

                                    _unitOfWork.StepsPreparationRepository.Update(existingSteps);
                                }
                                else
                                {
                                    _unitOfWork.StepsPreparationRepository.Add(Steps);
                                }
                                _unitOfWork.Save();
                            }
                        }
                    }
                }
                TempData["success"] = "تم إضافة الخطوات بشكل ناجح";
                return RedirectToAction("RedirectToCreateSteps", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================

        // كود جلب اخر ID 
        [HttpGet]
        public IActionResult GetLastId()
        {
            try
            {
                int lastId = _unitOfWork.StepsPreparationRepository.GetLastStepId();
                return Ok(lastId);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return StatusCode(500, ex.Message);
            }
        }

        //=========================================POST Add ID Preparation ===================================
        [HttpPost]
        public IActionResult GetAddID(int preparationFk, LoginTredMarktViewModel PrepaVM)
        {
            // Fetch the production and steps associated with the given ProductionFK
            PrepaVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == preparationFk);
            PrepaVM.stepsVM = _unitOfWork.StepsPreparationRepository.GetAll()
                .Where(c => c.PreparationsFK == preparationFk).ToList();

            // Create a new step
            var newStep = new PreparationSteps
            {
                PreparationsFK = preparationFk,
            };

            // Save the new step to the database
            _unitOfWork.StepsPreparationRepository.Add(newStep);
            _unitOfWork.Save();

            // Return the new step's ID
            return Json(newStep.PrepStepsID);
        }
        //============================================================================

        // تبع List 
        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            var preparations = _unitOfWork.PreparationRepository.GetAll(incloudeProperties: "componentsCountPrint,toolsCountPrint")
          .Where(u => u.BrandFK == id).OrderBy(item => item.PreparationsOrder).ToList();

            return Json(new { data = preparations });
        }
        #endregion
        //============================================================================

        // زر الحذف تبع المكونات 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Delete(int? id) //this is for delete button in rows component 
        {
            var ComponentDelete = _unitOfWork.ComponentRepository.Get(u => u.PrepIngredientsID == id);
            int PreparationFk = ComponentDelete.PreparationsFK;
            var BrandFKE = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationFk);
            int? BranFK = BrandFKE.BrandFK;
            if (ComponentDelete == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.ComponentRepository.Remove(ComponentDelete);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToComponent", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion 
        //============================================================================

        //تبع الحذف في صفحة الإضافة : 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Deletec2(int? id) //this is for delete button in rows component 
        {
            var ComponentDelete = _unitOfWork.ComponentRepository.Get(u => u.PrepIngredientsID == id);
            int PreparationFk = ComponentDelete.PreparationsFK;
            var BrandFKE = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationFk);
            int? BranFK = BrandFKE.BrandFK;
            if (ComponentDelete == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.ComponentRepository.Remove(ComponentDelete);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateComponent", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion
        //============================================================================

        //زر حذف صفحة تعديل الأدوات 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult DeleteToolVariety(int? id) //this is for delete button in rows أدوات التحضير والصنف
        {

            var toolsVarityDelete = _unitOfWork.PrepaToolsVarietyRepository.Get(u => u.PrepToolsID == id);
            int PreparationFk = toolsVarityDelete.PreparationsFK;
            var BrandFKEx = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationFk);
            int? BranFK = BrandFKEx.BrandFK;

            if (toolsVarityDelete == null)
            {

                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.PrepaToolsVarietyRepository.Remove(toolsVarityDelete);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToTools", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion
        //============================================================================

        // تبع الحذف في صفحة الإضافة : 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult DeleteToolVarietyT1(int? id) //this is for delete button in rows أدوات التحضير والصنف
        {

            var toolsVarityDelete = _unitOfWork.PrepaToolsVarietyRepository.Get(u => u.PrepToolsID == id);
            int PreparationFk = toolsVarityDelete.PreparationsFK;
            var BrandFKEx = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationFk);
            int? BranFK = BrandFKEx.BrandFK;

            if (toolsVarityDelete == null)
            {

                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.PrepaToolsVarietyRepository.Remove(toolsVarityDelete);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateTools", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion
        //============================================================================

        ////زر الحذف تبع صفحة تعديل الخطوات
        #region API CALLS
        public IActionResult Deletesteps(int? id)
        {
            var stepsToDelete = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == id);
            var BrandFK = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == stepsToDelete.PreparationsFK);

            string IDStep = stepsToDelete.PrepStepsID.ToString();
            //string FKBrand = BrandFK.BrandFK.ToString();

            //عشان أوجهه لصفحة التعديل 
            int PreparationFk = stepsToDelete.PreparationsFK;
            int? BranFK = BrandFK.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            if (stepsToDelete == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            // Delete the associated image file
            if (!string.IsNullOrEmpty(stepsToDelete.PrepImage))
            {
                string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", IDStep, stepsToDelete.PrepImage);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.StepsPreparationRepository.Remove(stepsToDelete);
            _unitOfWork.Save();

            // Find all steps with a higher PrepStepsNum
            var preparationFK = stepsToDelete.PreparationsFK;

            var subsequentSteps = _unitOfWork.StepsPreparationRepository
                .GetAll(incloudeProperties: "Preparation").Where(u => u.PreparationsFK == preparationFK).ToList(); // Add ToList() to materialize the query;

            // Decrement PrepStepsNum for each subsequent step
            for (int i = 0; i < subsequentSteps.Count; i++)
            {
                var step = subsequentSteps[i];

                if (step.PrepStepsID > id)
                {
                    var getOld = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == step.PrepStepsID);
                    getOld.PrepStepsNum -= 1;
                    _unitOfWork.StepsPreparationRepository.Update(getOld);
                }
            }
            _unitOfWork.Save();

            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToSteps", new { PreparationID = PreparationFk, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 

        }
        #endregion
        //============================================================================

        ////زر الحذف تبع صفحة إضافة الخطوات
        #region API CALLS
        public IActionResult Deletesteps2(int? id)
        {
            var stepsToDelete = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == id);
            var BrandFK = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == stepsToDelete.PreparationsFK);

            string IDStep = stepsToDelete.PrepStepsID.ToString();
            //string FKBrand = BrandFK.BrandFK.ToString();

            //عشان أوجهه لصفحة التعديل 
            int PreparationFk = stepsToDelete.PreparationsFK;
            int? BranFK = BrandFK.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            if (stepsToDelete == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            // Delete the associated image file
            if (!string.IsNullOrEmpty(stepsToDelete.PrepImage))
            {
                string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", IDStep, stepsToDelete.PrepImage);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.StepsPreparationRepository.Remove(stepsToDelete);
            _unitOfWork.Save();

            // Find all steps with a higher PrepStepsNum
            var preparationFK = stepsToDelete.PreparationsFK;

            var subsequentSteps = _unitOfWork.StepsPreparationRepository
                .GetAll(incloudeProperties: "Preparation").Where(u => u.PreparationsFK == preparationFK).ToList(); // Add ToList() to materialize the query;

            // Decrement PrepStepsNum for each subsequent step
            for (int i = 0; i < subsequentSteps.Count; i++)
            {
                var step = subsequentSteps[i];

                if (step.PrepStepsID > id)
                {
                    var getOld = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == step.PrepStepsID);
                    getOld.PrepStepsNum -= 1;
                    _unitOfWork.StepsPreparationRepository.Update(getOld);
                }
            }
            _unitOfWork.Save();

            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateSteps", new { PreparationID = PreparationFk, brandFk = BranFK }) });
            //أحتاج يرجع لنفس صفحة التعديل 

        }
        #endregion
        //============================================================================

        ////زر الحذف في صفحة قائمة التحضيرات 
        #region
        //[HttpDelete]
        public IActionResult DeletePreparationPost(int? id)
        {
            var DeleteTools = _unitOfWork.PrepaToolsVarietyRepository.GetAll().Where(u => u.PreparationsFK == id).ToList();
            _unitOfWork.PrepaToolsVarietyRepository.RemoveRange(DeleteTools);
            _unitOfWork.Save();


            var DelteComponent = _unitOfWork.ComponentRepository.GetAll().Where(u => u.PreparationsFK == id).ToList();
            _unitOfWork.ComponentRepository.RemoveRange(DelteComponent);
            _unitOfWork.Save();

            var Deletesteps = _unitOfWork.StepsPreparationRepository.GetAll().Where(u => u.PreparationsFK == id).ToList();
            if (Deletesteps != null)
            {
                for (int i = 0; i < Deletesteps.Count; i++)
                {
                    var delet = Deletesteps[i];
                    var BrandId = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == delet.PreparationsFK);
                    var IDstep = _unitOfWork.StepsPreparationRepository.Get(u => u.PrepStepsID == delet.PrepStepsID);

                    string IDStep = IDstep.PrepStepsID.ToString();

                    if (!string.IsNullOrEmpty(delet.PrepImage))
                    {
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "IMAGES", IDStep, delet.PrepImage);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    _unitOfWork.StepsPreparationRepository.Remove(delet);
                    _unitOfWork.Save();

                }
            }
            var DeleteoneOflist = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == id);
            int PreparaionID = DeleteoneOflist.PreparationsID;
            //عشان أوجهه لصفحة List 
            int? FKBrandToRedyrect1 = DeleteoneOflist.BrandFK;

            if (DeleteoneOflist == null)
            {

                return Json(new { success = false, Message = "Error While Deleting" });
            }
            string IDStep2 = DeleteoneOflist.PreparationsID.ToString();

            if (!string.IsNullOrEmpty(DeleteoneOflist.prepareImage))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "IMAGES", IDStep2, DeleteoneOflist.prepareImage);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.PreparationRepository.Remove(DeleteoneOflist);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToPreparation", new { PreparationID = PreparaionID, brandFk = FKBrandToRedyrect1 }) }); //أحتاج يرجع لنفس صفحة التعديل 

        }
        #endregion
        //============================================================================
        public IActionResult PrintPreparation(int PreparationFK, int brandfk, int mainSectionId)
        {
            var model = new LoginTredMarktViewModel
            {
                PreparationVM = new Preparations(),
                PreparationList = new List<Preparations>(),
                PreparationsTools = new List<PreparationTools>(),
                stepsVM = new List<PreparationSteps>(),
                componontVMList = new List<PreparationIngredients>(),
                TredMarktVM = new Brands(),
                MainsectionVM = new MainSections(),
            };

            model.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandfk);
            model.MainsectionVM = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == mainSectionId);
            model.PreparationListCount = _unitOfWork.PreparationRepository.GetAll(incloudeProperties: "componentsCountPrint,toolsCountPrint,stepsCountPrint").Where(u => u.BrandFK == brandfk).ToList();
            model.stepsVM = _unitOfWork.StepsPreparationRepository.GetAll().Where(c => c.PreparationsFK == PreparationFK).ToList();
            model.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandfk).ToList();

            return new ViewAsPdf("PrintPreparation", model)
            {
                FileName = Uri.EscapeDataString("دليل الوصفات - " + model.TredMarktVM.BrandName + ".pdf"),
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0), // Adjust margins as needed
            };
        }
        //============================================================================

    }
}



