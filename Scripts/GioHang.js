$(document).ready(function () {
    DatHang = function (BookId) {
        var SL = $('#txtSL').val();
        if (isNaN(SL) == true) {
            SL = 1;
        }
        $.ajax({
            url: "/GioHang/ThemGioHang",
            data: { iMaSP: BookId, SL: SL },
            type: "POST",
            success: function (result) {
                var TongSL = 0;
                var TongTien = 0;


                $.each(result, function (i, item) {
                    TongSL += item.SoLuongMua;
                    TongTien += item.TongTien
                });
                $('#cart > a  > span').html(TongSL + " Sản Phẩm - " + TongTien + "VND")
                alert("Đặt Hàng Thành Công!!")
            }
        });
        return false;
    };
    a = function() {
        alert("ok");
    }
    CapNhat = (function (BookId) {
        SL = $("#" + BookId).val();
        if (isNaN(SL) == true) {
            SL = 1;
        }
        var TongTien1 = 0;
        var TongSL1 = 0
        $.ajax({
            url: "/GioHang/ThemGiohang",
            data: { iMaSP: BookId, SL: SL },
            type: "POST",
            success: function (result) {
                var item = "";
                $('.Cart-info tbody').empty();
                $.each(result, function (i, item) {
                    var R = "<tr>"
                        + "<td><input disabled type='text' value=" + item.BookId + " id='MaSach' name='MaSach'></td>"
                        + "<td><a href='~/ViewSP/ChiTietSanPham?MaSp='" + item.BookId + "><img src='/Images/" + item.ImgUrl + " '/></a></td>"
                        + " <td>" + item.Title + "</td>"
                        + "<td><input type='text' id=" + item.BookId + " class='txtSLGH' value=" + item.SoLuongMua + " /><a class='fa fa-upload btn-Update' onclick='CapNhat(" + item.BookId + ")' aria-hidden='true'></a><a class='fa fa-trash-o btn-Delete' aria-hidden='true' onclick='XoaSP(@item.BookId)'></a></td>"
                        + "<td><span id='DonGia'> " + item.Price + "</span></td>"
                        + "<td>" + item.TongTien + "</td>"
                    " </tr>"
                    TongTien1 += item.TongTien;
                    TongSL1 += item.Quantity;
                    $('.Cart-info tbody').append(R);
                });
                $('.TongTien > h2 > span').html(TongTien1)
                $('#cart > a  > span').html(TongSL1 + " Sản Phẩm - " + TongTien1 + "VND")
                $('#tongtien').html(TongTien1)
            },
        });
        return false;
    });
    XoaSP = (function (BookId) {
        $.ajax({
            url: "/GioHang/XoaSP",
            data: { iMaSP: BookId },
            type: "POST",
            success: function (result) {
                var item = "";
                var TongTien = 0;
                var TongSL2 = 0;
                $('.Cart-info tbody').empty();
                $.each(result, function (i, item) {
                    var R = "<tr>"
                        + "<td><input disabled type='text' value=" + item.BookId + " id='MaSach' name='MaSach'></td>"
                        + "<td><a href='~/ViewSP/ChiTietSanPham?MaSP='" + item.BookId + "><img src='/Images/" + item.ImgUrl + " '/></a></td>"
                        + " <td>" + item.Title + "</td>"
                        + "<td><input type='text' id=" + item.BookId + " class='txtSLGH' value=" + item.SoLuongMua + " /><a class='fa fa-upload btn-Update' onclick='CapNhat(" + item.BookId + ")' aria-hidden='true'></a><a class='fa fa-trash-o btn-Delete' aria-hidden='true' onclick='XoaSP("+ item.BookId +")'></a></td>"
                        + "<td><span id='DonGia'> " + item.Price + "</span></td>"
                        + "<td>" + item.TongTien + "</td>"
                   " </tr>"
                    TongTien += item.TongTien
                    TongSL2 += item.Quantity
                    $('.Cart-info tbody').append(R);

                });
                $('.TongTien > h2 > span').html(TongTien)
                $('#cart > a  > span').html(TongSL2 + " Sản Phẩm - " + TongTien + "VND")
                $('#tongtien').html(TongTien)

            },
        });
        return false;
    });
    tai = function () {
        alert("Load lại")
    };
});