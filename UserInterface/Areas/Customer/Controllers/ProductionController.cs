﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.Production;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;


namespace UserInterface.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private bool isFirstComponentAdded = false;


        public ProductionController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;

        }

        //للإرسال FK الى صفحة القائمة بدون رقم ف URL
        public IActionResult RedirectToProduction(int brandFK)
        {
            HttpContext.Session.SetInt32("BrandFK", brandFK);

            TempData.Keep("BrandFK");
            return RedirectToAction("ProductionList");
        }
        public IActionResult ProductionList() //this for display List Of التحضيرات Page1
        {
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");

            TempData.Keep("BrandFK"); // Keep the TempData for further use

            LoginTredMarktViewModel PrVM = new LoginTredMarktViewModel
            {
                itemList33333 = _unitOfWork.itemsRepository.GetAll()
                            .Where(u => u.BrandFK == brandFK).OrderBy(item => item.ProductionOrder).ToList(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()

            };
            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();

            // Store the FK value in TempData
            TempData["ID"] = brandFK;
            // Display the updated list
            return View(PrVM);
        }
        //============================================================================

        //الانتقال الى صفحة المعلومات1
        public IActionResult RedirectToInormation1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Informations1");
        }
        //الانتقال الى صفحة المعلومات2
        public IActionResult Informations1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                componontVMList2 = new List<ProductionIngredients>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                stepsVM2List = new List<ProductionSteps>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.componontVMList2 = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            PrVM.ToolsVarityVM2List = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول الأدوات واللي يساعده على العرض هي viewD
            PrVM.stepsVM2List = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول الأدوات واللي يساعده على العرض هي viewD
            return View(PrVM);
        }

        [HttpPost] //This for Add Or Update Page . 
        public async Task<IActionResult> Informations1(LoginTredMarktViewModel PropaVM, IFormFile? file) // should insert name in Upsert view
        {
            if (ModelState.IsValid)
            {

                string ProductionID = PropaVM.Productionvm.ProductionID.ToString();
                string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Construct the folder path where the image will be saved
                    string folderPath = Path.Combine(wwwRootPath, "IMAGES", ProductionID);
                    string ProductionPath = Path.Combine(folderPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(PropaVM.Productionvm.ProductImage))
                    {
                        var oldImagePath = Path.Combine(folderPath, PropaVM.Productionvm.ProductImage);

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the image with the new file name
                    using (var fileStream = new FileStream(Path.Combine(ProductionPath), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Store only the file name in the database
                    PropaVM.Productionvm.ProductImage = fileName;
                }

                _unitOfWork.itemsRepository.Update(PropaVM.Productionvm); // تحديث Product
                _unitOfWork.Save();
                TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                return RedirectToAction("RedirectToInormation1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //============================================================================

        //الانتقال الى صفحة المكونات1
        public IActionResult RedirectToComponent1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Components1");
        }
        //الانتقال الى صفحة المكونات2
        public IActionResult Components1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                componontVMList2 = new List<ProductionIngredients>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                stepsVM2List = new List<ProductionSteps>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.componontVMList2 = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            return View(PrVM);
        }
        [HttpPost] //This for Add Or Update Page . 
        public IActionResult Components1(LoginTredMarktViewModel PropaVM) // should insert name in Upsert view
        {
            if (ModelState.IsValid)
            {

                int productionID = PropaVM.Productionvm.ProductionID;
                string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                if (PropaVM.componontVMList2 != null) // تحديث المكونات
                {
                    for (int i = 0; i < PropaVM.componontVMList2.Count; i++)
                    {
                        var Components = PropaVM.componontVMList2[i];

                        //int lastIdComponents = _unitOfWork.ComponentRepository2.GetLastComponentId();
                        //int LastId1Components = lastIdComponents + 1;

                        var existingComponent = _unitOfWork.ComponentRepository2.Get(u => u.ProdIngredientsID == Components.ProdIngredientsID, incloudeProperties: "Production");
                        if (existingComponent == null)
                        {
                            var newComponent = new ProductionIngredients
                            {
                                //ProdIngredientsID = LastId1Components,
                                ProductionFK = productionID,
                                ProdQuantity = Components.ProdQuantity,
                                ProdUnit = Components.ProdUnit,
                                ProdIngredientsName = Components.ProdIngredientsName
                            };
                            _unitOfWork.ComponentRepository2.Add(newComponent);
                            _unitOfWork.Save();

                        }
                        else
                        {
                            existingComponent.ProdQuantity = Components.ProdQuantity;
                            existingComponent.ProdUnit = Components.ProdUnit;
                            existingComponent.ProdIngredientsName = Components.ProdIngredientsName;

                            _unitOfWork.ComponentRepository2.Update(existingComponent);
                            _unitOfWork.Save();
                        }
                    }
                }

                TempData["success"] = "تم تحديث المكونات بشكل ناجح";
                return RedirectToAction("RedirectToComponent1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //============================================================================

        //الانتقال الى الأدوات1
        public IActionResult RedirectToTools1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Tools1");
        }
        //الانتقال الى صفحة الأدوات2
        public IActionResult Tools1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                componontVMList2 = new List<ProductionIngredients>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                stepsVM2List = new List<ProductionSteps>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.ToolsVarityVM2List = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول الأدوات واللي يساعده على العرض هي viewD
            return View(PrVM);
        }
        [HttpPost] //This for Add Or Update Page . 
        public IActionResult Tools1(LoginTredMarktViewModel PropaVM) // should insert name in Upsert view
        {
            if (ModelState.IsValid)
            {

                int productionID = PropaVM.Productionvm.ProductionID;
                string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                if (PropaVM.ToolsVarityVM2List != null) //تحديث الأدوات.
                {
                    for (int i = 0; i < PropaVM.ToolsVarityVM2List.Count; i++)
                    {
                        var Tools = PropaVM.ToolsVarityVM2List[i];

                        //int lastIdTools = _unitOfWork.PrepaToolsVarietyRepository2.GetLastToolsId();
                        //int LastId1Tools = lastIdTools + 1;

                        var existingtoolvariety = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProdToolsID == Tools.ProdToolsID, incloudeProperties: "Production");
                        if (existingtoolvariety == null)
                        {
                            var firstRowToolAdd = new ProductionTools
                            {
                                //ProdToolsID = LastId1Tools,
                                ProductionFK = productionID,
                                ProdTools = Tools.ProdTools,
                            };
                            _unitOfWork.PrepaToolsVarietyRepository2.Add(firstRowToolAdd);
                            _unitOfWork.Save();
                        }
                        else //if is exit from database
                        {
                            existingtoolvariety.ProdToolsID = Tools.ProdToolsID;
                            existingtoolvariety.ProdTools = Tools.ProdTools;
                            _unitOfWork.PrepaToolsVarietyRepository2.Update(existingtoolvariety);
                            _unitOfWork.Save();
                        }
                    }
                }

                TempData["success"] = "تم تحديث الأدوات بشكل ناجح";
                return RedirectToAction("RedirectToTools1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //============================================================================

        //الانتقال الى الخطوات1
        public IActionResult RedirectToSteps1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("Steps1");
        }
        //الانتقال الى صفحة الخطوات2
        public IActionResult Steps1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                componontVMList2 = new List<ProductionIngredients>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                stepsVM2List = new List<ProductionSteps>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.stepsVM2List = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول الأدوات واللي يساعده على العرض هي viewD
            return View(PrVM);
        }

        [HttpPost] //This for Add Or Update Page . 
        public async Task<IActionResult> Steps1(LoginTredMarktViewModel PropaVM) // should insert name in Upsert view
        {
            if (ModelState.IsValid)
            {
                int stepsID = PropaVM.Productionvm.ProductionID;
                int productionID = PropaVM.Productionvm.ProductionID;
                string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                if (PropaVM.stepsVM2List != null)
                {
                    for (int i = 0; i < PropaVM.stepsVM2List.Count; i++)
                    {
                        var Steps = PropaVM.stepsVM2List[i];

                        string wwwRootPathSteps = _webHostEnvironment.WebRootPath; // get the root folder

                        //int LastId = _unitOfWork.StepsPreparationRepository2.GetLastStepId();
                        //int LastId1 = LastId + 1;

                        var existingSteps9 = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == Steps.ProdStepsID, incloudeProperties: "Production");
                        if (existingSteps9 == null)
                        {
                            var newStep = new ProductionSteps
                            {
                                //ProdStepsID = LastId1,
                                ProductionFK = Steps.ProductionFK,
                                ProdText = Steps.ProdText,
                                ProdStepsNum = Steps.ProdStepsNum

                            };

                            string IDstep = newStep.ProdStepsID.ToString();
                            string ProductionVMFk = PropaVM.Productionvm.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{newStep.ProdStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.ProdSImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", IDstep, newStep.ProdSImage);

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

                                newStep.ProdSImage = fileNameSteps1; // Update the image path
                                _unitOfWork.StepsPreparationRepository2.Add(newStep);
                                _unitOfWork.Save();
                            }
                        }
                        else
                        {
                            string IDstep = Steps.ProdStepsID.ToString();
                            string ProductionVMFk = PropaVM.Productionvm.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{Steps.ProdStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.ProdSImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", IDstep, Steps.ProdSImage);

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
                                Steps.ProdSImage = fileNameSteps1;
                            }

                            // Save or update Steps data to the database
                            if (Steps.ProductionFK == stepsID) // int stepsID = PrepaVM.PreparationVM.التحضير_ID;
                            {
                                var existingSteps = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == Steps.ProdStepsID, incloudeProperties: "Production");

                                if (existingSteps != null)
                                {

                                    existingSteps.ProdText = Steps.ProdText;
                                    existingSteps.ProdSImage = Steps.ProdSImage;
                                    existingSteps.ProdStepsNum = Steps.ProdStepsNum;

                                    _unitOfWork.StepsPreparationRepository2.Update(existingSteps);
                                }
                                else
                                {
                                    _unitOfWork.StepsPreparationRepository2.Add(Steps);
                                }
                                _unitOfWork.Save();
                            }
                        }
                    }
                }

                TempData["success"] = "تم تحديث الخطوات بشكل ناجح";
                return RedirectToAction("RedirectToSteps1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //============================================================================

        //========================================صفحات الإنشاء ====================================

        //صفحة انشاء المعلومات
        public IActionResult RedirectToCreateInformations1(int? ProductionID, int brandFK)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFK);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateInformations1");
        }
        public IActionResult CreateInformations1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");

            TempData.Keep("BrandFK"); // Keep the TempData for further use
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                componontVMList2 = new List<ProductionIngredients>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                itemList33333 = new List<Production>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()


            };
            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();
            if (ProductionID == 0 || ProductionID == null)
            {
                PrVM.Productionvm = new Production();
            }
            else
            {
                PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);

            }
            PrVM.componontVMList2 = new List<ProductionIngredients>();
            PrVM.ToolsVarityVM2List = new List<ProductionTools>();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.itemsList = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK);

            PrVM.itemList33333 = new List<Production>()
            {
         new Production { ProductType = "الأطباق الرئيسية" },
         new Production { ProductType = "المشروبات الساخنة" },
         new Production { ProductType = "المشروبات الباردة" },
         new Production { ProductType = "المقبلات الباردة" },
         new Production { ProductType = "الفطور" },
         new Production { ProductType = " الحلى" },
         new Production { ProductType = "الأطباق الجانبية" },
         new Production { ProductType = "وجبات الأطفال" },
         new Production { ProductType = "السلطات" },
         new Production { ProductType = "الشوربات" },
         new Production { ProductType = "الموهيتو" }

            };

            return View(PrVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreateInformations1(LoginTredMarktViewModel PropaVM, IFormFile? file, int selectedValue, int selectPreparation1) 
        {
            if (ModelState.IsValid)
            {
                var FKProduct = PropaVM.TredMarktVM.BrandID;

                var type = _unitOfWork.itemsRepository.Get(u => u.ProductionID == selectPreparation1);
                var exitingProduction = _unitOfWork.itemsRepository.Get(u => u.ProductionID == PropaVM.Productionvm.ProductionID);

                if (exitingProduction == null)  // if Add 
                {
                    var setFK = new Production
                    {
                        BrandFK = FKProduct,
                        ProductName = PropaVM.Productionvm.ProductName,
                        PreparationTime = PropaVM.Productionvm.PreparationTime,
                        VersionNumber = PropaVM.Productionvm.VersionNumber,
                        Expiry = PropaVM.Productionvm.Expiry,
                        ProductType = PropaVM.Productionvm.ProductType,
                        Station = PropaVM.Productionvm.Station,

                    };
                    _unitOfWork.itemsRepository.Add(setFK);
                    _unitOfWork.Save();

                    PropaVM.Productionvm.ProductionID = setFK.ProductionID;
                    //this code for image if add or update.
                    string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                    if (file != null)
                    {

                        // Convert numeric values to strings
                        string ProductionID = setFK.ProductionID.ToString(); // Convert to string
                        string ProductionVMFK = PropaVM.TredMarktVM.BrandID.ToString(); // Convert to string

                        // Combine paths using Path.Combine, ensuring all arguments are strings 
                        string ProductionDirectory = Path.Combine(wwwRootPath, "IMAGES", ProductionID);

                        //اذا المسار مش موجود سو مسار جديد 
                        if (!Directory.Exists(ProductionDirectory))
                        {
                            Directory.CreateDirectory(ProductionDirectory);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        string ProductionPath = Path.Combine(ProductionDirectory, fileName);

                        // Use the correct file path when creating FileStream
                        using (var stream = new FileStream(ProductionPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        setFK.ProductImage = fileName; // Save only the file name in the database
                        _unitOfWork.Save();
                    }
                    //// reOrder2 
                    if (selectedValue == 0)
                    {
                        int IdProduction = setFK.ProductionID;
                        setFK.ProductionOrder = IdProduction;
                        //// Get the maximum order value in the existing list
                        //double maxOrder = _unitOfWork.itemsRepository.GetAll()
                        //    .Max(item => item.ProductionOrder) ?? 0.0f; // Default to 0.0f if there are no existing items

                        //// Round down the maxOrder value to the nearest integer
                        //int maxOrderAsInt = (int)Math.Floor(maxOrder);

                        //// Set the new order value for the "اخرى" (Other) item
                        //double newOrder = maxOrderAsInt + 1.0f;
                        //setFK.ProductionOrder = newOrder;
                    }
                    else
                    {
                        var getIdOrder = _unitOfWork.itemsRepository.Get(u => u.ProductionID == selectedValue);
                        double? OldOrder = getIdOrder.ProductionOrder; // Default to 0.0f if Order is null
                        double? newOrder = OldOrder + 0.1;
                        setFK.ProductionOrder = newOrder;
                    }

                    _unitOfWork.Save();
                    TempData["success"] = "تم إضافة الإنتاج بشكل ناجح";
                }
                else
                {
                    string ProductionID = PropaVM.Productionvm.ProductionID.ToString();
                    string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                        // Construct the folder path where the image will be saved
                        string folderPath = Path.Combine(wwwRootPath, "IMAGES", ProductionID);
                        string ProductionPath = Path.Combine(folderPath, fileName);

                        // Ensure the directory exists
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(PropaVM.Productionvm.ProductImage))
                        {
                            var oldImagePath = Path.Combine(folderPath, PropaVM.Productionvm.ProductImage);

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save the image with the new file name
                        using (var fileStream = new FileStream(Path.Combine(ProductionPath), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Store only the file name in the database
                        PropaVM.Productionvm.ProductImage = fileName;
                    }

                    _unitOfWork.itemsRepository.Update(PropaVM.Productionvm); // تحديث Product
                    _unitOfWork.Save();
                    TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                }
                return RedirectToAction("RedirectToCreateInformations1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.TredMarktVM.BrandID });

            }
            return View(PropaVM);
        }
        //============================================================================

        //صفحة إنشاء المكونات 
        public IActionResult RedirectToCreateComponent1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateComponent1");
        }
        //الانتقال الى صفحة المكونات2
        public IActionResult CreateComponent1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                componontVM2 = new ProductionIngredients(),
                componontVMList2 = new List<ProductionIngredients>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.componontVM2 = _unitOfWork.ComponentRepository2.Get(u => u.ProductionFK == ProductionID);
            PrVM.componontVMList2 = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            return View(PrVM);
        }
        [HttpPost]
        public IActionResult CreateComponent1(LoginTredMarktViewModel PropaVM)
        {
            int ProductionFK = PropaVM.Productionvm.ProductionID;

            if (ModelState.IsValid)
            {
                //int lastIdComponents = _unitOfWork.ComponentRepository2.GetLastComponentId();
                //int LastId1Components = lastIdComponents + 1;

                if (PropaVM.componontVM2 != null)
                {
                    var firstComponent = new ProductionIngredients
                    {
                        //ProdIngredientsID = LastId1Components,
                        ProductionFK = ProductionFK,
                        ProdIngredientsName = PropaVM.componontVM2.ProdIngredientsName, // Retrieve data from form
                        ProdUnit = PropaVM.componontVM2.ProdUnit,
                        ProdQuantity = PropaVM.componontVM2.ProdQuantity
                    };

                    _unitOfWork.ComponentRepository2.Add(firstComponent);
                    _unitOfWork.Save();
                }
                if (PropaVM.componontVMList2 != null && PropaVM.componontVMList2.Any())
                { // if condition checks whether the PropaVM.componontVMList is not null and contains at least one item. 
                    for (int i = 0; i < PropaVM.componontVMList2.Count; i++)
                    {
                        var Components = PropaVM.componontVMList2[i];
                        //int lastIdComponents1 = _unitOfWork.ComponentRepository2.GetLastComponentId();
                        //int LastId1Components1 = lastIdComponents + 1;

                        var existingComponent = _unitOfWork.ComponentRepository2.Get(u => u.ProdIngredientsID == Components.ProdIngredientsID, incloudeProperties: "Production");
                        if (existingComponent == null)
                        {
                            //LastId1Components++;
                            //int componentId = PropaVM.Productionvm.ProductionID;

                            var newComponent = new ProductionIngredients
                            {
                                //ProdIngredientsID = LastId1Components,
                                ProductionFK = ProductionFK,
                                ProdQuantity = Components.ProdQuantity,
                                ProdUnit = Components.ProdUnit,
                                ProdIngredientsName = Components.ProdIngredientsName
                            };
                            _unitOfWork.ComponentRepository2.Add(newComponent);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            existingComponent.ProdQuantity = Components.ProdQuantity;
                            existingComponent.ProdUnit = Components.ProdUnit;
                            existingComponent.ProdIngredientsName = Components.ProdIngredientsName;

                            _unitOfWork.ComponentRepository2.Update(existingComponent);
                            _unitOfWork.Save();
                        }
                    }
                }
                _unitOfWork.Save();
                TempData["success"] = "تم إضافة المكونات بشكل ناجح";
                return RedirectToAction("RedirectToCreateComponent1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.TredMarktVM.BrandID });
            }
            return View(PropaVM);
        }
        //============================================================================

        //صفحة إنشاء الادوات 
        public IActionResult RedirectToCreateTools1(int? ProductionID, int? brandFk)
        {
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateTools1");
        }
        //الانتقال الى صفحة الأدوات2
        public IActionResult CreateTools1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                ToolsVarityVM2 = new ProductionTools(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.ToolsVarityVM2 = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProductionFK == ProductionID);
            PrVM.ToolsVarityVM2List = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList();

            return View(PrVM);
        }

        [HttpPost]
        public IActionResult CreateTools1(LoginTredMarktViewModel PropaVM)
        {
            int ProductionFK = PropaVM.Productionvm.ProductionID;

            if (ModelState.IsValid)
            {
                //int lastIdTools = _unitOfWork.PrepaToolsVarietyRepository2.GetLastToolsId();
                //int LastId1Tools = lastIdTools + 1;
                if (PropaVM.ToolsVarityVM2 != null)
                {
                    var firstRowToolAdd = new ProductionTools
                    {
                        //ProdToolsID = LastId1Tools,
                        ProductionFK = ProductionFK,
                        ProdTools = PropaVM.ToolsVarityVM2.ProdTools,
                    };
                    _unitOfWork.PrepaToolsVarietyRepository2.Add(firstRowToolAdd);
                    _unitOfWork.Save();
                }

                if (PropaVM.ToolsVarityVM2List != null && PropaVM.ToolsVarityVM2List.Any())
                {
                    for (int i = 0; i < PropaVM.ToolsVarityVM2List.Count; i++)
                    {
                        var Tools = PropaVM.ToolsVarityVM2List[i];

                        //int lastIdTools1 = _unitOfWork.PrepaToolsVarietyRepository2.GetLastToolsId();
                        //int LastId1Tools1 = lastIdTools1 + 1;

                        var existingtoolvariety = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProdToolsID == Tools.ProdToolsID, incloudeProperties: "Production");
                        if (existingtoolvariety == null)
                        {
                            var firstRowToolAdd = new ProductionTools
                            {
                                //ProdToolsID = LastId1Tools1,
                                ProductionFK = ProductionFK,
                                ProdTools = Tools.ProdTools,
                            };
                            _unitOfWork.PrepaToolsVarietyRepository2.Add(firstRowToolAdd);
                            _unitOfWork.Save();
                        }
                        else //if is exit from database
                        {
                            existingtoolvariety.ProdToolsID = Tools.ProdToolsID;
                            existingtoolvariety.ProdTools = Tools.ProdTools;
                            _unitOfWork.PrepaToolsVarietyRepository2.Update(existingtoolvariety);
                            _unitOfWork.Save();
                        }
                    }
                }

                _unitOfWork.Save();
                TempData["success"] = "تم إضافة الأدوات بشكل ناجح";
                return RedirectToAction("RedirectToCreateTools1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.TredMarktVM.BrandID });
            }
            return View(PropaVM);
        }
        //============================================================================

        //صفحة إنشاء الخطوات 
        public IActionResult RedirectToCreateSteps1(int? ProductionID, int? brandFk)
        {
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateSteps1");
        }
        //الانتقال الى صفحة الخطوات2
        public IActionResult CreateSteps1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                TredMarktVM = new Brands(),
                stepsVM2 = new ProductionSteps(),
                stepsVM2List = new List<ProductionSteps>(),
                welcomTredmarketProduction = new LoginTredMarktViewModel()
            };

            PrVM.welcomTredmarketProduction.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.welcomTredmarketProduction.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFk);
            PrVM.welcomTredmarketProduction.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFk).ToList();
            PrVM.welcomTredmarketProduction.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFk).ToList();
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.stepsVM2 = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProductionFK == ProductionID);
            PrVM.stepsVM2List = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList();

            return View(PrVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSteps1(LoginTredMarktViewModel PropaVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var FKProduct = PropaVM.TredMarktVM.BrandID;
                string wwwRootPath = _webHostEnvironment.WebRootPath; // get us root folder

                if (PropaVM.stepsVM2List != null)
                {
                    for (int i = 0; i < PropaVM.stepsVM2List.Count; i++)
                    {
                        var Steps = PropaVM.stepsVM2List[i];

                        //int LastId = _unitOfWork.StepsPreparationRepository2.GetLastStepId();
                        //int LastId1 = LastId + 1;

                        var existingSteps9 = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == Steps.ProdStepsID, incloudeProperties: "Production");
                        if (existingSteps9 == null)
                        {
                            var newStep = new ProductionSteps
                            {
                                //ProdStepsID = LastId1,
                                ProductionFK = Steps.ProductionFK,
                                ProdText = Steps.ProdText,
                                ProdStepsNum = Steps.ProdStepsNum

                            };

                            string IDstep = newStep.ProdStepsID.ToString();
                            string ProductionVMFk = PropaVM.Productionvm.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{newStep.ProdStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.ProdSImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPath, "IMAGES", IDstep, newStep.ProdSImage);

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

                                newStep.ProdSImage = fileNameSteps1; // Update the image path
                                _unitOfWork.StepsPreparationRepository2.Add(newStep);
                                _unitOfWork.Save();
                            }
                        }
                        else
                        {
                            string IDstep = Steps.ProdStepsID.ToString();
                            int IDsteps = PropaVM.Productionvm.ProductionID;
                            string ProductionVMFk = PropaVM.Productionvm.BrandFK.ToString();

                            string StepsPath = Path.Combine(wwwRootPath, "IMAGES", IDstep);

                            var file1Name = $"file1_{Steps.ProdStepsID}";
                            var file1ForStep = HttpContext.Request.Form.Files[file1Name];

                            if (file1ForStep != null)
                            {
                                if (!string.IsNullOrEmpty(Steps.ProdSImage)) // Check if there's an existing image path
                                {
                                    var OldImagePath1 = Path.Combine(wwwRootPath, "IMAGES", IDstep, Steps.ProdSImage);

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
                                Steps.ProdSImage = fileNameSteps1;
                            }

                            // Save or update Steps data to the database
                            if (Steps.ProductionFK == IDsteps) // int stepsID = PrepaVM.PreparationVM.التحضير_ID;
                            {
                                var existingSteps = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == Steps.ProdStepsID, incloudeProperties: "Production");

                                if (existingSteps != null)
                                {

                                    existingSteps.ProdText = Steps.ProdText;
                                    existingSteps.ProdSImage = Steps.ProdSImage;
                                    existingSteps.ProdStepsNum = Steps.ProdStepsNum;

                                    _unitOfWork.StepsPreparationRepository2.Update(existingSteps);
                                }
                                else
                                {
                                    _unitOfWork.StepsPreparationRepository2.Add(Steps);
                                }
                                _unitOfWork.Save();
                            }
                        }
                    }
                }
                TempData["success"] = "تم إضافة الخطوات بشكل ناجح";
                return RedirectToAction("RedirectToCreateSteps1", new { PropaVM.Productionvm.ProductionID, brandFk = PropaVM.TredMarktVM.BrandID });

            }
            return View(PropaVM);
        }
        //============================================================================

        //=========================================GET LAST ID ===================================
        [HttpGet]
        public IActionResult GetLastId()
        {
            try
            {
                int lastId = _unitOfWork.StepsPreparationRepository2.GetLastStepId();
                return Ok(lastId);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return StatusCode(500, ex.Message);
            }
        }
        //============================================================================
        //=========================================POST Add ID ===================================
        [HttpPost]
        public IActionResult GetAddID(int ProductionFK, LoginTredMarktViewModel PropaVM)
        {
            // Fetch the production and steps associated with the given ProductionFK
            PropaVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionFK);
            PropaVM.stepsVM2List = _unitOfWork.StepsPreparationRepository2
                .GetAll(incloudeProperties: "Production")
                .Where(c => c.ProductionFK == ProductionFK)
                .ToList();

            // Create a new step
            var newStep = new ProductionSteps
            {
                ProductionFK = ProductionFK,
            };

            // Save the new step to the database
            _unitOfWork.StepsPreparationRepository2.Add(newStep);
            _unitOfWork.Save();

            // Return the new step's ID
            return Json(newStep.ProdStepsID);
        }
        //============================================================================

        //زر حذف صفحة تعديل الأدوات2 
        #region API CALLS 
        public IActionResult DeleteToolVariety2(int? id) //this is for delete button in rows أدوات التحضير والصنف
        {
            var toolsVarityDelete = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProdToolsID == id);
            int ProductionFK = toolsVarityDelete.ProductionFK;
            var BrandFKEx = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionFK);
            int? BranFK = BrandFKEx.BrandFK;
            if (toolsVarityDelete == null)
            {

                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.PrepaToolsVarietyRepository2.Remove(toolsVarityDelete);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToTools1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion 
        //============================================================================
        //زر حذف صفحة إضافة الأدوات2 
        #region API CALLS 
        public IActionResult DeleteToolVariety2t1(int? id) //this is for delete button in rows أدوات التحضير والصنف
        {
            var toolsVarityDelete = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProdToolsID == id);
            int ProductionFK = toolsVarityDelete.ProductionFK;
            var BrandFKEx = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionFK);
            int? BranFK = BrandFKEx.BrandFK;
            if (toolsVarityDelete == null)
            {

                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.PrepaToolsVarietyRepository2.Remove(toolsVarityDelete);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateTools1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================

        // تبع List 
        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll(int? id)
        {

            IEnumerable<Production> objProductionList = _unitOfWork.itemsRepository.GetAll()
                .Where(u => u.BrandFK == id).OrderBy(item => item.ProductionOrder).OrderBy(item => item.ProductionOrder).ToList();

            return Json(new { data = objProductionList });
        }
        #endregion
        //============================================================================

        // 2زر الحذف تبع المكونات 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Delete(int? id) //this is for delete button in rows component 
        {
            var ComponentDelete2 = _unitOfWork.ComponentRepository2.Get(u => u.ProdIngredientsID == id);
            int ProductionFK = ComponentDelete2.ProductionFK;
            var BrandFKEx = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionFK);
            int? BranFK = BrandFKEx.BrandFK;

            if (ComponentDelete2 == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.ComponentRepository2.Remove(ComponentDelete2);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToComponent1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================

        // 2زر الحذف تبع المكونات "الإضافة"
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Deletec1(int? id) //this is for delete button in rows component 
        {
            var ComponentDelete2 = _unitOfWork.ComponentRepository2.Get(u => u.ProdIngredientsID == id);
            int ProductionFK = ComponentDelete2.ProductionFK;
            var BrandFKEx = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionFK);
            int? BranFK = BrandFKEx.BrandFK;

            if (ComponentDelete2 == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            _unitOfWork.ComponentRepository2.Remove(ComponentDelete2);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateComponent1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================

        //2زر الحذف في صفحة قائمة Product 
        #region
        //[HttpDelete]
        public IActionResult DeletePreparationPost(int? id)
        {
            var DeleteTools = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == id).ToList();
            _unitOfWork.PrepaToolsVarietyRepository2.RemoveRange(DeleteTools);


            var DelteComponent = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == id).ToList();
            _unitOfWork.ComponentRepository2.RemoveRange(DelteComponent);

            var Deletesteps = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == id).ToList();

            // Check if Deletesteps is not null
            if (Deletesteps != null)
            {
                for (int i = 0; i < Deletesteps.Count; i++)
                {
                    var delet = Deletesteps[i];

                    var BrandId = _unitOfWork.itemsRepository.Get(u => u.ProductionID == delet.ProductionFK);
                    var IDstep = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == delet.ProdStepsID);

                    string IDStep = IDstep.ProdStepsID.ToString();

                    // Delete the associated image files
                    if (!string.IsNullOrEmpty(delet.ProdSImage))
                    {
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "IMAGES", IDStep, delet.ProdSImage);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // Remove the entity from the repository
                    _unitOfWork.StepsPreparationRepository2.Remove(delet);
                    _unitOfWork.Save();
                }
            }

            var DeleteoneOflist = _unitOfWork.itemsRepository.Get(u => u.ProductionID == id);

            //عشان أوجهه لصفحة List 
            int? FKBrandToRedyrect1 = DeleteoneOflist.BrandFK;
            if (DeleteoneOflist == null)
            {

                return Json(new { success = false, Message = "خطأ أثناء الحذف " });
            }
            string IDStep2 = DeleteoneOflist.ProductionID.ToString();

            if (!string.IsNullOrEmpty(DeleteoneOflist.ProductImage))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "IMAGES", IDStep2, DeleteoneOflist.ProductImage);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.itemsRepository.Remove(DeleteoneOflist);
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToProduction", new { BrandFK = FKBrandToRedyrect1 }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================

        //زر الحذف تبع صفحة تعديل الخطوات
        #region API CALLS
        public IActionResult Deletestep(int? id)
        {
            var stepsToDelete = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == id);
            var BrandFK = _unitOfWork.itemsRepository.Get(u => u.ProductionID == stepsToDelete.ProductionFK);

            string IDStep = stepsToDelete.ProdStepsID.ToString();
            //string FKBrand = BrandFK.BrandFK.ToString();

            //أوجهه الى صفحة التعديل
            //عشان أوجهه لصفحة التعديل 
            int ProductionFK = stepsToDelete.ProductionFK;
            int? BranFK = BrandFK.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            if (stepsToDelete == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            // Delete the associated image file
            if (!string.IsNullOrEmpty(stepsToDelete.ProdSImage))
            {
                string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", IDStep, stepsToDelete.ProdSImage);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.StepsPreparationRepository2.Remove(stepsToDelete);
            _unitOfWork.Save();

            // Find all steps with a higher PrepStepsNum
            var preparationFK = stepsToDelete.ProductionFK;

            var subsequentSteps = _unitOfWork.StepsPreparationRepository2
                .GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == preparationFK).ToList(); // Add ToList() to materialize the query;

            // Decrement PrepStepsNum for each subsequent step
            for (int i = 0; i < subsequentSteps.Count; i++)
            {
                var step = subsequentSteps[i];

                if (step.ProdStepsID > id)
                {
                    var getOld = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == step.ProdStepsID);
                    getOld.ProdStepsNum -= 1;
                    _unitOfWork.StepsPreparationRepository2.Update(getOld);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToSteps1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //زر الحذف تبع صفحة إضافة الخطوات
        #region API CALLS
        public IActionResult Deletestep2(int? id)
        {
            var stepsToDelete = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == id);
            var BrandFK = _unitOfWork.itemsRepository.Get(u => u.ProductionID == stepsToDelete.ProductionFK);

            string IDStep = stepsToDelete.ProdStepsID.ToString();
            //string FKBrand = BrandFK.BrandFK.ToString();

            //أوجهه الى صفحة التعديل
            //عشان أوجهه لصفحة التعديل 
            int ProductionFK = stepsToDelete.ProductionFK;
            int? BranFK = BrandFK.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            if (stepsToDelete == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            // Delete the associated image file
            if (!string.IsNullOrEmpty(stepsToDelete.ProdSImage))
            {
                string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", IDStep, stepsToDelete.ProdSImage);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.StepsPreparationRepository2.Remove(stepsToDelete);
            _unitOfWork.Save();

            // Find all steps with a higher PrepStepsNum
            var preparationFK = stepsToDelete.ProductionFK;

            var subsequentSteps = _unitOfWork.StepsPreparationRepository2
                .GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == preparationFK).ToList(); // Add ToList() to materialize the query;

            // Decrement PrepStepsNum for each subsequent step
            for (int i = 0; i < subsequentSteps.Count; i++)
            {
                var step = subsequentSteps[i];

                if (step.ProdStepsID > id)
                {
                    var getOld = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == step.ProdStepsID);
                    getOld.ProdStepsNum -= 1;
                    _unitOfWork.StepsPreparationRepository2.Update(getOld);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.Save();
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateSteps1", new { ProductionID = ProductionFK, brandFk = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        public IActionResult PrintProduction(int ProductionFK, int brandfk, int mainSectionId)
        {
            var model = new LoginTredMarktViewModel
            {
                Productionvm = new Production(),
                ProductionLoginVMlist = new List<Production>(),
                ToolsVarityVM2List = new List<ProductionTools>(),
                stepsVM2List = new List<ProductionSteps>(),
                componontVMList2 = new List<ProductionIngredients>(),
                TredMarktVM = new Brands(),
                MainsectionVM = new MainSections(),
            };

            model.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandfk);
            model.MainsectionVM = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == mainSectionId);
            model.ProductionListCount = _unitOfWork.itemsRepository.GetAll(incloudeProperties: "component2,toolsCountPrint2,stepsCountPrint2").Where(u => u.BrandFK == brandfk).ToList();
            model.stepsVM2List = _unitOfWork.StepsPreparationRepository2.GetAll().Where(c => c.ProductionFK == ProductionFK).ToList();
            model.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandfk).ToList();

            return new ViewAsPdf("PrintProduction", model)
            {
                FileName = Uri.EscapeDataString("دليل الوصفات - " + model.TredMarktVM.BrandName + ".pdf"),
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0), // Adjust margins as needed
            };
        }
        //============================================================================

    }
}





