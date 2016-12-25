$(function () {
    $("#form-register").submit(function (e) {
        e.preventDefault(); // Prevent Default Submission

        var data = $(this).serialize(),
            action = $(this).attr("action"),
            method = $(this).attr("method");

        $.ajax({
            url: action,
            type: method,
            data: data,
            success: function (data) {
                if (data.status == true) {
                    document.location = "/home/index";
                }
                else {
                    document.getElementById('register-fail').innerHTML = "Tên tài khoản đã tồn tại hoặc mật khẩu không đúng";
                }
            },
            error: function (err) {
                document.getElementById('register-fail').innerHTML = "Tên tài khoản đã tồn tại hoặc mật khẩu không đúng";
            },
            complete: function () {
            }
        });

        return false; // don't let the form be submitted
    });

});