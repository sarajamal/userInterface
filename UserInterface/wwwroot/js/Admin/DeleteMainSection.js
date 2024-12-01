function deleteSectionupdate(id, sectionName,BrandID) {
    // تحديد رابط URL بناءً على اسم القسم
    let deleteUrl = '';

    switch (sectionName) {
        case 'التحضيرات':
            deleteUrl = `/Admin/SectionDetailsPreparation/DeleteSectionPreparationUpdate?id=${id}&BrandID=${BrandID}`;
            break;
        case 'الإنتاج':
            deleteUrl = `/Admin/SectionDetailsProduction/DeleteSectionProductionUpdate?id=${id}&BrandID=${BrandID}`;
            break;
        case 'المواد الغذائية':
            deleteUrl = '/admin/SectionDetailsFoods/DeleteSectionFoodUpdate/' + id;
            break;
        case 'الأجهزة والأدوات':
            deleteUrl = '/Admin/SectionDetailsDevice_Tools/DeleteSectionDeviceToolsUpdate/' + id;
            break;
        case 'المنتجات الجاهزة':
            deleteUrl = '/Admin/SectionDetailsFinishProducts/DeleteSectionFinishUpdate/' + id;
            break;
        default:
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'اسم القسم غير صحيح'
            });
            return;
    }

    Swal.fire({
        title: 'تأكيد !!!!!',
        text: " هل أنت متأكد من حذف القسم بشكل كامل؟  ",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: deleteUrl,
                success: function (data) {
                    if (data.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'تم الحذف بنجاح',
                            text: data.message
                        }).then(() => {
                            window.location.href = data.redirectToUrl; // Perform the redirection
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'خطأ',
                            text: data.message
                        });
                    }
                }
            });
        }
    });
}

function deleteSectionAdd(id, sectionName, BrandID) {
    // تحديد رابط URL بناءً على اسم القسم
    let deleteUrl = '';

    switch (sectionName) {
        case 'التحضيرات':
            deleteUrl = `/Admin/SectionDetailsPreparation/DeleteSectionPreparationAdd?id=${id}&BrandID=${BrandID}`;
            break;
        case 'الإنتاج':
            deleteUrl = `/Admin/SectionDetailsProduction/DeleteSectionProductionAdd?id=${id}&BrandID=${BrandID}`;
            break;
        case 'المواد الغذائية':
            deleteUrl = '/admin/SectionDetailsFoods/DeleteSectionFoodAdd/' + id;
            break;
        case 'الأجهزة والأدوات':
            deleteUrl = '/Admin/SectionDetailsDevice_Tools/DeleteSectionDeviceToolsAdd/' + id;
            break;
        case 'المنتجات الجاهزة':
            deleteUrl = '/Admin/SectionDetailsFinishProducts/DeleteSectionFinishAdd/' + id;
            break;
        default:
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'اسم القسم غير صحيح'
            });
            return;
    }

    Swal.fire({
        title: 'تأكيد !!!!!',
        text: " هل أنت متأكد من حذف القسم بشكل كامل؟  ",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: deleteUrl,
                success: function (data) {
                    if (data.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'تم الحذف بنجاح',
                            text: data.message
                        }).then(() => {
                            window.location.href = data.redirectToUrl; // Perform the redirection
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'خطأ',
                            text: data.message
                        });
                    }
                }
            });
        }
    });
}

function addSection(id, sectionName, BrandID) {
    // تحديد رابط URL بناءً على اسم القسم
    let addUrl = '';

    switch (sectionName) {
        case 'التحضيرات':
            addUrl = `/Admin/SectionDetailsPreparation/AddSectionPreparationUpdate?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'الإنتاج':
            addUrl = `/Admin/SectionDetailsProduction/AddSectionProductionUpdate?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'المواد الغذائية':
            addUrl = `/admin/SectionDetailsFoods/AddSectionFoodUpdate?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'الأجهزة والأدوات':
            addUrl = `/Admin/SectionDetailsDevice_Tools/AddSectionDeviceToolsUpdate?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'المنتجات الجاهزة':
            addUrl = `/Admin/SectionDetailsFinishProducts/AddSectionFinishUpdate?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        default:
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'اسم القسم غير صحيح'
            });
            return;
    }

    // تنفيذ طلب POST باستخدام AJAX
    $.ajax({
        url: addUrl, // استخدام المتغير الصحيح لإضافة القسم
        type: 'POST',
        data: {
            BrandID: BrandID,
            sectionName: sectionName
        },
        success: function (data) {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'تمت إضافة قسم ' + sectionName + ' بنجاح',
                }).then(() => {
                    window.location.href = data.redirectToUrl; // إعادة التوجيه باستخدام URL المستلم
                });
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'فشل في إضافة القسم: ' + sectionName
            }).then(() => {
                // إعادة التوجيه في حالة الخطأ
                if (xhr.responseJSON && xhr.responseJSON.redirectToUrl) {
                    window.location.href = xhr.responseJSON.redirectToUrl;
                }
            });
        }
    });
}

function addSectionAdd(id, sectionName, BrandID) {
    // تحديد رابط URL بناءً على اسم القسم
    let addUrl = '';

    switch (sectionName) {
        case 'التحضيرات':
            addUrl = `/Admin/SectionDetailsPreparation/AddSectionPreparation?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'الإنتاج':
            addUrl = `/Admin/SectionDetailsProduction/AddSectionProduction?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'المواد الغذائية':
            addUrl = `/admin/SectionDetailsFoods/AddSectionFood?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'الأجهزة والأدوات':
            addUrl = `/Admin/SectionDetailsDevice_Tools/AddSectionDeviceTools?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        case 'المنتجات الجاهزة':
            addUrl = `/Admin/SectionDetailsFinishProducts/AddSectionFinish?id=${id}&sectionName=${sectionName}&BrandID=${BrandID}`;
            break;
        default:
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'اسم القسم غير صحيح'
            });
            return;
    }

    // تنفيذ طلب POST باستخدام AJAX
    $.ajax({
        url: addUrl, // استخدام المتغير الصحيح لإضافة القسم
        type: 'POST',
        data: {
            BrandID: BrandID,
            sectionName: sectionName
        },
        success: function (data) {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'تمت إضافة قسم ' + sectionName + ' بنجاح',
                }).then(() => {
                    window.location.href = data.redirectToUrl; // إعادة التوجيه باستخدام URL المستلم
                });
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            Swal.fire({
                icon: 'error',
                title: 'خطأ',
                text: 'فشل في إضافة القسم: ' + sectionName
            }).then(() => {
                // إعادة التوجيه في حالة الخطأ
                if (xhr.responseJSON && xhr.responseJSON.redirectToUrl) {
                    window.location.href = xhr.responseJSON.redirectToUrl;
                }
            });
        }
    });
}
function AlertEnter(sectionName) {
    Swal.fire({
        title: 'الرجاء اختيار القسم أولا ',
        icon: 'warning',
        confirmButtonText: 'حسنًا'
    });
}
