﻿var clickCount = 0;
var lastID = 0; // Initialize lastID globally
function AddnewRowstepsUpdate2(ProductionFK) { //صفحة التعديل

    
    $.ajax({
        url: '/customer/Production/GetAddID',
        type: 'POST',
        data: {
            ProductionFK: ProductionFK,
            PropaVM: {} // Pass necessary data if needed
        },
        success: function (response) {
            lastID = parseInt(response); // Assuming response contains the new step ID
            addStep(ProductionFK);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching last ID:', error);
        }
    });

    function addStep(ProductionFK) {
        var tableBody = document.querySelector("#tblSteps2 tbody");
        var stepCells = tableBody.querySelectorAll("td");
        var stepCellsCount = stepCells.length;

        // Determine if adding to an existing row or creating a new row
        var newRow;
        if (stepCellsCount % 2 === 0) { // Every two clicks, start a new row
            newRow = document.createElement("tr");
            tableBody.appendChild(newRow);
        } else {
            // Get the last row in the table to append a new <td>
            newRow = tableBody.lastElementChild;
        }

        newRowIndex = stepCellsCount;
        var lastCell = stepCells[stepCells.length - 1];
        var lastStepInput = lastCell ? lastCell.querySelector(`input[name$="ProdStepsNum"]`) : null;
        var lastStepValue = lastStepInput ? parseInt(lastStepInput.value) : 0;
        currentStep1Value = lastStepValue + 1;


        // Create a new <td> for the current step
        var newCell = document.createElement("td");
        newCell.style.textAlign = "center";
        newCell.innerHTML = `
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProductionFK" value="${ProductionFK}" />
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdStepsNum" value="${currentStep1Value}" />
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdStepsID" value="${lastID}" />
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdSImage" />
        <div class="row">
            <div class="col-12 text-center">
                <div>${currentStep1Value}</div>
                <div>
                    <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
                </div>
                <div class="form-group mt-2">
                    <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage(this, 'PreviewPhoto1_${lastID}')">
                    <textarea class="form-control mt-2" id="stepsVM2List{newRowIndex}" name="stepsVM2List[${newRowIndex}].ProdText" placeholder="وصف الخطوة"></textarea>
                </div>
            </div>
        </div>
    `;

        // Append the new <td> to the current/new row
        newRow.appendChild(newCell);
        console.log("newCell:", newCell); // Debugging log   
        clickCount++;
    }
}

 
//صفحة الاضافة..
var currentStep1Value = 1;
var clickCount = 0;
var lastID = 0; // Initialize lastID globally

function AddnewRowstepsNew2(ProductionFK) {

    // Only retrieve lastID from server on the first click
    $.ajax({
        url: '/customer/Production/GetAddID',
        type: 'POST',
        data: {
            ProductionFK: ProductionFK,
            PropaVM: {} // Pass necessary data if needed
        },
        success: function (response) {
            lastID = parseInt(response);
            addStep(ProductionFK);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching last ID:', error);
        }
    });

    function addStep(ProductionFK) {
        var tableBody = document.querySelector("#tblSteps2 tbody");

        // Determine if adding to an existing row or creating a new row
        var newRow;
        if (clickCount % 2 === 0) { // Every two clicks, start a new row
            newRow = document.createElement("tr");
            tableBody.appendChild(newRow);
        } else {
            // Get the last row in the table to append a new <td>
            newRow = tableBody.lastElementChild;
        }

        // Create a new <td> for the current step
        var newCell = document.createElement("td");
        newCell.style.textAlign = "center";
        newCell.innerHTML = `
        <input type="hidden" name="stepsVM2List[${clickCount}].ProductionFK" value="${ProductionFK}" />
        <input type="hidden" name="stepsVM2List[${clickCount}].ProdStepsNum" value="${currentStep1Value}" />       
        <input type="hidden" name="stepsVM2List[${clickCount}].ProdStepsID" value="${lastID}" />
        <input type="hidden" name="stepsVM2List[${clickCount}].ProdSImage" />
        <div class="row">
            <div class="col-12 text-center">
                <div>${currentStep1Value}</div>
                <div>
                    <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
                </div>
                <div class="form-group mt-2">
                    <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage(this, 'PreviewPhoto1_${lastID}')">
                    <textarea class="form-control mt-2" id="stepsVM2List_${clickCount}" name="stepsVM2List[${clickCount}].ProdText" placeholder="وصف الخطوة"></textarea>
                </div>
            </div>
        </div>
    `;

        // Append the new <td> to the current/new row
        newRow.appendChild(newCell);

        // Increment values for next click
        currentStep1Value++;
        clickCount++;
        console.log("newCell:", newCell); // Debugging log   
    }
}

