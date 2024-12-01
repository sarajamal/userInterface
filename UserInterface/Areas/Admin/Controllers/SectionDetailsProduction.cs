using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Production;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;

namespace UserInterface.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SectionDetailsProduction : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private bool isFirstComponentAdded = false;


        public SectionDetailsProduction(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;

        }
        //للإرسال FK الى صفحة القائمة بدون رقم ف URL
        public IActionResult RedirectToProductionAdmin(int brandFk)
        {
            HttpContext.Session.SetInt32("BrandFK", brandFk);
            TempData.Keep("BrandFK");
            return RedirectToAction("ProductionAdminList");
        }
        public IActionResult ProductionAdminList() //this for display List Of التحضيرات Page1
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

            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK); 

            // Store the FK value in TempData
            TempData["ID"] = brandFK;
            // Display the updated list
            return View(PrVM);
        }
        // تبع List ------------------------------------------------------------------
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
        //الانتقال الى صفحة المعلومات1-------------------------------------------------
        public IActionResult RedirectToUpdateAdminInformation1(int? ProductionID, int? brandFk)
        {

            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateAdminInformation1");
        }
        //الانتقال الى صفحة المعلومات2
        public IActionResult UpdateAdminInformation1() // After Enter تعديل Display التحضيرات والمكونات...
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
        public async Task<IActionResult> UpdateAdminInformation1(LoginTredMarktViewModel PropaVM, IFormFile? file) // should insert name in Upsert view
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

                _unitOfWork.itemsRepository.UpdateAdmin(PropaVM.Productionvm); // تحديث Product
                _unitOfWork.Save();
                TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                return RedirectToAction("RedirectToUpdateAdminInformation1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //============================================================================
        //الانتقال الى صفحة المكونات1--------------------------------------------------
        public IActionResult RedirectToUpdateAdminComponents1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateAdminComponents1");
        }
        //الانتقال الى صفحة المكونات2
        public IActionResult UpdateAdminComponents1() // After Enter تعديل Display التحضيرات والمكونات...
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
            PrVM.componontVMList2 = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").OrderBy(c => c.ProdIngredientsID).Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            return View(PrVM);
        }
        [HttpPost] //This for Add Or Update Page . 
        public IActionResult UpdateAdminComponents1(LoginTredMarktViewModel PropaVM) // should insert name in Upsert view
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
                return RedirectToAction("RedirectToUpdateAdminComponents1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        // 2زر الحذف تبع المكونات ---------------------------------------------------
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Delete404(int? id) //this is for delete button in rows component 
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateAdminComponents1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================

        //الانتقال الى الأدوات1
        public IActionResult RedirectToUpdateAdminTools1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateAdminTools1");
        }
        //الانتقال الى صفحة الأدوات2
        public IActionResult UpdateAdminTools1() // After Enter تعديل Display التحضيرات والمكونات...
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
            PrVM.ToolsVarityVM2List = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").OrderBy(c => c.ProdToolsID).Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول الأدوات واللي يساعده على العرض هي viewD
            return View(PrVM);
        }
        [HttpPost] //This for Add Or Update Page . 
        public IActionResult UpdateAdminTools1(LoginTredMarktViewModel PropaVM) // should insert name in Upsert view
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

                        var existingtoolvariety = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProdToolsID == Tools.ProdToolsID);
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
                return RedirectToAction("RedirectToUpdateAdminTools1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //زر حذف صفحة تعديل الأدوات2 
        #region API CALLS 
        public IActionResult DeleteToolVariety2404(int? id) //this is for delete button in rows أدوات التحضير والصنف
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateAdminTools1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //الانتقال الى الخطوات1--------------------------------------------------------
        public IActionResult RedirectToUpdateAdminSteps1(int? ProductionID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateAdminSteps1");
        }
        //الانتقال الى صفحة الخطوات2
        public IActionResult UpdateAdminSteps1() // After Enter تعديل Display التحضيرات والمكونات...
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
            PrVM.stepsVM2List = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).OrderBy(item => item.ProdStepsNum).ToList(); //هو يحتوي على قائمة من جدول الأدوات واللي يساعده على العرض هي viewD
            return View(PrVM);
        }

        [HttpPost] //This for Add Or Update Page . 
        public async Task<IActionResult> UpdateAdminSteps1(LoginTredMarktViewModel PropaVM) // should insert name in Upsert view
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
                return RedirectToAction("RedirectToUpdateAdminSteps1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.Productionvm.BrandFK });
            }
            else
            {
                return View(PropaVM);
            }
        }
        //زر الحذف تبع صفحة تعديل الخطوات
        #region API CALLS
        public IActionResult Deletestep80(int? id)
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateAdminSteps1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================

        //=====================================================صفحات الإنشاء =============================================

        //صفحة انشاء المعلومات--------------------------------------------------------
        public IActionResult RedirectToCreateAdminInformations1(int? ProductionID, int brandFK)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFK);

            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminInformations1");
        }
        public IActionResult CreateAdminInformations1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            TempData.Keep("BrandFK"); // Keep the TempData for further use
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                tredMaeketVM = new Brands(),
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
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
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
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.itemsList = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK);
            PrVM.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
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
        public async Task<IActionResult> CreateAdminInformations1(LoginTredMarktViewModel PropaVM, IFormFile? file, int selectedValue, int selectPreparation1)
        {
            if (ModelState.IsValid)
            {
                var FKProduct = PropaVM.tredMaeketVM.BrandID;

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
                        string ProductionVMFK = PropaVM.tredMaeketVM.BrandID.ToString(); // Convert to string

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

                    _unitOfWork.itemsRepository.UpdateAdmin (PropaVM.Productionvm); // تحديث Product
                    _unitOfWork.Save();
                    TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                }
                return RedirectToAction("RedirectToCreateAdminInformations1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.tredMaeketVM.BrandID });

            }
            return View(PropaVM);
        }
        //============================================================================
        //صفحة إنشاء المكونات --------------------------------------------------------
        public IActionResult RedirectToCreateAdminComponent1(int? ProductionID, int brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminComponent1");
        }
        //الانتقال الى صفحة المكونات2
        public IActionResult CreateAdminComponent1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                tredMaeketVM = new Brands(),
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
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.componontVM2 = _unitOfWork.ComponentRepository2.Get(u => u.ProductionFK == ProductionID);
            PrVM.componontVMList2 = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            return View(PrVM);
        }
        [HttpPost]
        public IActionResult CreateAdminComponent1(LoginTredMarktViewModel PropaVM)
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
                return RedirectToAction("RedirectToCreateAdminComponent1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.tredMaeketVM.BrandID });
            }
            return View(PropaVM);
        }
        //============================================================================
        // 2زر الحذف تبع المكونات "الإضافة"
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Deletec1404(int? id) //this is for delete button in rows component 
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateAdminComponent1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //صفحة إنشاء الادوات ----------------------------------------------------------
        public IActionResult RedirectToCreateAdminTools1(int? ProductionID, int? brandFk)
        {
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminTools1");
        }
        //الانتقال الى صفحة الأدوات2
        public IActionResult CreateAdminTools1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                tredMaeketVM = new Brands(),
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
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.ToolsVarityVM2 = _unitOfWork.PrepaToolsVarietyRepository2.Get(u => u.ProductionFK == ProductionID);
            PrVM.ToolsVarityVM2List = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).ToList();

            return View(PrVM);
        }

        [HttpPost]
        public IActionResult CreateAdminTools1(LoginTredMarktViewModel PropaVM)
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
                return RedirectToAction("RedirectToCreateAdminTools1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.tredMaeketVM.BrandID });
            }
            return View(PropaVM);
        }
        //زر حذف صفحة إضافة الأدوات2 --------------------------------------------------
        #region API CALLS 
        public IActionResult DeleteToolVariety2t1404(int? id) //this is for delete button in rows أدوات التحضير والصنف
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateAdminTools1", new { ProductionID = ProductionFK, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //صفحة إنشاء الخطوات 
        public IActionResult RedirectToCreateAdminSteps1(int? ProductionID, int? brandFk)
        {
            HttpContext.Session.SetInt32("ProductionID", ProductionID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminSteps1");
        }
        //الانتقال الى صفحة الخطوات2
        public IActionResult CreateAdminSteps1() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? ProductionID = HttpContext.Session.GetInt32("ProductionID");
            LoginTredMarktViewModel PrVM = new()
            {
                Productionvm = new Production(),
                tredMaeketVM = new Brands(),
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
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.Productionvm = _unitOfWork.itemsRepository.Get(u => u.ProductionID == ProductionID);
            PrVM.stepsVM2 = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProductionFK == ProductionID);
            PrVM.stepsVM2List = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(c => c.ProductionFK == ProductionID).OrderBy(item => item.ProdStepsNum).ToList();

            return View(PrVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminSteps1(LoginTredMarktViewModel PropaVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var FKProduct = PropaVM.tredMaeketVM.BrandID;
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
                return RedirectToAction("RedirectToCreateAdminSteps1", new { ProductionID = PropaVM.Productionvm.ProductionID, brandFk = PropaVM.tredMaeketVM.BrandID });

            }
            return View(PropaVM);
        }
        //===============================POST Add ID =================================
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
        //زر الحذف تبع صفحة إضافة الخطوات-------------------------------------------
        #region API CALLS
        public IActionResult Deletestep280(int? id)
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateAdminSteps1", new { ProductionID = ProductionFK, brandFk = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //2زر الحذف في صفحة قائمة Product 
        #region
        //[HttpDelete]
        public IActionResult DeleteProductionPost(int? id)
        {
            var DeleteTools = _unitOfWork.PrepaToolsVarietyRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == id).ToList();
            _unitOfWork.PrepaToolsVarietyRepository2.RemoveRange(DeleteTools);
            _unitOfWork.Save();


            var DelteComponent = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == id).ToList();
            _unitOfWork.ComponentRepository2.RemoveRange(DelteComponent);
            _unitOfWork.Save();

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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToProductionAdmin", new { BrandFK = FKBrandToRedyrect1 }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //زر الحذف لكامل القسم في صفحة UpdateMainsection
        public IActionResult DeleteSectionProductionUpdate(int? id, int? BrandID)
        {
            if (BrandID == null)
            {
                return Json(new { success = false, message = "لايوجد علامة تجارية" });
            }

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            // جلب قائمة التحضيرات المرتبطة بـ BrandID
            var deleteProductionList = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == BrandID).ToList();

            if (deleteProductionList != null && deleteProductionList.Count > 0)
            {
                foreach (var production in deleteProductionList)
                {
                    // جلب الأدوات المرتبطة بـ PreparationID
                    var deleteTools = _unitOfWork.PrepaToolsVarietyRepository2.GetAll().Where(u => u.ProductionFK == production.ProductionID).ToList();

                    // حذف الأدوات المرتبطة
                    _unitOfWork.PrepaToolsVarietyRepository2.RemoveRange(deleteTools);
                    _unitOfWork.Save();

                    //حذف المكونات المرتبطة
                    var DelteComponent = _unitOfWork.ComponentRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == production.ProductionID).ToList();
                    _unitOfWork.ComponentRepository2.RemoveRange(DelteComponent);
                    _unitOfWork.Save();

                    //حذف الخطوات المرتبطة 
                    var Deletesteps = _unitOfWork.StepsPreparationRepository2.GetAll(incloudeProperties: "Production").Where(u => u.ProductionFK == production.ProductionID).ToList();
                    if (Deletesteps != null)
                    {
                        for (int i = 0; i < Deletesteps.Count; i++)
                        {
                            var delet = Deletesteps[i];
                            var BrandId = _unitOfWork.itemsRepository.Get(u => u.ProductionID == delet.ProductionFK);
                            var IDstep = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == delet.ProdStepsID);

                            string IDStep = IDstep.ProdStepsID.ToString();

                            if (!string.IsNullOrEmpty(delet.ProdSImage))
                            {
                                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "IMAGES", IDStep, delet.ProdSImage);
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                            }

                            _unitOfWork.StepsPreparationRepository2.Remove(delet);
                            _unitOfWork.Save();

                        }

                    }
                    // حذف الصورة المرتبطة بالتحضير
                    if (!string.IsNullOrEmpty(production.ProductImage))
                    {
                        string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", production.ProductionID.ToString(), production.ProductImage);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // حذف التحضير نفسه
                    _unitOfWork.itemsRepository.Remove(production);
                    _unitOfWork.Save();
                }     
            }
            //حذف القسم بالكامل 
            var DeleteMainSection = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == id);
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
            // حفظ التغييرات بعد حذف كل الأدوات والتحضيرات
            _unitOfWork.Save();
            // إعادة التوجيه إلى صفحة التعديل
            return Json(new
            {
                success = true,
                redirectToUrl = Url.Action("RedirectToUpdateMainSection", "Sections", new { area = "Admin", brandFk = BrandID })
            });
        }

        //============================================================================
        //زر الحذف لكامل القسم في صفحة AddMainsection
        public IActionResult DeleteSectionProductionAdd(int? id, int? BrandID)
        {
            if (BrandID == null)
            {
                return Json(new { success = false, message = "Invalid Brand ID" });
            }

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            // جلب قائمة التحضيرات المرتبطة بـ BrandID
            var deleteProductionList = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == BrandID).ToList();

            if (deleteProductionList != null && deleteProductionList.Count > 0)
            {
                foreach (var production in deleteProductionList)
                {
                    // جلب الأدوات المرتبطة بـ PreparationID
                    var deleteTools = _unitOfWork.PrepaToolsVarietyRepository2.GetAll().Where(u => u.ProductionFK == production.ProductionID).ToList();

                    // حذف الأدوات المرتبطة
                    _unitOfWork.PrepaToolsVarietyRepository2.RemoveRange(deleteTools);
                    _unitOfWork.Save();

                    //حذف المكونات المرتبطة
                    var DelteComponent = _unitOfWork.ComponentRepository2.GetAll().Where(u => u.ProductionFK == production.ProductionID).ToList();
                    _unitOfWork.ComponentRepository2.RemoveRange(DelteComponent);
                    _unitOfWork.Save();

                    //حذف الخطوات المرتبطة 
                    var Deletesteps = _unitOfWork.StepsPreparationRepository2.GetAll().Where(u => u.ProductionFK == production.ProductionID).ToList();
                    if (Deletesteps != null)
                    {
                        for (int i = 0; i < Deletesteps.Count; i++)
                        {
                            var delet = Deletesteps[i];
                            var BrandId = _unitOfWork.itemsRepository.Get(u => u.ProductionID == delet.ProductionFK);
                            var IDstep = _unitOfWork.StepsPreparationRepository2.Get(u => u.ProdStepsID == delet.ProdStepsID);

                            string IDStep = IDstep.ProdStepsID.ToString();

                            if (!string.IsNullOrEmpty(delet.ProdSImage))
                            {
                                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "IMAGES", IDStep, delet.ProdSImage);
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                            }

                            _unitOfWork.StepsPreparationRepository2.Remove(delet);
                            _unitOfWork.Save();

                        }

                    }
                    // حذف الصورة المرتبطة بالتحضير
                    if (!string.IsNullOrEmpty(production.ProductImage))
                    {
                        string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", production.ProductionID.ToString(), production.ProductImage);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // حذف التحضير نفسه
                    _unitOfWork.itemsRepository.Remove(production);
                    _unitOfWork.Save();
                }
            }
            //حذف القسم بالكامل 
            var DeleteMainSection = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == id);
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
            // حفظ التغييرات بعد حذف كل الأدوات والتحضيرات
            _unitOfWork.Save();

            // إعادة التوجيه إلى صفحة التعديل
            return Json(new
            {
                success = true,
                redirectToUrl = Url.Action("RedirectToAddMainSection", "Sections", new { area = "Admin", brandFk = BrandID })
            });
        }

        //============================================================================
        [HttpPost]
        public async Task<IActionResult> RedirectToProductionAdminUpdate(LoginTredMarktViewModel viewModel)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                var sectionsList = viewModel.MainsectionVMlist.ToList(); // Convert to list to allow indexing
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                for (int i = 0; i < sectionsList.Count; i++)
                {
                    var sections = sectionsList[i];

                    // تحقق إذا كان اسم القسم هو "الإنتاج"
                    if (sections.SectionsName == "الإنتاج")
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

                        // الخروج من اللوب بعد تنفيذ العمل للقسم "الإنتاج"
                        break;
                    }
                }

                _unitOfWork.Save();
                //TempData["success"] = "تم إضافة قسم الإنتاج بنجاح";
                return RedirectToAction("RedirectToProductionAdmin", new { brandFk = viewModel.TredMarktVM.BrandID });
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddSectionProductionUpdate(string sectionName, int BrandID)
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateMainSection", "Sections", new { area = "Admin", brandFK = BrandID, mainSectionId = newSection.MainSectionsID }) });
        }
        [HttpPost]
        public IActionResult AddSectionProduction(string sectionName, int BrandID)
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
