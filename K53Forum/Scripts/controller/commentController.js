//$(function () {
//    $("#form-commented").submit(function (e) {
//        //e.preventDefault(); // Prevent Default Submission

//        var data = $(this).serialize(),
//            action = $(this).attr("action"),
//            method = $(this).attr("method");
//        //var postId = $(this).data('PostId');
//        $(".loading").show(); // show loading div
//        $.ajax({
//            url: action,
//            type: method,
//            data: data,
//            success: function (data) {
//                if (data.status == true) {
//                    //document.location = '/post/details/' + postId;
//                }
//                else {
//                    //document.getElementById('login-fail').innerHTML = "Sai tài khoản hoặc mật khẩu";
//                }
//            },
//        });
//    });

//});