
$(function () {
    $('.liked').off('click').on('click', function () {
        var id = $(this).data('key');
        console.log(id);
        $.ajax({
            url: '/post/liked/' + id,
            type: 'post',
            data: {},
            success: function (data) {
                if (data.status == true) {
                    document.getElementById('liked-' + id).innerHTML = ' Đã thích';
                    document.getElementById('liked-' + id).style.color = "#32ac99";
                }
                else {
                    document.getElementById('liked-' + id).innerHTML = ' Thích';
                    document.getElementById('liked-' + id).style.color = "#158cba";
                }
                document.getElementById('likedCount-' + id).innerHTML = ' ' + data.count;
            }
        })
    })
})