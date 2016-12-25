$(function () {
    $('#form-commented').submit(function (e) {
        e.preventDefault();
        var action = $(this).attr('action');
        var method = $(this).attr('method');
        var data = $(this).serialize();
        $.ajax({
            url: action,
            method: method,
            data: data,

            success: function (data) {
                if (data.status == true) {
                    var str = '<div class="media comment-' + data.id + '">'
                        + '<div class="media-left">'
                        + '<a href="/member/details/' + data.memberId + '">'
                        + '<img class="media-object" src="' + data.avatar + '" alt="' + data.avatar + '" style="max-width:50px;">'
                        + '</a>'
                        + '</div>'
                        + '<div class="media-body">'
                        + '<h5 class="media-heading">'
                        + '<a href= "/member/details/' + data.memberId + '">' + data.fullname + '</a>'
                        + '<small>'
                        + '<span> <i class="fa fa-calendar-o"> ' + data.dateCreated + '</i></span>'
                        + '<a href="/article/deletecomment/' + data.id + '" class="delete-comment"> xóa</a>'
                        + '</small>'
                        + '</h5>'
                        + '' + data.content + ''
                        + '</div>'
                        + '</div>'

                    $(str).insertAfter('#insert-commented');

                    $('#Commented').summernote({
                        focus: true               // set focus to editable area after initializing summernote
                    });
                    $('#Commented').code('');
                }
            }
        })
    })
})

// delete
$(function () {
    $('.delete-comment').off('click').on('click', function (e) {
        e.preventDefault();
        var action = $(this).attr('href');

        bootbox.confirm({
            message: "Bạn muốn xóa bình luận này?",
            buttons: {
                cancel: {
                    label: 'Hủy',
                    className: 'btn-default'
                },
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-success'
                }
            }, callback: function (result) {
                if (result == true) {
                    $.ajax({
                        url: action,
                        method: 'POST',
                        data: {},

                        success: function (data) {
                            if (data.status == true) {
                                $('.comment-' + data.id).remove();
                            }
                        }
                    })
                }
            }
        });
    })
})

$(function () {
    $('#form-edit-article').submit(function (e) {
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
                        url: '/article/details/' + data.id,
                        target: '_self'
                    }, {
                            type: 'success'
                        });
                }
                else {
                    $.notify({
                        title: '<strong>Thông báo</strong>',
                        message: data.name + ' đã được cập nhật thành công. Nhấp vào thông báo này để <strong>xem lại thông tin.</strong>',
                        url: '/article/details/' + data.id,
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