$(function () {

    var toplamArticle = $("div#listeleme article").length;
    var veriSayisi = 5;
    $("div#listeleme article:gt(" + (veriSayisi - 1) + ")").hide();
    var sayfaSayisi = Math.ceil(toplamArticle / veriSayisi);
    for (var i = 1; i <= sayfaSayisi; i++) {
        $("#sayfalama").append('<a href="javascript:void(0)">' + i + '</a>');
    }

    $("#sayfalama a:first").addClass("active");
    $("#sayfalama a").click(function () {
        var indis = $(this).index() + 1;
        var gt = veriSayisi * indis;
        $("#sayfalama a").removeClass("active");
        $(this).addClass("active");
        $("div#listeleme article").hide();
        for (var i = gt - veriSayisi; i < gt; i++) {
            $("div#listeleme article:eq(" + i + ")").show();
        }

    });
});