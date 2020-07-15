

   Cmt= function() {
        var MS = $('#MaSach').val();
    var Ngay = $('#Ngay').val();
    var name = $('#txt-frmRating').val();
       var BL = $('#erea-frmRating').val();
       alert("Cmt thanh cong")
    $.ajax(
            {
        url: "/Chitiet/ChitietBook",
                data: {iMaSach: MS, iName: name, iComment: BL, Ngay: Ngay},
    type: "POST",
                success: function (data) {
     
                    $('.new-Cmt .author > b').append(name);
    $('.new-Cmt .author > span').append(Ngay);

    $(".new-Cmt .text-review").append(BL)
    $(".new-Cmt").css("display", "block");
}
});
return false;

};