// صفحة الإضافة بعد الحفظ 
var clickCount = 0;
var lastID = 0; // Initialize lastID globally
function AddnewRowstepsUpdate222(ProductionFK) { //صفحة التعديل

        // Only retrieve lastID from server on the first click
        $.ajax({
            url: '/customer/Production/GetAddID',
            type: 'POST',
            data: {
                ProductionFK: ProductionFK,
                PropaVM: {} // Pass necessary data if needed
            },
            success: function (response) {
                lastID = parseInt(response);
                addStep2(ProductionFK);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching last ID:', error);
            }
        });
    function addStep2(ProductionFK) {
        var tableBody = document.querySelector("#tblSteps2 tbody");
        var stepCells = tableBody.querySelectorAll("td");
        var stepCellsCount = stepCells.length;

        // Determine if adding to an existing row or creating a new row
        var newRow;
        if (stepCellsCount % 2 === 0) { // Every two clicks, start a new row
            newRow = document.createElement("tr");
            tableBody.appendChild(newRow);
        } else {
            // Get the last row in the table to append a new <td>
            newRow = tableBody.lastElementChild;
        }

        newRowIndex = stepCellsCount;
        var lastCell = stepCells[stepCells.length - 1];

        var lastStepInput = lastCell ? lastCell.querySelector(`input[name$="ProdStepsNum"]`) : null;
        var lastStepValue = lastStepInput ? parseInt(lastStepInput.value) : 0;
        currentStep1Value = lastStepValue + 1;

        // Create a new <td> for the current step
        var newCell = document.createElement("td");
        newCell.style.textAlign = "center";
        newCell.innerHTML = `
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProductionFK" value="${ProductionFK}" />
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdStepsNum" value="${currentStep1Value}" />
         <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdStepsID" value="${lastID}" />
        <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdSImage" />
        <div class="row">
            <div class="col-12 text-center">
                <div>${currentStep1Value}</div>
                <div>
                    <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
                </div>
                <div class="form-group mt-2">
                    <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage(this, 'PreviewPhoto1_${lastID}')">
                    <textarea class="form-control mt-2" id="stepsVM2List{newRowIndex}" name="stepsVM2List[${newRowIndex}].ProdText" placeholder="وصف الخطوة"></textarea>
                </div>
            </div>
        </div>
    `;

        // Append the new <td> to the current/new row
        newRow.appendChild(newCell);
        console.log("newCell:", newCell); // Debugging log   
        clickCount++;

    }
}

function displaySelectedImage(input, imgId) {
    // Get the reference to the HTML img element based on the provided imgId
    var imgElement = document.getElementById(imgId);

    // Check if a file has been selected in the input element
    if (input.files && input.files[0]) {
        const file = input.files[0];
        const img = new Image();
        const reader = new FileReader();

        // Define an event handler for when the FileReader has finished reading the file
        reader.onload = function (e) {
            img.src = e.target.result; // Load the image for dimension checking
        };

        img.onload = function () {
            const width = img.width;
            const height = img.height;

            // Set max dimensions to 1000x1000 px
            const maxDimension = 1000;

            // Calculate approximate file size in MB
            const fileSizeInMB = (file.size / (1024 * 1024)).toFixed(2); // Convert bytes to MB

            // Check if the image dimensions are acceptable (≤ 1000x1000 px)
            if (width <= maxDimension && height <= maxDimension) {
                // If dimensions are acceptable, set the image preview
                imgElement.src = img.src;
            } else {
                // If dimensions are larger, display an error message
                showError(
                    `عذراً، حجم الصورة كبير جداً! 
                     أبعاد الصورة: ${width}x${height} بكسل.
                     حجم الصورة: ${fileSizeInMB} ميجابايت.
                     يرجى اختيار صورة بحجم أصغر أو أبعاد لا تتجاوز ${maxDimension}x${maxDimension} بكسل.`
                );
                input.value = ""; // Reset the file input if the image is too large
                imgElement.src = "/IMAGES/noImage.png"; // Reset the preview to default
            }
        };

        // Handle potential errors while reading the file
        reader.onerror = function () {
            showError("حدث خطأ أثناء تحميل الصورة. يرجى المحاولة مرة أخرى.");
        };

        // Read the selected file as a data URL (base64 encoded)
        reader.readAsDataURL(file);
    }
}

