$(function () {
    $("#form-register").submit(function (e) {
        e.preventDefault(); // Prevent Default Submission

        var data = $(this).serialize(),
            action = $(this).attr("action"),
            method = $(this).attr("method");

        $(".loading").show(); // show loading div
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
                    //console.log('xxxxxxxxxxxx');
                }
                // all went well...
                //document.location = "/home/index";
            },
            error: function (err) {
                document.getElementById('register-fail').innerHTML = "Tên tài khoản đã tồn tại hoặc mật khẩu không đúng";
                // there was something not right...
                //console.log("lỗi rồi bạn");
            },
            complete: function () {
                $(".loading").hide(); // hide the loading
            }
        });

        return false; // don't let the form be submitted
    });

});