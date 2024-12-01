
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Test12.DataAccess.Repository.IRepository;
using Test12.Models.Models;
using Test12.Models.Models.Preparation;
using Test12.Models.Models.trade_mark;
using Test12.Models.ViewModel;

namespace UserInterface.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SectionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionsController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;

        }
        /*--------------------------UpdateMainSection--------------------------------------------*/
        public IActionResult RedirectToUpdateMainSection(int brandFK, int mainSectionId)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId);
            HttpContext.Session.SetInt32("BrandFK", brandFK);
            TempData.Keep("BrandFK");
            return RedirectToAction("UpdateMainSection");

        }
        public IActionResult UpdateMainSection()
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");

            LoginTredMarktViewModel viewModel = new()
            {
                MainsectionVM = new MainSections(),
                TredMarktVM = new Brands(),
                MainsectionVMlist = new List<MainSections>(),
                PreparationVM = new Preparations()
            };

            viewModel.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);
            viewModel.PreparationVM = _unitOfWork.PreparationRepository.Get(u => u.BrandFK == brandFK);
            // Get existing sections
            var existingSections = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();

            foreach (var setIsCheck in existingSections)
            {
                setIsCheck.IsChecked = true;
            }

            // Define new sections to add if they don't already exist
            var newSections = new List<MainSections>
    {
       
        new MainSections { SectionsName = "الأجهزة والأدوات", BrandFK = brandFK },
        new MainSections { SectionsName = "المواد الغذائية", BrandFK = brandFK },
        new MainSections { SectionsName = "التحضيرات", BrandFK = brandFK },
        new MainSections { SectionsName = "الإنتاج", BrandFK = brandFK },
        new MainSections { SectionsName = "المنتجات الجاهزة", BrandFK = brandFK },
    };

            // Add new sections to the list if they don't already exist
            foreach (var newSection in newSections)
            {
                if (!existingSections.Any(s => s.SectionsName == newSection.SectionsName))
                {
                    existingSections.Add(newSection);
                }
            }

            // Sort existingSections based on the order in newSections
            viewModel.MainsectionVMlist = newSections
                .Select(ns => existingSections.FirstOrDefault(es => es.SectionsName == ns.SectionsName) ?? ns)
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMainSection(LoginTredMarktViewModel viewModel)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                var sectionsList = viewModel.MainsectionVMlist.ToList(); // Convert to list to allow indexing
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                for (int i = 0; i < sectionsList.Count; i++)
                {
                    var sections = sectionsList[i];
                    var SectionPath = Path.Combine(wwwRootPath, "IMAGES", sections.MainSectionsID.ToString());
                    var fileName = $"file_{i}";
                    var fileSections = HttpContext.Request.Form.Files[fileName];

                    if (fileSections != null || sections.IsChecked) // Check if the section is checked
                    {
                        if (fileSections != null)
                        {

                            try
                            {
                                if (sections.MainSectionsID != 0)
                                {

                                    sections.MainSectionsOrder = sections.MainSectionsID;
                                    sections.IsChecked = true;

                                    _unitOfWork.Save();
                                }
                                else
                                {
                                    _unitOfWork.MainsectionRepository.Add(sections);
                                    _unitOfWork.Save();

                                    sections.MainSectionsOrder = sections.MainSectionsID;
                                    sections.IsChecked = true;

                                    _unitOfWork.Save();
                                }

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
                        else if (sections.MainSectionsID != 0)
                        {

                            sections.MainSectionsOrder = sections.MainSectionsID;
                            sections.IsChecked = true;
                            _unitOfWork.Save();
                        }
                        else
                        {
                            _unitOfWork.MainsectionRepository.Add(sections);
                            _unitOfWork.Save();

                            sections.MainSectionsOrder = sections.MainSectionsID;
                            sections.IsChecked = true;

                            _unitOfWork.Save();
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
                }
                _unitOfWork.Save();
                TempData["success"] = "تم حفظ التغييرات بنجاح";
                return RedirectToAction("RedirectToUpdateMainSection", new { brandFk = viewModel.TredMarktVM.BrandID });
            }

            return View(viewModel);
        }
        /*---------------------------------------------------------------------------------------*/
        /*--------------------------AddMainSection-----------------------------------------------*/
        public IActionResult RedirectToAddMainSection(int brandFK, int mainSectionId)
        {
            // تخزين البيانات في الجلسة
            HttpContext.Session.SetInt32("mainSectionId", mainSectionId);
            HttpContext.Session.SetInt32("BrandFK", brandFK);
            TempData.Keep("BrandFK");
            return RedirectToAction("AddMainSection");

        }
        public IActionResult AddMainSection()
        {
            // استرجاع البيانات من الجلسة
            int? brandFK = HttpContext.Session.GetInt32("BrandFK");
            int? mainSectionId = HttpContext.Session.GetInt32("mainSectionId");

            LoginTredMarktViewModel viewModel = new()
            {
                MainsectionVM = new MainSections(),
                TredMarktVM = new Brands(),
                MainsectionVMlist = new List<MainSections>(),
            };

            viewModel.TredMarktVM = _unitOfWork.TredMarketRepository.Get(u => u.BrandID == brandFK);

            // Get existing sections
            var existingSections = _unitOfWork.MainsectionRepository.GetAll().Where(u => u.BrandFK == brandFK).ToList();
            foreach (var setIsCheck in existingSections)
            {
                setIsCheck.IsChecked = true;
            }

            // Define new sections to add if they don't already exist
            var newSections = new List<MainSections>
    {
         new MainSections { SectionsName = "الأجهزة والأدوات", BrandFK = brandFK },
        new MainSections { SectionsName = "المواد الغذائية", BrandFK = brandFK },
        new MainSections { SectionsName = "التحضيرات", BrandFK = brandFK },
        new MainSections { SectionsName = "الإنتاج", BrandFK = brandFK },
        new MainSections { SectionsName = "المنتجات الجاهزة", BrandFK = brandFK },
    };

            // Add new sections to the list if they don't already exist
            foreach (var newSection in newSections)
            {
                if (!existingSections.Any(s => s.SectionsName == newSection.SectionsName))
                {
                    existingSections.Add(newSection);
                }
            }

            // Sort existingSections based on the order in newSections
            viewModel.MainsectionVMlist = newSections
                .Select(ns => existingSections.FirstOrDefault(es => es.SectionsName == ns.SectionsName) ?? ns)
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddMainSection(LoginTredMarktViewModel viewModel)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (ModelState.IsValid)
            {
                var brandId = viewModel.TredMarktVM.BrandID.ToString();
                var sectionsList = viewModel.MainsectionVMlist.ToList();

                for (int i = 0; i < sectionsList.Count; i++)
                {
                    var sections = sectionsList[i];
                    var fileName = $"file_{i}";
                    var fileSections = HttpContext.Request.Form.Files[fileName];

                    if (fileSections != null || sections.IsChecked)
                    {
                        if (fileSections != null)
                        {
                            try
                            {
                                if (sections.MainSectionsID == 0)
                                {
                                    _unitOfWork.MainsectionRepository.Add(sections);
                                    _unitOfWork.Save();

                                    sections.MainSectionsOrder = sections.MainSectionsID;
                                    sections.IsChecked = true;
                                    _unitOfWork.Save();
                                }
                                else
                                {
                                    sections.MainSectionsOrder = sections.MainSectionsID;
                                    sections.IsChecked = true;
                                    _unitOfWork.Save();
                                }

                                var sectionPath = Path.Combine(wwwRootPath, "IMAGES", sections.MainSectionsID.ToString());

                                // حذف الصورة القديمة إذا كانت موجودة
                                if (!string.IsNullOrEmpty(sections.SectionsImage))
                                {
                                    var oldImagePath = Path.Combine(sectionPath, sections.SectionsImage);
                                    if (System.IO.File.Exists(oldImagePath))
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                    }
                                }

                                string fileNamesection = Guid.NewGuid().ToString() + Path.GetExtension(fileSections.FileName);

                                if (!Directory.Exists(sectionPath))
                                {
                                    Directory.CreateDirectory(sectionPath);
                                }

                                // ضغط الصورة وحفظها
                                string compressedImagePath = Path.Combine(sectionPath, fileNamesection);
                                await CompressAndSaveImage(fileSections, compressedImagePath);

                                sections.SectionsImage = fileNamesection;
                                _unitOfWork.MainsectionRepository.Update(sections);
                                _unitOfWork.Save();
                            }
                            catch (Exception ex)
                            {
                                // تسجيل الخطأ
                                ModelState.AddModelError("", "Error while uploading the image.");
                                return View(viewModel);
                            }
                        }
                        else if (sections.MainSectionsID != 0)
                        {
                            sections.MainSectionsOrder = sections.MainSectionsID;
                            sections.IsChecked = true;
                            _unitOfWork.Save();
                        }
                        else
                        {
                            _unitOfWork.MainsectionRepository.Add(sections);
                            _unitOfWork.Save();

                            sections.MainSectionsOrder = sections.MainSectionsID;
                            sections.IsChecked = true;

                            _unitOfWork.Save();
                        }
                    }
                    else
                    {
                        var existingSections = _unitOfWork.MainsectionRepository.Get(u => u.MainSectionsID == sections.MainSectionsID);
                        if (existingSections != null)
                        {
                            existingSections.SectionsImage = sections.SectionsImage;
                            existingSections.MainSectionsOrder = sections.MainSectionsID;
                            sections.IsChecked = true;
                            _unitOfWork.MainsectionRepository.Update(existingSections);
                            _unitOfWork.Save();
                        }
                    }
                }

                TempData["success"] = "تم إضافة الأقسام بنجاح";
                return RedirectToAction("RedirectToAddMainSection", new { brandFk = viewModel.TredMarktVM.BrandID });
            }

            return View(viewModel);
        }

        // طريقة لضغط الصورة وحفظها باستخدام SixLabors.ImageSharp
        private async Task CompressAndSaveImage(IFormFile file, string outputPath)
        {
            using (var image = Image.Load(file.OpenReadStream()))
            {
                // ضبط الحجم الأقصى للصورة (اختياري)
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(800, 800) // تحديد الحجم الأقصى للصورة
                }));

                // حفظ الصورة بالجودة المحددة
                await image.SaveAsync(outputPath, new JpegEncoder
                {
                    Quality = 75 // مستوى الضغط (0-100)
                });
            }
        }

        /*---------------------------------------------------------------------------------------*/


    }
}

