
$(function () {
     
    // Retrieve the id value from the data attribute in the thead element
    var id = document.querySelector("thead").getAttribute("data-id");
    loadDataTable77(id);
});
function loadDataTable77(id) {

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

    dataTable = $('#tblDatadevice').dataTable({
        "ajax": {
            "url": `/admin/SectionDetailsDevice_Tools/GetAll?id=${id}`, // Include the received ID in the URL
        },
        "columns": [
            {
                data: 'devicesAndTools_Name',
                "width": "35%",
                "className": "text-center"
 
            },
        {
                data: 'devicesAndTools_Image',
                    "render": function (data, _, row) {
                        var numericID = parseInt(row.devicesAndToolsID, 10);
                        var numericFK = parseInt(row.brandFK, 10);

                        // Adjusted imagePath2 to point to the external server
                        var imagePath2 = `/IMAGES/${numericID}/${row.devicesAndTools_Image}`;

                        // Customize the content of the cell with both text and image
                        return `<img src="${imagePath2}" alt="Image" width="150" height="100"/>`;
                    },
                "width": "45%",
                    "className": "text-center"
            },

            {
                data: 'devicesAndToolsID',
                "render": function (data) {
                    // Conditionally display the delete button based on the user's role
                    var deleteButton = '';
                    if (userRole === "true") { // Only show delete button if the user is a Manager
                        deleteButton = `<a onClick=DelteToolsdevice77('/admin/SectionDetailsDevice_Tools/DelteToolsdevice77/${data}') 
                        class="btn btn-style5 " > <i class="bi bi-trash-fill"></i></a > `;
                    }
                    return `<div role="group">
                    <button type="button" class="btn btn-style4 fnt-white px-4 device-index-button"
                            data-toggle="modal"
                            data-target="#IndexDeviceToolsAdmin"
                            data-controller="SectionDetailsDevice_Tools"
                            data-action="IndexDeviceToolsAdmin"
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
                data: 'devicesAndToolsOrder', // Assuming 'Order' is the name of your 'Order' column
                "visible": false, // Hide the "Order" column from the user interface
                "orderable": false // Disable sorting for the "Order" column
            }
        ],
        "order": [] // Disable initial sorting
    });
}
function DelteToolsdevice77(url) {
    console.log("DelteToolsdevice function called with URL:", url); // Add this line

    Swal.fire({
        title: 'هل أنت متأكد ؟',
        text: "الاجهزة والأدوات  هل تريد استعادة ماتم حذفه؟",
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

//كود عرض الصور من السيرفر اذا كانت string
//{
//    data: 'devicesAndTools_Image',
//        "render": function (data, _, row) {
//            var numericID = parseInt(row.devicesAndToolsID, 10);
//            var numericFK = parseInt(row.brandFK, 10);

//            // Adjusted imagePath2 to point to the external server
//            var imagePath2 = `https://manuals.befranchisor.com/IMAGES/${numericFK}/DevicesAndTools/${numericID}/${row.devicesAndTools_Image}`;

//            // Customize the content of the cell with both text and image
//            return `<img src="${imagePath2}" alt="Image" width="150" height="100"/>`;
//        },
//    "width": "45%",
//        "className": "text-center"
//},

//{
//    data: 'devicesAndTools_Image',
//        "render": function (data, _, row) {
//            // Check if data is not null or undefined and it is actually a byte array
//            if (data && data instanceof Array) {
//                // Convert byte array to Base64 string
//                var base64String = btoa(String.fromCharCode.apply(null, new Uint8Array(data)));

//                // Determine the MIME type of the image from the byte array (magic numbers)
//                var mimeType = 'image/jpeg'; // Default to JPEG
//                if (data.length > 3) {
//                    // Check if it's a PNG
//                    if (data[0] === 0x89 && data[1] === 0x50 && data[2] === 0x4E && data[3] === 0x47) {
//                        mimeType = 'image/png';
//                    }
//                    // Other checks can be added here for different MIME types if needed
//                }

//                // Use the Base64 string as the source for the image with the determined MIME type
//                return `<img src="data:${mimeType};base64,${base64String}" alt="Image" width="150" height="100"/>`;
//            } else {
//                // If data is not a byte array, this code path shouldn't be reached. 
//                // You might want to handle this case differently.
//                return 'لايوجد صورة'; // Placeholder for when the image data isn't in the expected byte array format.
//            }

//        },
//    "width": "45%",
//        "className": "text-center"