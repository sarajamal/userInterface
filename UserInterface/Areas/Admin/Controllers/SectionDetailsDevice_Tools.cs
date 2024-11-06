using Microsoft.AspNetCore.Mvc;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Device_Tools;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;

namespace UserInterface.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SectionDetailsDevice_Tools : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionDetailsDevice_Tools(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;

        }
        //=========================GET LIST ===========================================
        public IActionResult RedirectToDeviceToolsAdminList(int brandFK, int mainSectionId)
        {
            TempData["mainSectionId"] = mainSectionId;
            HttpContext.Session.SetInt32("BrandFK", brandFK);

            TempData.Keep("BrandFK");

            return RedirectToAction("DeviceToolsListAdmin");
        }
        public IActionResult DeviceToolsListAdmin() //this for display List Of التحضيرات Page1
        {
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");

            int? mainSectionId = TempData["mainSectionId"] as int?;
            TempData.Keep("BrandFK"); // Keep the TempData for further use

            LoginTredMarktViewModel PrVM = new()
            {
                DeviceToolsLoginVMlist = _unitOfWork.DevicesAndTools.GetAll()
                .Where(u => u.BrandFK == brandFK).OrderBy(item => item.DevicesAndToolsOrder).ToList(),


                WelcomtredmarketDeviceTools = new LoginTredMarktViewModel()

            };
            PrVM.WelcomtredmarketDeviceTools.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.WelcomtredmarketDeviceTools.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomtredmarketDeviceTools.Productionvm = _unitOfWork.itemsRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomtredmarketDeviceTools.CleanViewModel = _unitOfWork.CleanRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomtredmarketDeviceTools.ReadyFoodLoginVM = _unitOfWork.readyFoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomtredmarketDeviceTools.FoodLoginVM = _unitOfWork.FoodRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomtredmarketDeviceTools.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            PrVM.WelcomtredmarketDeviceTools.MainsectionVM = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == mainSectionId);
            PrVM.WelcomtredmarketDeviceTools.MainsectionVMlist = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomtredmarketDeviceTools.FoodLoginVMlist = _unitOfWork.FoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomtredmarketDeviceTools.ProductionLoginVMlist = _unitOfWork.itemsRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomtredmarketDeviceTools.PreparatonLoginVMlist = _unitOfWork.PreparationRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomtredmarketDeviceTools.ReadyFoodLoginVMlist = _unitOfWork.readyFoodRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomtredmarketDeviceTools.CleanLoginVMlist = _unitOfWork.CleanRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            PrVM.WelcomtredmarketDeviceTools.tredList = _unitOfWork.TredMarketRepository.GetAll().Where(c => c.BrandID == brandFK).ToList();

            PrVM.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            PrVM.tredMaeketToolsVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            // Store the FK value in TempData
            TempData["ID"] = brandFK;
            // Display the upDate1d list
            return View(PrVM);
        }

        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll(int? id)
        {

            IEnumerable<DevicesAndTools> objPreparationList = _unitOfWork.DevicesAndTools.GetAll()
                .Where(u => u.BrandFK == id).OrderBy(item => item.DevicesAndToolsOrder).ToList();

            return Json(new { data = objPreparationList });
        }
        #endregion
        //============================================================================
        //------------------------------صفحة الإضافة-----------------------------------
        public IActionResult CreateDeviceToolsAdmin(int? id)
        {
            LoginTredMarktViewModel PrVM = new()
            {
                DeviceToolsLoginVM = new DevicesAndTools(),
                DeviceToolsLoginVMlist = new List<DevicesAndTools>(),
                tredMaeketToolsVM = new Brands(),

            };

            PrVM.tredMaeketToolsVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == id);
            PrVM.Devices_toolsVMorder = _unitOfWork.DevicesAndTools.GetAll().Where(u => u.BrandFK == id);
            PrVM.DeviceToolsLoginVM = new DevicesAndTools();
            PrVM.DeviceToolsLoginVMlist = new List<DevicesAndTools>();

            return View(PrVM);

        }
        [HttpPost]
        public async Task<IActionResult> CreateDeviceToolsAdmin(LoginTredMarktViewModel device_ToolsVM, int selectDevicetools)
        {
            if (ModelState.IsValid)
            {
                int DeviceFK = device_ToolsVM.tredMaeketToolsVM.BrandID;

                for (int i = 0; i < device_ToolsVM.DeviceToolsLoginVMlist.Count; i++)
                {
                    var devices = device_ToolsVM.DeviceToolsLoginVMlist[i];

                    // Fetch the last FoodStuffs entry based on FoodStuffsNum
                    var lastDevicesAndTools = _unitOfWork.DevicesAndTools.GetAll().Where(u => u.BrandFK == DeviceFK)
                                        .OrderByDescending(u => u.DevicesAndTools_Num)
                                        .FirstOrDefault();

                    // Set the FoodStuffsName based on the last entry's FoodStuffsNum
                    int? newDevicesAndTools_Num = lastDevicesAndTools.DevicesAndTools_Num;

                    if (newDevicesAndTools_Num != null && lastDevicesAndTools.DevicesAndTools_Num.HasValue)
                    {
                        newDevicesAndTools_Num = lastDevicesAndTools.DevicesAndTools_Num.Value + 1;
                    }
                    else
                    {
                        newDevicesAndTools_Num = 1;
                    }

                    var newDevice = new DevicesAndTools
                    {
                        DevicesAndToolsID = devices.DevicesAndToolsID,
                        BrandFK = DeviceFK,
                        DevicesAndTools_Name = devices.DevicesAndTools_Name,
                        DevicesAndTools_Num = newDevicesAndTools_Num,
                    };

                    string wwwRootDevicePath = _webHostEnvironment.WebRootPath; // get us root folder
                    var file1Name1 = $"file1_{newDevice.DevicesAndToolsID}";
                    var file1ForDevice1 = HttpContext.Request.Form.Files[file1Name1];

                    string DevicesAndToolsID = newDevice.DevicesAndToolsID.ToString();
                    string BrandFK = newDevice.BrandFK.ToString();

                    var devicePath1 = Path.Combine(wwwRootDevicePath, "IMAGES", DevicesAndToolsID);

                    if (file1ForDevice1 != null && file1ForDevice1.Length > 0)
                    {
                        string fileName11 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForDevice1.FileName);

                        if (!Directory.Exists(devicePath1))
                        {
                            Directory.CreateDirectory(devicePath1);
                        }

                        using (var fileStream = new FileStream(Path.Combine(devicePath1, fileName11), FileMode.Create)) //save images
                        {
                            await file1ForDevice1.CopyToAsync(fileStream);
                        }
                        newDevice.DevicesAndTools_Image = fileName11;
                    }
                    //// reOrder2 
                    if (selectDevicetools == 0)
                    {
                        int IDDevice = newDevice.DevicesAndToolsID;
                        newDevice.DevicesAndToolsOrder = IDDevice;

                        //// Get the maximum order value in the existing list
                        //double maxOrder = _unitOfWork.Device_tools1.GetAll()
                        //    .Max(item => item.DevicesAndToolsOrder) ?? 0.0f; // Default to 0.0f if there are no existing items

                        //// Round down the maxOrder value to the nearest integer
                        //int maxOrderAsInt = (int)Math.Floor(maxOrder);

                        //// Set the new order value for the "اخرى" (Other) item
                        //double newOrder = maxOrderAsInt + 1.0f;
                        //newDevice.DevicesAndToolsOrder = newOrder;
                    }
                    else
                    {
                        var getIdOrder = _unitOfWork.DevicesAndTools.Get(u => u.DevicesAndToolsID == selectDevicetools);
                        double? OldOrder = getIdOrder.DevicesAndToolsOrder; // Default to 0.0f if Order is null
                        double? newOrder = OldOrder + 0.1;
                        newDevice.DevicesAndToolsOrder = newOrder;
                    }

                    var existingDevices = _unitOfWork.DevicesAndTools.Get(u => u.DevicesAndToolsID == devices.DevicesAndToolsID);

                    if (existingDevices != null)
                    {
                        existingDevices.DevicesAndTools_Name = devices.DevicesAndTools_Name;
                        existingDevices.DevicesAndTools_Image = newDevice.DevicesAndTools_Image ?? existingDevices.DevicesAndTools_Image;
                        existingDevices.DevicesAndToolsOrder = newDevice.DevicesAndToolsOrder;
                        existingDevices.DevicesAndTools_Num = newDevice.DevicesAndTools_Num;
                        _unitOfWork.DevicesAndTools.Update(existingDevices);
                    }
                    else
                    {
                        _unitOfWork.DevicesAndTools.Add(newDevice);
                    }

                    _unitOfWork.Save();


                    List<DevicesAndTools> obdeviceToolsList = _unitOfWork.DevicesAndTools.GetAll().OrderBy(item => item.DevicesAndToolsOrder).ToList();
                    _unitOfWork.Save();
                }

                TempData["success"] = "تم إضافة الأجهزة والأدوات بشكل ناجح";
                return RedirectToAction("RedirectToDeviceToolsAdminList", new { brandFK = device_ToolsVM.tredMaeketToolsVM.BrandID });
            }

            // Handle the case where ModelState is not valid
            return View(device_ToolsVM);
        }
        //============================================================================
        //=========================================POST Add ID ===================================
        [HttpPost]
        public IActionResult GetAddID(int BrandFK, LoginTredMarktViewModel device_ToolsVM)
        {
            // Fetch the production and steps associated with the given ProductionFK
            device_ToolsVM.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.BrandFK == BrandFK);
            device_ToolsVM.DeviceToolsLoginVMlist = _unitOfWork.DevicesAndTools.GetAll(incloudeProperties: "Brand").Where(u => u.BrandFK == BrandFK).ToList();

            // Create a new step
            var newDevice = new DevicesAndTools
            {
                BrandFK = BrandFK,
            };

            // Save the new step to the database
            _unitOfWork.DevicesAndTools.Add(newDevice);
            _unitOfWork.Save();

            // Return the new step's ID
            return Json(newDevice.DevicesAndToolsID);
        }
        //============================================================================
        //---------------------------------صفحة التعديل -----------------------------
        public IActionResult IndexDeviceToolsAdmin(int? id)
        {
            TempData.Keep("BrandFK"); // Keep the TempData for further use

            LoginTredMarktViewModel PrVM = new()
            {
                DeviceToolsLoginVM = new DevicesAndTools(),
                DeviceToolsLoginVMlist = new List<DevicesAndTools>(),
                tredMaeketToolsVM = new Brands(),

            };

            PrVM.tredMaeketToolsVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == id);
            PrVM.DeviceToolsLoginVM = _unitOfWork.DevicesAndTools.Get(u => u.DevicesAndToolsID == id);
            PrVM.DeviceToolsLoginVMlist = _unitOfWork.DevicesAndTools.GetAll(incloudeProperties: "Brand").Where(u => u.DevicesAndToolsID == id).ToList(); //هو يحتوي على قائمة من جدول المكونات واللي يساعده على العرض هي view
            PrVM.Devices_toolsVMorder = _unitOfWork.DevicesAndTools.GetAll().Where(u => u.BrandFK == id);

            return View(PrVM);
        }

        [HttpPost]
        public async Task<IActionResult> IndexDeviceToolsAdmin(LoginTredMarktViewModel device_ToolsVM, int selectDevicetools)
        {

            if (ModelState.IsValid)
            {
                if (device_ToolsVM.DeviceToolsLoginVM != null)
                {
                    for (int i = 0; i < device_ToolsVM.DeviceToolsLoginVMlist.Count; i++)
                    {
                        var devices = device_ToolsVM.DeviceToolsLoginVMlist[i];

                        string DevicesAndToolsID = devices.DevicesAndToolsID.ToString();
                        string BrandFK = devices.BrandFK.ToString();
                        string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

                        var devicePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DevicesAndToolsID);

                        var file1Name = $"file1_{devices.DevicesAndToolsID}";
                        var file1ForDevice = HttpContext.Request.Form.Files[file1Name];

                        if (file1ForDevice != null)
                        {
                            if (!string.IsNullOrEmpty(devices.DevicesAndTools_Image)) // Check if there's an existing image path
                            {
                                var OldImagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DevicesAndToolsID, devices.DevicesAndTools_Image);

                                if (System.IO.File.Exists(OldImagePath1))
                                {
                                    System.IO.File.Delete(OldImagePath1); // Delete old image if it exists
                                }
                            }

                            string fileNameSteps1 = Guid.NewGuid().ToString() + Path.GetExtension(file1ForDevice.FileName);

                            //اذا المسار مش موجود سو مسار جديد 
                            if (!Directory.Exists(devicePath1))
                            {
                                Directory.CreateDirectory(devicePath1);
                            }
                            using (var fileStream1 = new FileStream(Path.Combine(devicePath1, fileNameSteps1), FileMode.Create))
                            {
                                await file1ForDevice.CopyToAsync(fileStream1);
                            }

                            devices.DevicesAndTools_Image = fileNameSteps1; // Update the image path
                        }
                        //// reOrder2 
                        //if (selectDevicetools == 0)
                        //{
                        //    int IDDevice = devices.DevicesAndToolsID;
                        //    devices.DevicesAndToolsOrder = IDDevice;

                        //// Get the maximum order value in the existing list
                        //double maxOrder = _unitOfWork.Device_tools1.GetAll()
                        //    .Max(item => item.DevicesAndToolsOrder) ?? 0.0f; // Default to 0.0f if there are no existing items

                        //// Round down the maxOrder value to the nearest integer
                        //int maxOrderAsInt = (int)Math.Floor(maxOrder);

                        //// Set the new order value for the "اخرى" (Other) item
                        //double newOrder = maxOrderAsInt + 1.0f;
                        //newDevice.DevicesAndToolsOrder = newOrder;
                        //}
                        //else
                        //{
                        //    var getIdOrder = _unitOfWork.DevicesAndTools.Get(u => u.DevicesAndToolsID == selectDevicetools);
                        //    double? OldOrder = getIdOrder.DevicesAndToolsOrder; // Default to 0.0f if Order is null
                        //    double? newOrder = OldOrder + 0.1;
                        //    devices.DevicesAndToolsOrder = newOrder;
                        //}
                        var existingDevices = _unitOfWork.DevicesAndTools.Get(u => u.DevicesAndToolsID == devices.DevicesAndToolsID);

                        if (existingDevices != null)
                        {

                            existingDevices.DevicesAndTools_Name = devices.DevicesAndTools_Name;
                            //existingDevices.DevicesAndToolsOrder = devices.DevicesAndToolsOrder;

                            existingDevices.DevicesAndTools_Image = devices.DevicesAndTools_Image;

                            _unitOfWork.DevicesAndTools.Update(existingDevices);
                        }
                        else
                        {
                            _unitOfWork.DevicesAndTools.Add(devices);
                        }
                        _unitOfWork.Save();
                    }
                }
            }
            TempData["success"] = "تم تحديث الأجهزة والأدوات بشكل ناجح";
            return RedirectToAction("RedirectToDeviceToolsAdminList", new { brandFK = device_ToolsVM.DeviceToolsLoginVM.BrandFK });
        }
        //============================================================================
        //زر الحذف في صفحة قائمة  الأدوات والأجهزة 
        #region
        //[HttpDelete]
        public IActionResult DelteToolsdevice77(int? id)
        {
            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            var deleteDeviceToolPicture = _unitOfWork.DevicesAndTools.Get(u => u.DevicesAndToolsID == id);

            string DevicesAndToolsID = deleteDeviceToolPicture.DevicesAndToolsID.ToString();
            string BrandFK = deleteDeviceToolPicture.BrandFK.ToString();

            // Delete the associated image file
            if (!string.IsNullOrEmpty(deleteDeviceToolPicture.DevicesAndTools_Image))
            {
                string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DevicesAndToolsID, deleteDeviceToolPicture.DevicesAndTools_Image);
                if (System.IO.File.Exists(imagePath1))
                {
                    System.IO.File.Delete(imagePath1);
                }
            }

            _unitOfWork.DevicesAndTools.Remove(deleteDeviceToolPicture);
            _unitOfWork.Save();

            return Json(new { success = true, redirectToUrl = Url.Action("RedirectToDeviceToolsAdminList", new { BrandFK = BrandFK }) }); //أحتاج يرجع لنفس صفحة التعديل 
        }
        #endregion
        //============================================================================
        //زر الحذف في صفحة تعديل الأقسام الرئيسية -----------------------------------*/
        #region
        //[HttpDelete]
        public IActionResult DeleteSectionDeviceToolsUpdate(int? id)
        {
            var DeleteMainSection = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == id);
            if (DeleteMainSection == null)
            {
                ModelState.AddModelError(string.Empty, "لايوجد قسم هنا .");
                return View();
            }
            var BrandFk = DeleteMainSection.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            var deleteDeviceToolsPicture = _unitOfWork.DevicesAndTools.GetAll().Where(u => u.BrandFK == BrandFk).ToList();

            if (deleteDeviceToolsPicture != null)
            {
                for (int i = 0; i < deleteDeviceToolsPicture.Count(); i++)
                {
                    var delet = deleteDeviceToolsPicture[i];

                    string DeviceToolsID = delet.DevicesAndToolsID.ToString();

                    // Delete the associated image file
                    if (!string.IsNullOrEmpty(delet.DevicesAndTools_Image))
                    {
                        string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DeviceToolsID, delet.DevicesAndTools_Image);
                        if (System.IO.File.Exists(imagePath1))
                        {
                            System.IO.File.Delete(imagePath1);
                        }
                    }

                    _unitOfWork.DevicesAndTools.Remove(delet);
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
        public IActionResult DeleteSectionDeviceToolsAdd(int? id)
        {
            var DeleteMainSection = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == id);
            if (DeleteMainSection == null)
            {
                ModelState.AddModelError(string.Empty, "لايوجد قسم هنا .");
                return View();
            }
            var BrandFk = DeleteMainSection.BrandFK;

            string wwwRootPathSteps = _webHostEnvironment.WebRootPath;

            var deleteDeviceToolsPicture = _unitOfWork.DevicesAndTools.GetAll().Where(u => u.BrandFK == BrandFk).ToList();

            if (deleteDeviceToolsPicture != null)
            {
                for (int i = 0; i < deleteDeviceToolsPicture.Count(); i++)
                {
                    var delet = deleteDeviceToolsPicture[i];

                    string DeviceToolsID = delet.DevicesAndToolsID.ToString();

                    // Delete the associated image file
                    if (!string.IsNullOrEmpty(delet.DevicesAndTools_Image))
                    {
                        string imagePath1 = Path.Combine(wwwRootPathSteps, "IMAGES", DeviceToolsID, delet.DevicesAndTools_Image);
                        if (System.IO.File.Exists(imagePath1))
                        {
                            System.IO.File.Delete(imagePath1);
                        }
                    }

                    _unitOfWork.DevicesAndTools.Remove(delet);
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
        public async Task<IActionResult> RedirectToDeviceToolsAdminUpdate(LoginTredMarktViewModel viewModel)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                var sectionsList = viewModel.MainsectionVMlist.ToList(); // Convert to list to allow indexing
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                for (int i = 0; i < sectionsList.Count; i++)
                {
                    var sections = sectionsList[i];

                    // تحقق إذا كان اسم القسم هو "الأجهزة والأدوات"
                    if (sections.SectionsName == "الأجهزة والأدوات")
                    {
                        sections.IsChecked = true;
                        sections.MainSectionsOrder = sections.MainSectionsID;
                        var SectionPath = Path.Combine(wwwRootPath, "IMAGES", sections.MainSectionsID.ToString());
                        var fileName = $"file_{i}";
                        var fileSections = HttpContext.Request.Form.Files[fileName];

                        if (fileSections != null || sections.IsChecked) // Check if the section is checked
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

                        // الخروج من اللوب بعد تنفيذ العمل للقسم "الأجهزة والأدوات"
                        break;
                    }
                }
                _unitOfWork.Save();
                //TempData["success"] = "تم إضافة قسم الأجهزة والأدوات بنجاح";
                return RedirectToAction("RedirectToDeviceToolsAdminList", new { brandFk = viewModel.TredMarktVM.BrandID });
            }

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddSectionDeviceToolsUpdate(string sectionName, int BrandID)
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
        public IActionResult AddSectionDeviceTools(string sectionName, int BrandID)
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
