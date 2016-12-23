$(function () {
    $("#form-login").submit(function (e) {
        e.preventDefault(); // Prevent Default Submission

        var data = $(this).serialize(),
            action = $(this).attr("action"),
            method = $(this).attr("method");
            var url =window.location;
        $.ajax({
            url: action,
            type: method,
            data: data,
            success: function (data) {
                if (data.status == true) {
                    document.location = url;
                    $.notify({
                        title: 'Thông báo!',
                        message: 'xin chào, ' + data.name + ' đã đăng nhập.'
                    }, {
                            type: 'success'
                        })
                }
                else {
                    document.getElementById('login-fail').innerHTML = "Sai tài khoản hoặc mật khẩu";
                }
            },
            error: function (err) {
            },
            complete: function () {
               
            }
        });
    });

});