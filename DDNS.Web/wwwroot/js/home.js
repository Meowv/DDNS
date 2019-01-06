$(function () {
    var culture = getCookie(".AspNetCore.Culture") || "c=zh-CN|uic=zh-CN";
    if (culture.indexOf("zh-CN") >= 0) {
        $(".language span:eq(0)").addClass("active");
    }
    if (culture.indexOf("zh-TW") >= 0) {
        $(".language span:eq(1)").addClass("active");
    }
    if (culture.indexOf("en-US") >= 0) {
        $(".language span:eq(2)").addClass("active");
    }
    $(".language span").click(function () {
        var _this = $(this);
        if (!_this.hasClass("active")) {
            var culture = _this.data("culture");
            $.getJSON("/api/culture?culture=" + culture, function (result) {
                if (result === 1) {
                    window.location.reload();
                }
            });
        }
    });
    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else return null;
    }
    var numpic = $('#slides li').size() - 1;
    var nownow = 0;
    var inout = 0;
    var SPEED = 5000;
    $('#slides li').eq(0).siblings('li').css({ 'display': 'none' });
    var ulstart = '<ul id="pagination">',
        ulcontent = '',
        ulend = '</ul>';
    addli();
    var pagination = $('#pagination li');
    var paginationwidth = $('#pagination').width();
    $('#pagination').css('margin-left', (-paginationwidth / 2))
    pagination.eq(0).addClass('current')
    function addli() {
        for (var i = 0; i <= numpic; i++) {
            ulcontent += '<li>' + '<a>' + (i + 1) + '</a>' + '</li>';
        }
        $('#slides').after(ulstart + ulcontent + ulend);
    }
    pagination.on('click', DOTCHANGE)
    function DOTCHANGE() {
        var changenow = $(this).index();
        if (changenow == nownow) return;
        $('#slides li').eq(nownow).css('z-index', '900');
        $('#slides li').eq(changenow).css({ 'z-index': '800' }).show();
        pagination.eq(changenow).addClass('current').siblings('li').removeClass('current');
        $('#slides li').eq(nownow).fadeOut(400, function () { $('#slides li').eq(changenow).fadeIn(500); });
        nownow = changenow;
    }
    pagination.mouseenter(function () {
        inout = 1;
    })
    pagination.mouseleave(function () {
        inout = 0;
    })
    function gogo() {
        var NN = nownow + 1;
        if (inout == 1) {
        } else {
            if (nownow < numpic) {
                $('#slides li').eq(nownow).css('z-index', '900');
                $('#slides li').eq(NN).css({ 'z-index': '800' }).show();
                pagination.eq(NN).addClass('current').siblings('li').removeClass('current');
                $('#slides li').eq(nownow).fadeOut(400, function () { $('#slides li').eq(NN).fadeIn(500); });
                nownow += 1;
            } else {
                NN = 0;
                $('#slides li').eq(nownow).css('z-index', '900');
                $('#slides li').eq(NN).stop(true, true).css({ 'z-index': '800' }).show();
                $('#slides li').eq(nownow).fadeOut(400, function () { $('#slides li').eq(0).fadeIn(500); });
                pagination.eq(NN).addClass('current').siblings('li').removeClass('current');
                nownow = 0;
            }
        }
        setTimeout(gogo, SPEED);
    }
    setTimeout(gogo, SPEED);
});