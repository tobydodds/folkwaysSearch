(function ($) {
    $(function () {

        var howManySecondsSinceAddedToCart = function () {
            var result = Number.MAX_VALUE;

            if (window.location.href.indexOf("addedtocart=") != -1) {
                var matches = window.location.href.match(/addedtocart=([^&]*)/);
                var savedTimeInMs = matches[1];
                var nowInMs = (new Date()).getTime();
                var diffInSec = ((nowInMs - savedTimeInMs) / 100);

                result = diffInSec;
            }

            return result;
        };

        var showNotification = function (message, success) {
            var cssClass = success ? "success" : "error";
            var messageElement = $("<div class=\"add-to-cart-message " + cssClass + "\">" + message + "</div>");

            $("body").append(messageElement);
            messageElement.dialog({
                show: { effect: 'fade', duration: 100 },
                hide: { effect: 'fade', duration: 100 },
                minHeight: 80,
                open: function (e, ui) {
                    var element = $(this);
                    setTimeout(function () {
                        element.dialog('close');
                    }, 2000);
                },
                close: function (e, ui) {
                    messageElement.remove();
                }
            });
            $(".ui-dialog-titlebar").hide();
        };

        $(function () {
            if (howManySecondsSinceAddedToCart() < 5) {
                var message = $(".buy-form").data("success-message");
                showNotification(message, true);
            }
        });

        $(".buy-form").on("submit", function (e) {
            var redirect = $(this).find('input[name=redirect]');
            var url = $(redirect).val();
            var urlParam = "addedtocart=";
            $(redirect).val(url.replace(urlParam, urlParam + (new Date()).getTime()));
        });
    });
})(jQuery);