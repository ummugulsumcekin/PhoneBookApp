
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

// Checkbox'ları seçilen kişileri silmek veya düzenlemek için
$('#tblData').on('change', '.contact-checkbox', function () {
    // Seçili checkbox'ların değerlerini al
    var selectedIds = [];
    $('.contact-checkbox:checked').each(function () {
        selectedIds.push($(this).data('id'));
    });

    // Seçili checkbox varsa düzenleme veya silme işlemlerini gerçekleştir
    if (selectedIds.length > 0) {
        // Birden fazla seçili kişi varsa düzenleme işlemi yapılmaz
        if (selectedIds.length === 1) {
            // Düzenleme işlemi
            $('#editSelected').prop('disabled', false);
        } else {
            // Birden fazla seçili kişi varsa düzenleme işlemi devre dışı bırakılır
            $('#editSelected').prop('disabled', true);
        }

        // Silme işlemi
        $('#deleteSelected').prop('disabled', false);
    } else {
        // Seçili checkbox yoksa düzenleme ve silme butonlarını devre dışı bırak
        $('#editSelected').prop('disabled', true);
        $('#deleteSelected').prop('disabled', true);
    }
});

// Silme butonları için click olayları
$('#tblData').on('click', '.btn-delete', function () {
    var contactId = $(this).data('id');
    deleteSelectedContacts([contactId]);
});

// Düzenleme butonları için click olayları
$('#tblData').on('click', '.btn-edit', function () {
    var contactId = $(this).data('id');
    window.location.href = '/Contact/UpdateContactView/' + contactId;
});

// Seçilen kişileri silme butonu için click olayı
$('#deleteSelected').on('click', function () {
    var selectedIds = [];
    $('.contact-checkbox:checked').each(function () {
        selectedIds.push($(this).data('id'));
    });

    if (selectedIds.length > 0) {
        deleteSelectedContacts(selectedIds);
    }
});

// Seçilen kişiyi düzenleme butonu için click olayı
$('#editSelected').on('click', function () {
    var selectedIds = $('.contact-checkbox:checked').map(function () {
        return $(this).data('id');
    }).get();

    if (selectedIds.length === 1) {
        window.location.href = '/Contact/UpdateContactView/' + selectedIds[0];
    }
});
