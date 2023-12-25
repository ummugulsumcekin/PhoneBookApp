var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    if ($.fn.DataTable.isDataTable('#tblData')) {
        $('#tblData').DataTable().destroy();
    }
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Contact/GetAllContacts' },
        "columns": [
            { data: 'firstName', "width": "25%" },
            { data: 'lastName', "width": "25%" },
            { data: 'phoneNumber', "width": "25%" },
            
            
        ],
        "initComplete": function (settings, json) {
            hideLoadingIndicator();  // DataTable yüklendiğinde loading indicator'ı gizle
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

// AddContactForm submit event handler
$('#addContactForm').submit(function (e) {
    e.preventDefault();

    // Form verilerini al
    var formData = {
        firstName: $('#firstName').val(),
        lastName: $('#lastName').val(),
        phoneNumber: $('#phoneNumber').val()
    };

    // AJAX ile yeni kayıt ekleme işlemini gerçekleştir
    $.ajax({
        type: 'POST',
        url: '/Contact/AddContactForm',
        data: JSON.stringify(formData),
        contentType: 'application/json',
        success: function (data) {
            // Başarılı olduğunda modalı kapat ve tabloyu güncelle
            $('#addContactModal').modal('hide');
            dataTable.ajax.reload();  // DataTable'ı yeniden yükle
            toastr.success('Yeni kayıt başarıyla eklendi.');
        },
        error: function (error) {
            console.log('Error:', error);
        }
    });
});

// Function to show the loading indicator
function showLoadingIndicator() {
    document.getElementById('loadingIndicator').style.display = 'block';
}

// Function to hide the loading indicator
function hideLoadingIndicator() {
    document.getElementById('loadingIndicator').style.display = 'none';
}