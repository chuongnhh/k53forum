$(function () {
    $('.delete-member').off('click').on('click', function (e) {
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
                                $('#member-' + data.id).remove();
                            }
                            else {
                                //console.log(data);
                            }
                        }
                    })
                }
            }
        });
    })
})