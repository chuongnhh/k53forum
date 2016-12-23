
$(function () {
    $('#form-edit-member').submit(function (e) {
        e.preventDefault();
        var method = $(this).attr('method');
        var action = $(this).attr('action');
        var data = $(this).serialize();

        $.ajax({
            url: action,
            type: method,
            data: data,
            success: function (data) {
                if (data.status = 'success') {
                    $.notify({
                        title: '<strong>Thông báo</strong>',
                        message: data.fullname + ' đã được cập nhật thành công. Nhấp vào thông báo này để <strong>đến trang chi cá nhân.</strong>',
                        url: '/member/details/' + data.id,
                        target:'_self'
                    }, {
                            type: 'success'
                        });
                }
                else {
                    $.notify({
                        title: '<strong>Thông báo</strong>',
                        message: 'Có lỗi xảy ra trong khi cập nhật ' + data.member + '. Vui lòng kiểm tra lại thông tin. Nhấp vào thông báo này để <strong>đến trang chi cá nhân.</strong>',
                        url: '/member/details/' + data.memberId,
                        target: '_self'
                    }, {
                            type: 'danger'
                        });
                }
            },
            error: function (err) {
                $.notify({
                    title: '<strong>Thông báo</strong>',
                    message: 'Có lỗi xảy ra trong khi cập nhật ' + data.member + '. Vui lòng kiểm tra lại thông tin. Nhấp vào thông báo này để <strong>đến trang chi cá nhân.</strong>',
                    url: '/member/details/' + data.memberId,
                    target: '_self'
                }, {
                        type: 'danger'
                    });
            },
            complete: function () {

            }
        })
    })
})