﻿
function DeleteToolVariety2(id) { //هذي فقط للعرض البرمجة في controller , هذي للحذف بعد الحفظ في قاعدة البيانات . 
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
                url: '/Production/DeleteToolVariety2/' + id, // Use the provided ID parameter
                type: 'DELETE',
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


function AddRowTool2(ProductionFK) { //صفحة التعديل 
    var tableBody = document.querySelector("#tblToolVarity2 tbody");
    var newRowNumber = tableBody.children.length + 1;

    var newRow = document.createElement("tr");
    newRow.innerHTML = `
        <td>${newRowNumber}</td>

        <td><input name="ToolsVarityVM2[${newRowNumber - 1}].ProdTools" class="form-control" /></td>

       <td style="text-align:center;" >
            <input type="hidden" name="ToolsVarityVM2[${newRowNumber - 1}].ProductionFK" value="${ProductionFK}" />
            <button type="button" class="btn btn-danger" data-row-index="${newRowNumber}" onclick="DeleteRow9(this)">حذف</button>
        </td>
         `;
    tableBody.appendChild(newRow);
}

function DeleteRow9(button) { // AJAX قبل تحفظ في قاعدة البيانات ماتحتاج controller 
  
    Swal.fire({
        title: 'هل أنت متأكد؟',
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'نعم!'
    }).then((result) => {
        if (result.isConfirmed) {
            var tableBody = document.querySelector("#tblToolVarity2 tbody");
            var rows = tableBody.children;
            button.closest("tr").remove();

            // Update row numbers
            for (var i = 0; i < rows.length; i++) {
                rows[i].cells[0].textContent = i + 1;
            }

            Swal.fire('تم الحذف!', 'تم الحذف بنجاح!', 'success');
        }
    });
}
document.querySelector("#tblToolVarity2 tbody").addEventListener("click", function (event) {
    if (event.target.classList.contains("data-row-index")) {
        DeleteRow3(event.target);
    }
});

var newRowNumber = 1;
var numeric = 2;
function AddRowToolnew2(ProductionFK) { //صفحة الإضافة
    var tableBody = document.querySelector("#tblToolVarity2 tbody");

    var newRow = document.createElement("tr");
    newRow.innerHTML = `

        <td style="text-align:center;">${numeric}</td>
        <td><input name="ToolsVarityVM2[${newRowNumber - 1}].ProdTools" class="form-control"  /></td>
       <td style="text-align:center;" >
            <input type="hidden" name="ToolsVarityVM2[${newRowNumber - 1}].ProductionFK" value="${ProductionFK}" />
              
            <button type="button" class="btn btn-danger" data-row-index="${newRowNumber}" onclick="DeleteRow9(this)">حذف</button> 
        </td>
         `;

    tableBody.appendChild(newRow);
    newRowNumber++;
    numeric++;
}