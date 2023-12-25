var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Contact/GetAllContacts' },

        "columns": [
            { data: 'firstName', "width": "28%" },
            { data: 'lastName', "width": "28%" },
            { data: 'phoneNumber', "width": "12%" },

            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/Contact/UpdateContactView/${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                     <a onClick="Delete(${data})" class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`;
                },
                "width": "25%"
            }
        ],
        "success": function (data) {
            console.log("Data from API:", data);
        }
    });
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Contact/RemoveContacts`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ contactIds: [id] }),
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function (error) {
                    console.log('Error:', error);
                }
            });
        }
    });
}

// Function to show the loading indicator
function showLoadingIndicator() {
    document.getElementById('loadingIndicator').style.display = 'block';
}

// Function to hide the loading indicator
function hideLoadingIndicator() {
    document.getElementById('loadingIndicator').style.display = 'none';
}

// Call the showLoadingIndicator function when the page is loading
showLoadingIndicator();

// Simulate an asynchronous action (e.g., loading data)
setTimeout(function () {
    // Call the hideLoadingIndicator function when the content is loaded
    hideLoadingIndicator();
}, 2000); // Adjust the timeout as needed