// Utility function to show error messages
function showError(message) {
    // Fallback to alert if no custom error element exists
    if (!document.getElementById("error-message")) {
        alert(message);
        return;
    }

    // Display error message in a predefined error div
    const errorDiv = document.getElementById("error-message");
    errorDiv.style.display = "block";
    errorDiv.innerText = message;

    // Optional: Hide the message after a few seconds
    setTimeout(() => {
        errorDiv.style.display = "none";
    }, 5000); // Hide after 5 seconds
}

//function toggleAddButtonVisibility(value) {

//    var addButton = document.getElementById("addStepButton");
//    var redMessage = document.querySelector(".red-message");
//    var saveButton = document.getElementById("saveChange");


//    if (value.trim() !== "") {
//        addButton.disabled = false; // Enable the button if text is entered
//        saveButton.disabled = false; // Enable the button if text is entered

//        redMessage.style.display = "none"; // Hide the red message
//    }

//    // Disable all delete buttons with the class 'delete-button'
//    var deleteButtons = document.querySelectorAll(".delete-button");
//    deleteButtons.forEach(function (button) {
//        button.disabled = true;
//    });

//}



//take two parameter ID1 = ID_التحضير , id=ID for the step . 
function Deletestep(id) { // after save in db . 
    Swal.fire({
        title: 'تأكيد !!',
        text: "تأكد أولا من حفظ أي الخطوات قمت بإضافتها قبل الحذف  ",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/customer/Production/Deletestep/' + id,

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

//==============================زر الحذف إضافة الخطوات ====================
function Deletestep2(id) { // after save in db . 
    Swal.fire({
        title: 'تأكيد !!',
        text: "تأكد أولا من حفظ أي الخطوات قمت بإضافتها قبل الحذف  ",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/customer/Production/Deletestep2/' + id,

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

//زر التعديل على الخطوات مع ID ليس عشوائي
//var clickCount = 0;
//var lastID = 0; // Initialize lastID globally
//function AddnewRowstepsUpdate2(ProductionFK) { //صفحة التعديل

//    if (clickCount === 0) {
    //    // Only retrieve lastID from server on the first click
    //    $.ajax({
    //        url: '/Production/GetLastId',
    //        type: 'GET',
    //        success: function (response) {
    //            lastID = parseInt(response) + 1;
    //            addStep(ProductionFK);
    //        },
    //        error: function (xhr, status, error) {
    //            console.error('Error fetching last ID:', error);
    //        }
    //    });
    //} else {
    //    // On subsequent clicks, increment lastID locally
    //    lastID++;
    //    addStep(ProductionFK);
    //}

    //function addStep(ProductionFK) {
    //    var tableBody = document.querySelector("#tblSteps2 tbody");
    //    var stepCells = tableBody.querySelectorAll("td");
    //    var stepCellsCount = stepCells.length;

    //    // Determine if adding to an existing row or creating a new row
    //    var newRow;
    //    if (stepCellsCount % 2 === 0) { // Every two clicks, start a new row
    //        newRow = document.createElement("tr");
    //        tableBody.appendChild(newRow);
    //    } else {
    //        // Get the last row in the table to append a new <td>
    //        newRow = tableBody.lastElementChild;
    //    }

    //    newRowIndex = stepCellsCount;
    //    var lastCell = stepCells[stepCells.length - 1];

    //    var lastStepInput = lastCell ? lastCell.querySelector(`input[name$="ProdStepsNum"]`) : null;
    //    var lastStepValue = lastStepInput ? parseInt(lastStepInput.value) : 0;
    //    currentStep1Value = lastStepValue + 1;


    //    // Create a new <td> for the current step
    //    var newCell = document.createElement("td");
    //    newCell.style.textAlign = "center";
    //    newCell.innerHTML = `
    //    <input type="hidden" name="stepsVM2List[${newRowIndex}].ProductionFK" value="${ProductionFK}" />
    //    <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdStepsNum" value="${currentStep1Value}" />
    //    <input type="hidden" name="stepsVM2List[${newRowIndex}].ProdSImage" />
    //    <div class="row">
    //        <div class="col-12 text-center">
    //            <div>${currentStep1Value}</div>
    //            <div>
    //                <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
    //            </div>
    //            <div class="form-group mt-2">
    //                <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage(this, 'PreviewPhoto1_${lastID}')">
    //                <textarea class="form-control mt-2" id="stepsVM2List{newRowIndex}" name="stepsVM2List[${newRowIndex}].ProdText" placeholder="وصف الخطوة"></textarea>
    //            </div>
    //        </div>
    //    </div>
    //`;

    //    // Append the new <td> to the current/new row
//        newRow.appendChild(newCell);
//        console.log("newCell:", newCell); // Debugging log   
//        clickCount++;

//    }
//}
//=========================================================================================================================================


//زر الحذف في صفحة التعديل .
//function DeleteRow11(button) {
//    var tableBody = document.querySelector("#tblSteps2 tbody");
//    var rows = tableBody.children;
//    var rowIndex = button.getAttribute("data-row-index");

//    // Check if the button click corresponds to the last row
//    if (rowIndex == rows.length - 1) {
//        Swal.fire({
//            title: 'هل أنت متأكد؟',
//            icon: 'warning',
//            showCancelButton: true,
//            cancelButtonText: 'الغاء',
//            confirmButtonColor: '#d33',
//            cancelButtonColor: '#3085d6',
//            confirmButtonText: 'نعم!'
//        }).then((result) => {
//            if (result.isConfirmed) {
//                // Remove the last row from the table
//                tableBody.removeChild(rows[rowIndex]);

//                // Display a success message
//                Swal.fire({
//                    icon: 'success',
//                    title: 'تم الحذف بنجاح',

//                });
//            }
//        });
//    } else {
//        // Display a message that you cannot delete rows until they are saved in the database
//        Swal.fire({
//            icon: 'error',
//            title: 'لا يمكنك حذف هذا الصف',
//            text: 'يجب حفظ التغييرات أولا والعودة الى صفحة التعديل لحذف الصف.',
//        });
//    }
//}

//زر الحذف في صفحة الاضافة
//function DeleteRow3(button) {
//    var tableBody = document.querySelector("#tblSteps2 tbody");
//    var rows = tableBody.children;
//    var rowIndex = button.getAttribute("data-row-index");

//    // Check if the button click corresponds to the last row
//    if (rowIndex == rows.length - 1) {
//        Swal.fire({
//            title: 'هل أنت متأكد؟',
//            icon: 'warning',
//            showCancelButton: true,
//            cancelButtonText: 'الغاء',
//            confirmButtonColor: '#d33',
//            cancelButtonColor: '#3085d6',
//            confirmButtonText: 'نعم!'
//        }).then((result) => {
//            if (result.isConfirmed) {
//                // Remove the last row from the table
//                tableBody.removeChild(rows[rowIndex]);

//                // Display a success message
//                Swal.fire({
//                    icon: 'success',
//                    title: 'تم الحذف بنجاح',

//                });
//            }
//        });
//    } else {
//        // Display a message that you cannot delete rows until they are saved in the database
//        Swal.fire({
//            icon: 'error',
//            title: 'أنت قادر على حذف الصف الأخير فقط ',
//            text: 'يجب حفظ التغييرات أولا والعودة الى صفحة التعديل لحذف الصف.',
//        });
//    }
//}


//function DeleteRow(button) {
//    var rowIndex = button.getAttribute("data-row-index");
//    var tableBody = document.querySelector("#tblSteps tbody");
//    var rows = tableBody.children;

//    // Find the row to delete
//    var rowToDelete = rows[rowIndex];

//    // Determine the رقم_الخطوة values for الخطوة1 and الخطوة2 in the row to delete
//    var رقم_الخطوة1ToDelete = parseInt(rowToDelete.querySelector(`input[name^="stepsVM[${rowIndex}].رقم_الخطوة1"]`).value);
//    var رقم_الخطوة2ToDelete = parseInt(rowToDelete.querySelector(`input[name^="stepsVM[${rowIndex}].رقم_الخطوة2"]`).value);

//    // Delete the row
//    rowToDelete.remove();

//    // Update the رقم_الخطوة values for الخطوة1 and الخطوة2 in the following rows
//    for (var i = rowIndex++ ; i < rows.length; i++) {
//        var رقم_الخطوة1Input = rows[i].querySelector(`input[name^="stepsVM[${i}].رقم_الخطوة1"]`);
//        var رقم_الخطوة2Input = rows[i].querySelector(`input[name^="stepsVM[${i}].رقم_الخطوة2"]`);

//        if (رقم_الخطوة1Input && رقم_الخطوة2Input) {
//            رقم_الخطوة1Input.value = رقم_الخطوة1ToDelete;
//            رقم_الخطوة2Input.value = رقم_الخطوة2ToDelete;

//            // Update any displayed رقم_الخطوة values in the row
//            rows[i].querySelector(`div[data-رقم_الخطوة1="${i}"]`).textContent = رقم_الخطوة1ToDelete;
//            rows[i].querySelector(`div[data-رقم_الخطوة2="${i}"]`).textContent = رقم_الخطوة2ToDelete;
//        }
//    }
//}
 