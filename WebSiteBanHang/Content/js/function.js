﻿var deleteSomee = function () {
    $('a[href$="http://somee.com"]').remove();
    $('script[src$="http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js"]').remove();
}

function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    }
    else {
        return true;
    }
}

var errMess = function (input_type, alert) {
    let wrapper = $(".form-signin>.form-label-group>label[for='" + input_type + "']");
    if (!$(wrapper).find(".animate-alert").length) {
        $(wrapper).after($("<div class=\"alert alert-danger animate-alert\" role=\"alert\">" + alert + "</div>"));
    }
    else {
        $(wrapper).find(".animate-alert").remove();
    }
}

var errMessTwo = function (input_type, alert) {
    let wrapper = $(".modal-body>" + input_type + "");
    if (!$(wrapper).find(".animate-alert").length) {
        $(wrapper).after($("<div class=\"alert alert-danger animate-alert\" role=\"alert\">" + alert + "</div>"));
    }
    else {
        $(wrapper).find(".animate-alert").remove();
    }
}

$(function () {
    "use strict";
    function menuscroll() {
        var $navmenu = $('.nav-menu');
        if ($(window).scrollTop() > 50) {
            $navmenu.addClass('is-scrolling');
        } else {
            $navmenu.removeClass("is-scrolling");
        }
    }
    menuscroll();
    $(window).on('scroll', function () {
        menuscroll();
    });

    $('.navbar-nav > li:not(.dropdown) > a').on('click', function () {
        $('.navbar-collapse').collapse('hide');
    });

    var siteNav = $('#navbar');
    siteNav.on('show.bs.collapse', function (e) {
        $(this).parents('.nav-menu').addClass('menu-is-open');
    })
    siteNav.on('hide.bs.collapse', function (e) {
        $(this).parents('.nav-menu').removeClass('menu-is-open');
    })

    $('a[href*="#"]').not('[href="#"]').not('[href="#0"]').not('[data-toggle="tab"]').on('click', function (event) {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                event.preventDefault();
                $('html, body').animate({
                    scrollTop: target.offset().top
                }, 1000, function () {
                    var $target = $(target);
                    $target.focus();
                    if ($target.is(":focus")) {
                        return false;
                    } else {
                        $target.attr('tabindex', '-1');
                        $target.focus();
                    };
                });
            }
        }
    });

    var $testimonialsDiv = $('.testimonials');
    if ($testimonialsDiv.length && $.fn.owlCarousel) {
        $testimonialsDiv.owlCarousel({
            items: 1,
            nav: true,
            dots: false,
            navText: ['<span class="ti-arrow-left"></span>', '<span class="ti-arrow-right"></span>']
        });
    }

    var $galleryDiv = $('.img-gallery');
    if ($galleryDiv.length && $.fn.owlCarousel) {
        $galleryDiv.owlCarousel({
            nav: false,
            center: true,
            loop: true,
            autoplay: true,
            dots: true,
            navText: ['<span class="ti-arrow-left"></span>', '<span class="ti-arrow-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 3
                }
            }
        });
    }

});

$(document).ajaxStart(function () {
    $("#pressData").show();
});

$(document).ajaxStop(function () {
    $("#pressData").hide();
});