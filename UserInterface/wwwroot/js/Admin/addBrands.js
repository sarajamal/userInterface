$(function () {
    // Retrieve the id value from the data attribute in the thead element
    var ID = document.querySelector("thead").getAttribute("data-id");
    loadDataTable(ID);
});
function loadDataTable(ID) {

    // Create a <style> element
    var style = document.createElement('style');
    // Define the CSS rules for your custom class
    style.innerHTML = `
        .custom-font-bold {
             background: #676869;
             color: #fff;
             font-family: 'Almarai', sans-serif;
             vertical-align:middle;
        }
    `;

    // Append the <style> element to the document's <head>
    document.head.appendChild(style);

    dataTable = $('#tblBrands').dataTable({
        "ajax": {
            "url": `/admin/Index/getallbrands?ID=${ID}`, // Include the received ID in the URL
            "type": "GET",
            "datatype": "json"
        },
        "columns": [

            {
                data: 'brandName',
                "width": "15%",
                "className": "text-center"
            },
            {
                data: 'brandCoverImage',

                "render": function (data, _, row) {

                    var brandID = parseInt(row.brandID, 10);
                    var imagePath2 = `/IMAGES/${brandID}/${row.brandCoverImage}`;

                    // Customize the content of the cell with both text and image
                    return `<img src="${imagePath2}" alt="Image" loading="lazy" style="
                   width: auto;  height: auto; max-width: 150px;
                   max-height: 150px; object-fit: contain; display: block; margin-left: auto; margin-right: auto;"/>`;
                },
                "width": "15%",
                "className": "text-center"
            },
            {
                data: 'brandLogoImage',

                "render": function (data, _, row) {
                    var brandID = parseInt(row.brandID, 10);
                    var imagePath2 = `/IMAGES/${brandID}/${row.brandLogoImage}`;

                    // Customize the content of the cell with both text and image
                    return `<img src="${imagePath2}" alt="Image" style="
                   width: auto;  height: auto; max-width: 150px;
                   max-height: 130px; object-fit: contain; display: block; margin-left: auto; margin-right: auto;"/>`;
                },
                "width": "15%",
                "className": "text-center"
            },
            {
                data: 'brandFooterImage',

                "render": function (data, _, row) {
                    var brandID = parseInt(row.brandID, 10);
                    var imagePath2 = `/IMAGES/${brandID}/${row.brandFooterImage}`;

                    // Customize the content of the cell with both text and image
                    return `<img src="${imagePath2}" alt="Image" style="
                   width: auto;  height: auto; max-width: 150px;
                   max-height: 190px; object-fit: contain; display: block; margin-left: auto; margin-right: auto;"/>`;
                },
                "width": "15%",
                "className": "text-center"
            },
            {
                data: 'createdBY',
                "width": "7%",
                "className": "text-center"
            },
            {
                data: 'expirationDate',
                "render": function (data, _, row) {
                    // التحقق من وجود تاريخ انتهاء الصلاحية أولاً
                    if (row.user && row.user.expirationDate) {
                        // التحقق مما إذا كان تاريخ انتهاء الصلاحية لا يزال صالحًا
                        if (new Date(row.user.expirationDate) >= new Date()) {
                            return `<span class="text-success">نشط</span>`;
                        } else {
                            return `<span class="text-danger">غير نشط</span>`;
                        }
                    } else {
                        // في حالة عدم وجود بيانات تاريخ انتهاء الصلاحية
                        return `<span class="text-muted">لا يوجد حساب لديه</span>`;
                    }
                },
                "width": "7%",
                "className": "text-center"
            },
            {
                data: 'id',
                "render": function (data, type, row) {
                    // Assuming 'row' has a property for BrandFK
                    var brandID = row.brandID;

                    return `
                        <div role="group">
                            <a href="/admin/Index/RedirectToUpdateBrands?brandFK=${brandID}" class="btn btn-style4 fnt-white mx-2">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                           <button onclick="printInBackground(${brandID})" class="btn btn-custom-blue fnt-white mx-2">
                                <i class="bi bi-printer"></i>
                           </button>
                        </div>`;
                },
                "width": "7%",
                "className": "text-center"
            },
        ],
    });
}
function printInBackground(brandID) {
    var printUrl = `/admin/Index/PrintBrands?brandfk=${brandID}`;
    window.open(printUrl, '_blank');
}
function DeletePreparationPost(url) {
    Swal.fire({
        title: 'هل أنت متأكد ؟',
        text: " هل تريد استعادة ماتم حذفه؟",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url, // Use the provided ID parameter
                //type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'تم الحذف بنجاح',
                            text: data.message
                        }).then(() => {
                            location.reload(); // Reload the page after successful deletion
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
    })
}
