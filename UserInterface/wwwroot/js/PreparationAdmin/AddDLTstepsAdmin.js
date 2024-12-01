
var clickCount = 0;
var lastID = 0; // Initialize lastID globally
function AddnewRowstepsUpdate101(preparationFk) {

            // Only retrieve lastID from server on the first click
            $.ajax({
                url: '/admin/SectionDetailsPreparation/GetAddID',
                type: 'POST',
                data: {
                    preparationFk: preparationFk,
                    PrepaVM: {} // Pass necessary data if needed
                },
                success: function (response) {
                    lastID = parseInt(response);
                    addStep101(preparationFk);
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching last ID:', error);
                }
            });

        function addStep101(preparationFk) {
            var tableBody = document.querySelector("#tblSteps tbody");
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
            var lastStepInput = lastCell ? lastCell.querySelector(`input[name$="PrepStepsNum"]`) : null;
            var lastStepValue = lastStepInput ? parseInt(lastStepInput.value) : 0;
            currentStep1Value = lastStepValue + 1;

            // Create the new cell
            newCell = document.createElement("td");
            newCell.style.textAlign = "center";
            newCell.innerHTML = `
            <input type="hidden" name="stepsVM[${newRowIndex}].PreparationsFK" value="${preparationFk}" />
            <input type="hidden" name="stepsVM[${newRowIndex}].PrepStepsNum" value="${currentStep1Value}" />
            <input type="hidden" name="stepsVM[${newRowIndex}].PrepStepsID" value="${lastID}" />
            <input type="hidden" name="stepsVM[${newRowIndex}].PrepImage"  />
            <div class="row">
                <div class="col-12 text-center">
                    <div>${currentStep1Value}</div>
                    <div>
                        <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
                    </div>
                    <div class="form-group mt-2">
                        <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage1(this, 'PreviewPhoto1_${lastID}')">
                        <textarea class="form-control mt-2" id="stepsVM_${newRowIndex}" name="stepsVM[${newRowIndex}].PrepText" placeholder="وصف الخطوة" ></textarea>
                    </div>
                    <div class="py-5"></div>
                </div>
            </div>
        `;

            // Append the new cell to the current or new row
            newRow.appendChild(newCell);
            console.log("newCell:", newCell); // Debugging log
            clickCount++;

        }
    }

    //صفحة الاضافة..
    var currentStep1Value = 1;
    var clickCount = 0;
    var lastID = 0; // Initialize lastID globally

function AddnewRowstepsNew02(preparationFk) {

    // Only retrieve lastID from server on the first click
    $.ajax({
        url: '/admin/SectionDetailsPreparation/GetAddID',
        type: 'POST',
        data: {
            preparationFk: preparationFk,
            PrepaVM: {} // Pass necessary data if needed
        },
        success: function (response) {
            lastID = parseInt(response);
            addStep(preparationFk);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching last ID:', error);
        }
    });
    function addStep(preparationFk) {
        var tableBody = document.querySelector("#tblSteps tbody");

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
        <input type="hidden" name="stepsVM[${clickCount}].PreparationsFK" value="${preparationFk}" />
        <input type="hidden" name="stepsVM[${clickCount}].PrepStepsNum" value="${currentStep1Value}" />
         <input type="hidden" name="stepsVM[${clickCount}].PrepStepsID" value="${lastID}" />
        <input type="hidden" name="stepsVM[${clickCount}].PrepImage" />
        <div class="row">
            <div class="col-12 text-center">
                <div>${currentStep1Value}</div>
                <div>
                    <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
                </div>
                <div class="form-group mt-2">
                    <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage1(this, 'PreviewPhoto1_${lastID}')">
                    <textarea class="form-control mt-2" id="stepsVM_${clickCount}" name="stepsVM[${clickCount}].PrepText" placeholder="وصف الخطوة"></textarea>
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

//صفحة الإضافة بعد الحفظ 
var clickCount = 0;
var lastID = 0; // Initialize lastID globally
function AddnewRowstepsNew2202(preparationFk) {

   
        // Only retrieve lastID from server on the first click
        $.ajax({
            url: '/admin/SectionDetailsPreparation/GetAddID',
            type: 'POST',
            data: {
                preparationFk: preparationFk,
                PrepaVM: {} // Pass necessary data if needed
            },
            success: function (response) {
                lastID = parseInt(response);
                addStep1102(preparationFk);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching last ID:', error);
            }
        });

    function addStep1102(preparationFk) {
        var tableBody = document.querySelector("#tblSteps tbody");
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
        var lastStepInput = lastCell ? lastCell.querySelector(`input[name$="PrepStepsNum"]`) : null;
        var lastStepValue = lastStepInput ? parseInt(lastStepInput.value) : 0;

        currentStep1Value = lastStepValue + 1;


        // Create the new cell
        newCell = document.createElement("td");
        newCell.style.textAlign = "center";
        newCell.innerHTML = `
        <input type="hidden" name="stepsVM[${newRowIndex}].PreparationsFK" value="${preparationFk}" />
        <input type="hidden" name="stepsVM[${newRowIndex}].PrepStepsNum" value="${currentStep1Value}" />
        <input type="hidden" name="stepsVM[${newRowIndex}].PrepStepsID" value="${lastID}" />
        <input type="hidden" name="stepsVM[${newRowIndex}].PrepImage"  />
        <div class="row">
            <div class="col-12 text-center">
                <div>${currentStep1Value}</div>
                <div>
                    <img id="PreviewPhoto1_${lastID}" src="/IMAGES/noImage.png" alt="Logo" width="125" height="125" style="border: 1px; margin-top: 20px;">
                </div>
                <div class="form-group mt-2">
                    <input type="file" name="file1_${lastID}" class="border-0 shadow mt-5" id="customFile1_${lastID}" data-preview-id="PreviewPhoto1_${lastID}" onchange="displaySelectedImage1(this, 'PreviewPhoto1_${lastID}')">
                    <textarea class="form-control mt-2" id="stepsVM_${newRowIndex}" name="stepsVM[${newRowIndex}].PrepText" placeholder="وصف الخطوة" ></textarea>
                </div>
            </div>
        </div>
    `;

        // Append the new cell to the current or new row
        newRow.appendChild(newCell);
        console.log("newCell:", newCell); // Debugging log   
        clickCount++;

    }
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

function displaySelectedImage1(input, imgId) {
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
                showError1(
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
            showError1("حدث خطأ أثناء تحميل الصورة. يرجى المحاولة مرة أخرى.");
        };

        // Read the selected file as a data URL (base64 encoded)
        reader.readAsDataURL(file);
    }
}

// Utility function to show error messages
function showError1(message) {
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








    //take two parameter ID1 = ID_التحضير , id=ID for the step . 
    function Deletestep101(id) { // after save in db . 
        Swal.fire({
            title: 'تأكيد !!!!!',
            text: " تأكد أولا من حفظ أي خطوة قمت بإضافتها قبل الحذف  ",
            icon: 'warning',
            showCancelButton: true,
            cancelButtonText: 'الغاء',
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'حذف '
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/admin/SectionDetailsPreparation/Deletestep101/'+ id,
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

//====================زر الحذف لصفحة الإضافة ====================================
function Deletestep202(id) { // after save in db . 
    Swal.fire({
        title: 'تأكيد !!!!!',
        text: " تأكد أولا من حفظ أي خطوة قمت بإضافتها قبل الحذف  ",
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'الغاء',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'حذف '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/admin/SectionDetailsPreparation/Deletesteps202/' + id,
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
