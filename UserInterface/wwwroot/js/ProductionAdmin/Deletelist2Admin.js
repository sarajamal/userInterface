﻿
$(function () {
    var id = document.querySelector("thead").getAttribute("data-id");
    loadDataTable404(id);
});

function loadDataTable404(id) {

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

    dataTable = $('#tblData').dataTable({
        "ajax": {
            "url": `/admin/SectionDetailsProduction/GetAll?id=${id}`,
        },
        "columns": [
            {
                data: 'productName',
                "width": "20%",
                "className": "text-center "
            },
            {
                data: 'productType',
                "width": "20%",
                "className": "text-center "
            },
            {
                data: 'productImage',
                "render": function (data, _, row) {
                    var numericID = parseInt(row.brandFK, 10); // Extract numeric part
                    var numericID2 = parseInt(row.productionID, 10); // Extract numeric part

                    var imagePath = `/IMAGES/${numericID2}/${data}`;
                    return `<img src="${imagePath}" alt="Image" width="150" height="100"/>`;
                },
                "width": "40%",
                "className": "text-center"
            },
            {
                data: 'productionID',
                "render": function (data, type, row) {
                    // Assuming 'row' has a property for BrandFK
                    var brandFk = row.brandFK;
                    var deleteButton = '';
                    if (userRole === "true") { // Only show delete button if the user is a Manager
                        deleteButton = `  <a onClick=DeleteProductionPost('/admin/SectionDetailsProduction/DeleteProductionPost/${data}') class="btn btn-style5 "> <i class="bi bi-trash-fill"></i>
                        </a>`;
                    }

                    return `<div role="group">
                     <a href="/admin/SectionDetailsProduction/RedirectToUpdateAdminInformation1?ProductionID=${data}&brandFK=${brandFk}" class="btn btn-style4 ftn-white mx-2"> 
                     <i class="bi bi-pencil-square"></i>
                     </a>
                     ${deleteButton} <!-- Show delete button only if the user is a Manager -->
                    </div>`;
                },
                "width": "16%",
                "className": "text-center"
            },
            {
                data: 'productionOrder', // Assuming 'Order' is the name of your 'Order' column
                "visible": false, // Hide the "Order" column from the user interface
                "orderable": false // Disable sorting for the "Order" column
            }
        ],
        "order": [] // Disable initial sorting
    });
}
function DeleteProductionPost(url) {
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