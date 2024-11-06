using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using Test12.DataAccess.Repository;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.Production;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;

namespace UserInterface.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SectionDetailsPreparation : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionDetailsPreparation(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;

        }
        public IActionResult RedirectToPreparationAdmin(int brandFK, int mainSectionId)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId);
            HttpContext.Session.SetInt32("BrandFK", brandFK);

            TempData.Keep("BrandFK");
            return RedirectToAction("PreparationAdminList");
        }
        public IActionResult PreparationAdminList() //this for display List Of التحضيرات Page1
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");

            TempData.Keep("BrandFK"); // Keep the TempData for further use

            var PreparationList = _unitOfWork.PreparationRepository.GetAll(incloudeProperties: "componentsCountPrint,toolsCountPrint")
                                     ?.Where(u => u.BrandFK == brandFK)
                                     ?.OrderBy(item => item.PreparationsOrder)
                                     ?.ToList() ?? new List<Preparations>();
            LoginTredMarktViewModel PrVM = new LoginTredMarktViewModel
            {
                PreparationList = PreparationList,
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
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);


            TempData["ID"] = brandFK;
            // Assuming you handle the header through layout or another mechanism
            return View(PrVM);
        }
        // تبع List -----------------------------------------------------------------
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

        //CreateAdminInformation -----------------------------------------------------
        public IActionResult RedirectToCreateAdminInformations(int? PreparationID, int brandFK)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFK);

            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminInformation");
        }
        public IActionResult CreateAdminInformation()
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
            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.componontVMList = new List<PreparationIngredients>();
            PrVM.ToolsVarityVMList = new List<PreparationTools>();
            PrVM.stepsVM = new List<PreparationSteps>();
            PrVM.PreparationList = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK);

            return View(PrVM);
        }
        [HttpPost] //This for Add Or Update Page . 
        public async Task<IActionResult> CreateAdminInformation(LoginTredMarktViewModel PrepaVM, IFormFile? file, int selectedValue) // should insert name in Upsert view
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
                        //string preparationVMFk = PrepaVM.TredMarktVM.BrandID.ToString(); // Convert to string

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
            return RedirectToAction("RedirectToCreateAdminInformations", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFK = PrepaVM.TredMarktVM.BrandID });
        }
        //============================================================================
        //صفحة إنشاء المكونات 
        public IActionResult RedirectToCreateAdminComponent(int? PreparationID, int brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk);

            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminComponent");
        }
        public IActionResult CreateAdminComponent() // After Enter تعديل Display التحضيرات والمكونات...
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
        public IActionResult CreateAdminComponent(LoginTredMarktViewModel PrepaVM) // should insert name in Upsert view
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
                return RedirectToAction("RedirectToCreateAdminComponent", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFK = PrepaVM.TredMarktVM.BrandID });
            }
            return View(PrepaVM);
        }
        //============================================================================
        // زر الحذف تبع المكونات 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult Delete00(int? id) //this is for delete button in rows component 
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateAdminComponent", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion 

        //تبع الحذف في صفحة الإضافة : --------------------------------------------------
        #region API CALLS 
        //[HttpDelete]
        public IActionResult DeleteCom2(int? id) //this is for delete button in rows component 
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateAdminComponent", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion
        //============================================================================

        //التعديل في صفحة المعلومات---------------------------------------------------
        public IActionResult RedirectToSectionDetailsPreparation(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);

            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateAdminInformation");
        }

        // الانتقال الى صفحة المعلومات 2 
        public IActionResult UpdateAdminInformation() // After Enter تعديل Display التحضيرات والمكونات...
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
        public async Task<IActionResult> UpdateAdminInformation(LoginTredMarktViewModel PrepaVM, IFormFile? file)
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
                _unitOfWork.PreparationRepository.UpdateAdmin(PrepaVM.PreparationVM);
                _unitOfWork.Save();

                TempData["success"] = "تم تحديث المعلومات بشكل ناجح";
                return RedirectToAction("RedirectToSectionDetailsPreparation", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            else
            {
                return View(PrepaVM);
            }

        }
        //============================================================================
        //  التعديل  في صفحة المكوانات 1-----------------------------------------------
        public IActionResult RedirectToUpdateAdminComponent(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);

            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateAdminComponent");
        }

        //للانتقال الى صفحة المكونات2 :
        public IActionResult UpdateAdminComponent() // After Enter تعديل Display التحضيرات والمكونات...
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
        public IActionResult UpdateAdminComponent(LoginTredMarktViewModel PrepaVM)
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
                return RedirectToAction("RedirectToUpdateAdminComponent", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================
        // صفحة إنشاء الأدوات----------------------------------------------------------
        public IActionResult RedirectToCreateToolsvarityAdmin(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);

            TempData.Keep("BrandFK");
            return RedirectToAction("CreateToolsvarityAdmin");
        }
        public IActionResult CreateToolsvarityAdmin() // After Enter تعديل Display التحضيرات والمكونات...
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
        public IActionResult CreateToolsvarityAdmin(LoginTredMarktViewModel PrepaVM)
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
                return RedirectToAction("RedirectToCreateToolsvarityAdmin", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================
        // زر الحذف في صفحة الإضافة---------------------------------------------------- 
        #region API CALLS 
        //[HttpDelete]
        public IActionResult DeleteToolVarietyT100(int? id) //this is for delete button in rows أدوات التحضير والصنف
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

        //الانتقال الى صفحة  تعديل الأدوات1 
        public IActionResult RedirectToUpdateToolsvarityAdmin(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في TempData (استخدامها فقط إذا كانت للمرة القادمة فقط)
            TempData["PreparationID"] = PreparationID ?? 0;
            TempData["BrandFK"] = brandFk ?? 0;

            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);

            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateToolsvarityAdmin");
        }
        //الانتقال الى صفحة الأدوات1 
        public IActionResult UpdateToolsvarityAdmin() // After Enter تعديل Display التحضيرات والمكونات...
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
        public IActionResult UpdateToolsvarityAdmin(LoginTredMarktViewModel PrepaVM)
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
                return RedirectToAction("RedirectToUpdateToolsvarityAdmin", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================
        //زر حذف صفحة تعديل الأدوات ---------------------------------------------------
        #region API CALLS 
        //[HttpDelete]
        public IActionResult DeleteToolVariety00(int? id) //this is for delete button in rows أدوات التحضير والصنف
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateToolsvarityAdmin", new { PreparationID = PreparationFk, brandFk = BranFK }) });
        }
        #endregion
        //============================================================================

        //الانتقال الى صفحة الخطوات1 
        public IActionResult RedirectToUpdateStepsAdmin(int? PreparationID, int? brandFk)        
        {
            // تخزين البيانات في TempData (استخدامها فقط إذا كانت للمرة القادمة فقط)
            TempData["PreparationID"] = PreparationID ?? 0;
            TempData["BrandFK"] = brandFk ?? 0;

            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);

            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateStepsAdmin");
        }
        //الانتقال الى صفحة الخطوات2 
        public IActionResult UpdateStepsAdmin() // After Enter تعديل Display التحضيرات والمكونات...
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
        public async Task<IActionResult> UpdateStepsAdmin(LoginTredMarktViewModel PrepaVM)
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
                return RedirectToAction("RedirectToUpdateStepsAdmin", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================
        //POST Add ID Preparation ----------------------------------------------------
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
        //زر الحذف تبع صفحة تعديل الخطوات---------------------------------------------
        #region API CALLS
        public IActionResult Deletestep101(int? id)
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

            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToUpdateStepsAdmin", new { PreparationID = PreparationFk, BrandFK = BranFK }) }); //أحتاج يرجع لنفس صفحة التعديل 

        }
        #endregion
        //============================================================================
        //الانتقال الى صفحة إضافة الخطوات1 --------------------------------------------
        public IActionResult RedirectToCreateAdminSteps(int? PreparationID, int? brandFk)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("PreparationID", PreparationID ?? 0);
            HttpContext.Session.SetInt32("BrandFK", brandFk ?? 0);
            TempData.Keep("BrandFK");
            return RedirectToAction("CreateAdminSteps");
        }
        //الانتقال الى صفحة الخطوات2 
        public IActionResult CreateAdminSteps() // After Enter تعديل Display التحضيرات والمكونات...
        {
            // استرجاع البيانات من الجلسة
            int? brandFk = HttpContext.Session.GetInt32("BrandFK");
            int? PreparationID = HttpContext.Session.GetInt32("PreparationID");
            LoginTredMarktViewModel PrVM = new()
            {
                PreparationVM = new Preparations(),
                componontVMList = new List<PreparationIngredients>(),
                stepsVM = new List<PreparationSteps>(),
                tredMaeketVM = new Brands(),
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
            PrVM.tredMaeketVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFk);
            PrVM.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.PreparationsID == PreparationID);
            PrVM.stepsVM = _unitOfWork.StepsPreparationRepository.GetAll().Where(c => c.PreparationsFK == PreparationID).OrderBy(item => item.PrepStepsNum).ToList();
            return View(PrVM);
        }
        [HttpPost] // This is for Add or Update Page.
        public async Task<IActionResult> CreateAdminSteps(LoginTredMarktViewModel PrepaVM)
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
                return RedirectToAction("RedirectToCreateAdminSteps", new { PreparationID = PrepaVM.PreparationVM.PreparationsID, brandFk = PrepaVM.PreparationVM.BrandFK });
            }
            return View(PrepaVM);
        }
        //============================================================================
        //زر الحذف تبع صفحة إضافة الخطوات --------------------------------------------
        #region API CALLS
        public IActionResult Deletesteps202(int? id)
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

            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToCreateAdminSteps", new { PreparationID = PreparationFk, brandFk = BranFK }) });
            //أحتاج يرجع لنفس صفحة التعديل 

        }
        #endregion
        //============================================================================
        ////زر الحذف في صفحة قائمة التحضيرات 
        #region
        //[HttpDelete]
        public IActionResult DeletePreparationAdmin(int? id)
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
            //عشان أوجهه لصفحة List 
            int PreparaionID = DeleteoneOflist.PreparationsID;
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
            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToPreparationAdmin", new { PreparationID = PreparaionID, brandFk = FKBrandToRedyrect1 }) }); //أحتاج يرجع لنفس صفحة التعديل 

        }
        #endregion
        //============================================================================
        //زر الحذف لكامل القسم في صفحة UpdateMainsection
        public IActionResult DeleteSectionPreparationUpdate(int? id ,int? BrandID)
        {
            if (BrandID == null)
            {
                return Json(new { success = false, message = "Invalid Brand ID" });
            }

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            // جلب قائمة التحضيرات المرتبطة بـ BrandID
            var deletePreparationList = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == BrandID).ToList();

            if (deletePreparationList != null && deletePreparationList.Count > 0)
            {
                foreach (var preparation in deletePreparationList)
                {
                    // جلب الأدوات المرتبطة بـ PreparationID
                    var deleteTools = _unitOfWork.PrepaToolsVarietyRepository.GetAll(incloudeProperties: "Preparation").Where(u => u.PreparationsFK == preparation.PreparationsID).ToList();

                    // حذف الأدوات المرتبطة
                        _unitOfWork.PrepaToolsVarietyRepository.RemoveRange(deleteTools);
                        _unitOfWork.Save();
                  
                    //حذف المكونات المرتبطة
                    var DelteComponent = _unitOfWork.ComponentRepository.GetAll(incloudeProperties: "Preparation").Where(u => u.PreparationsFK == preparation.PreparationsID).ToList();
                    _unitOfWork.ComponentRepository.RemoveRange(DelteComponent);
                    _unitOfWork.Save();

                    //حذف الخطوات المرتبطة 
                    var Deletesteps = _unitOfWork.StepsPreparationRepository.GetAll(incloudeProperties: "Preparation").Where(u => u.PreparationsFK == preparation.PreparationsID).ToList();
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
                    // حذف الصورة المرتبطة بالتحضير
                    if (!string.IsNullOrEmpty(preparation.prepareImage))
                    {
                        string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", preparation.PreparationsID.ToString(), preparation.prepareImage);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    
                    // حذف التحضير نفسه
                    _unitOfWork.PreparationRepository.Remove(preparation);
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
                    string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DeleteMainSection.MainSectionsID.ToString() ,DeleteMainSection.SectionsImage);
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
        public IActionResult DeleteSectionPreparationAdd(int? id, int? BrandID)
        {
            if (BrandID == null)
            {
                return Json(new { success = false, message = "Invalid Brand ID" });
            }

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            // جلب قائمة التحضيرات المرتبطة بـ BrandID
            var deletePreparationList = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == BrandID).ToList();

            if (deletePreparationList != null && deletePreparationList.Count > 0)
            {
                foreach (var preparation in deletePreparationList)
                {
                    // جلب الأدوات المرتبطة بـ PreparationID
                    var deleteTools = _unitOfWork.PrepaToolsVarietyRepository.GetAll().Where(u => u.PreparationsFK == preparation.PreparationsID).ToList();

                    // حذف الأدوات المرتبطة
                    _unitOfWork.PrepaToolsVarietyRepository.RemoveRange(deleteTools);
                    _unitOfWork.Save();

                    //حذف المكونات المرتبطة
                    var DelteComponent = _unitOfWork.ComponentRepository.GetAll().Where(u => u.PreparationsFK == preparation.PreparationsID).ToList();
                    _unitOfWork.ComponentRepository.RemoveRange(DelteComponent);
                    _unitOfWork.Save();

                    //حذف الخطوات المرتبطة 
                    var Deletesteps = _unitOfWork.StepsPreparationRepository.GetAll().Where(u => u.PreparationsFK == preparation.PreparationsID).ToList();
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
                    // حذف الصورة المرتبطة بالتحضير
                    if (!string.IsNullOrEmpty(preparation.prepareImage))
                    {
                        string imagePath = Path.Combine(wwwRootPathSteps, "IMAGES", preparation.PreparationsID.ToString(), preparation.prepareImage);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // حذف التحضير نفسه
                    _unitOfWork.PreparationRepository.Remove(preparation);
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
                    string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES",DeleteMainSection.MainSectionsID.ToString(), DeleteMainSection.SectionsImage);
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

        [HttpPost]
        public async Task<IActionResult> RedirectToPreparationAdminUpdate(LoginTredMarktViewModel viewModel)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                var sectionsList = viewModel.MainsectionVMlist.ToList(); // Convert to list to allow indexing
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                for (int i = 0; i < sectionsList.Count; i++)
                {
                    var sections = sectionsList[i];

                    // شرط للتحقق من أن اسم القسم هو "التحضيرات"
                    if (sections.SectionsName == "التحضيرات")
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

                        // الخروج من اللوب بعد تنفيذ العمل لهذا القسم
                        break;
                    }
                }
            
                    _unitOfWork.Save();
                //TempData["success"] = "تم إضافة قسم التحضيرات بنجاح";
                return RedirectToAction("RedirectToPreparationAdmin", new { brandFk = viewModel.TredMarktVM.BrandID });
            }

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddSectionPreparationUpdate(string sectionName, int BrandID)
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
        public IActionResult AddSectionPreparation(string sectionName, int BrandID)
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
