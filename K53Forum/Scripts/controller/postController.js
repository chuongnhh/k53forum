$(function () {
    $('#form-edit-post').submit(function (e) {
        e.preventDefault();
        var data = $(this).serialize();
        var action = $(this).attr('action');
        var method = $(this).attr('method');

        $.ajax({
            url: action,
            method: method,
            data: data,
            
            success: function (data) {
                if (data.status == true) {
                    $.notify({
                        title: '<strong>Thông báo</strong>',
                        message: data.name + ' đã được cập nhật thành công. Nhấp vào thông báo này để <strong>xem lại thông tin.</strong>',
                        url: '/post/details/' + data.id,
                        target: '_self'
                    }, {
                            type: 'success'
                        });
                }
                else {
                    $.notify({
                        title: '<strong>Thông báo</strong>',
                        message: data.name + ' đã được cập nhật thành công. Nhấp vào thông báo này để <strong>xem lại thông tin.</strong>',
                        url: '/post/details/' + data.id,
                        target: '_self'
                    }, {
                            type: 'danger'
                        });
                }
            },
            error: function (err) {

            },
            complete: function () {

            }
        })
    })
})

// binh luan
$(function () {
    $('#commented').off('click').on('click', function () {
        $('#Commented').summernote({
            focus: true,                // set focus to editable area after initializing summernote
        });
    })
})