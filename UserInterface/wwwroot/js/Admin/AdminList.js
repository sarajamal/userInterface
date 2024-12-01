var dataTable;
var userRole = '@(User.IsInRole(SD.Role_Manager) ? "true" : "false")'; // This is now a string ("true" or "false")

$(function () {
    loadDataTable();
});

function loadDataTable() {
    // Create a <style> element for custom styles
    var style = document.createElement('style');
    style.innerHTML = `
        .custom-font-bold {
             background: #676869;
             color: #fff;
             font-family: 'Almarai', sans-serif;
             vertical-align: middle;
        }
    `;
    document.head.appendChild(style); // Append the style to the document's <head>

    dataTable = $('#tblUsers').dataTable({
        "ajax": {
            "url": '/admin/Index/getall', // Include the received ID in the URL
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                data: 'userName',
                "width": "25%",
                "className": "text-center"
            },
            {
                data: 'email',
                "width": "25%",
                "className": "text-center"
            },
            {
                data: 'roleName',
                "width": "25%",
                "className": "text-center"
            },
            {
                data: 'id',
                "render": function (data, type, row) {
                    var Id = row.id;
                    var username = row.userName;

                    // Conditionally display the delete button based on the userRole variable
                    var deleteButton = '';
                    if (userRole === "true") {
                        deleteButton = `
                            <button type="button"onClick="DeleteUser('${data}')" class="btn btn-style5">
                                <i class="bi bi-trash-fill"></i>
                            </button>`;
                    }

                    return `
                        <div role="group">
                            <a href="/Identity/Account/UpdateUsers/?username=${username}" class="btn btn-style4 fnt-white mx-2">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            ${deleteButton} <!-- Only show delete button if the user is Manager -->
                        </div>`;
                },
                "width": "16%",
                "className": "text-center"
            }
        ],
    });
}

function DeleteUser(userId) {
    Swal.fire({
        title: 'هل أنت متأكد من حذف المستخدم ؟',
        text: "لن تتمكن من استعادة هذا المستخدم بعد الحذف!",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST', // تأكد من أن الطلب يتم باستخدام POST
                url: '/admin/Index/DeleteUser',  // تأكد من تطابق الـ URL مع الإجراء في الـ Controller
                data: { id: userId }, // تمرير معرف المستخدم كجزء من البيانات
                success: function (data) {
                    if (data.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'تم الحذف بنجاح',
                            text: data.message
                        }).then(() => {
                            location.reload(); // إعادة تحميل الصفحة بعد الحذف
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'خطأ',
                            text: data.message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'خطأ',
                        text: 'حدث خطأ أثناء محاولة حذف المستخدم.'
                    });
                }
            });
        }
    });
}
