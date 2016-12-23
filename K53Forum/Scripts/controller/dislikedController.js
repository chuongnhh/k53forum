$(function () {
    $('.disliked').off('click').on('click', function () {
        var id = $(this).data('key');
        $.ajax({
            url: '/post/disliked/' + id,
            type: 'post',
            data: {},
            success: function (data) {
                if (data.status == true) {
                    document.getElementById('disliked-'+id).innerHTML = ' Đã không thích';
                    document.getElementById('disliked-' + id).style.color = "#32ac99";
                }
                else {
                    document.getElementById('disliked-' + id).innerHTML = ' Không thích';
                    document.getElementById('disliked-' + id).style.color = "#158cba";
                }
                document.getElementById('dislikedCount-' + id).innerHTML = ' ' + data.count;
            }
        })
    })
})