// focus reply
$(function () {
    $('#reply').off('click').on('click', function () {
        $('#Reply').summernote({
            focus: true,                // set focus to editable area after initializing summernote
        });
    })
})

$(function () {
    $('#form-reply').submit(function (e) {
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
                    var str = '<div class="media reply-'+data.id+'">'
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
                        + '<a href="/discussion/deletereply/' + data.id + '" class="delete-reply"> xóa</a>'
                        + '</small>'
                        + '</h5>'
                        + '' + data.content + ''
                        + '</div>'
                        + '</div>'
                    $(str).insertAfter('#insert-reply');

                    $('#Reply').summernote({
                        focus: true               // set focus to editable area after initializing summernote
                    });
                    $('#Reply').code('');
                }
            }
        })
    })
})

// delete
$(function () {
    $('.delete-reply').off('click').on('click', function (e) {
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
                                $('.reply-' + data.id).remove();
                            }
                        }
                    })
                }
            }
        });
    })
})