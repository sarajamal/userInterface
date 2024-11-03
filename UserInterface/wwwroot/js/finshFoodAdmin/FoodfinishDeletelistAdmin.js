$(function () {
    // Retrieve the id value from the data attribute in the thead element
    var id = document.querySelector("thead").getAttribute("data-id");
    loadDataTable99(id);
});
function loadDataTable99(id) {

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

    dataTable = $('#tblFoodfinish').dataTable({
        "ajax": {
            "url": `/admin/SectionDetailsFinishProducts/GetAll?id=${id}`, // Include the received ID in the URL
        },
        "columns": [
            {
                data: 'readyProductsName',
                "width": "40%",
                "className": "text-center"
            },
            {
                data: 'readyProductsImage',
                "render": function (data, _, row) {
                    var numericID = parseInt(row.readyProductsID, 10);
                    var numericFK = parseInt(row.brandFK, 10);

                    // Include the full URL of the external server in imagePath3
                    var imagePath3 = `/IMAGES/${numericID}/${data}`;

                    return `<img src="${imagePath3}" alt="Image" width="150" height="100"/>`;
                },
                "width": "44%",
                "className": "text-center"
            },
            {
                data: 'readyProductsID',
                "render": function (data) {
                    // Conditionally display the delete button based on the user's role
                    var deleteButton = '';
                    if (userRole === "true") { // Only show delete button if the user is a Manager
                        deleteButton = `<a onClick=DeleteFinshFoodAdmin('/admin/SectionDetailsFinishProducts/DeleteFinshFoodAdmin/${data}') class="btn btn-style5 "> <i class="bi bi-trash-fill"></i>
                        </a>`;
                    }
                    return `<div role="group">
                       <button type="button" class="btn btn-style4 fnt-white px-4 finsh-index-button"
                            data-toggle="modal"
                            data-target="#FinishProductIndexAdmin"
                            data-controller="SectionDetailsFinishProducts"
                            data-action="FinishProductIndexAdmin"
                            data-id="${data}">   
                            <i class="bi bi-pencil-square"></i> 
                       </button>    
                        ${deleteButton} <!-- Show delete button only if the user is a Manager -->

                    </div>`;
                },
                "width": "20%",
                "className": "text-center"
            },
            {
                data: 'readyProductsOrder', // Assuming 'Order' is the name of your 'Order' column
                "visible": false, // Hide the "Order" column from the user interface
                "orderable": false // Disable sorting for the "Order" column
            }
        ],
        "order": [] // Disable initial sorting
    });
}

function DeleteFinshFoodAdmin(url) {
    Swal.fire({
        title: 'هل أنت متأكد ؟',
        text: " في المنتجات الجاهزة هل تريد استعادة ماتم حذفه؟",
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

