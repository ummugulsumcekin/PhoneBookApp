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



function deleteSelectedContacts(contactIds) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: 'Seçili kişileri silmek istediğinizden emin misiniz?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Contact/RemoveContacts`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ contactIds: contactIds }),
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