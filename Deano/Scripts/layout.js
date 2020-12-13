var Layout = {
    ContentPanel: '#cont_bot',
    IndexContentPanel: '#center',
    AccountContentPanel: '#accountContent',
    EditPanel: '#accountContent2',
    CurrentPanelContent: '',
    WalleyesPanel: '#walleyes',

    SwapPanelForward: function (path, panel, callback) {
        $.ajax({ url: path, async: false, cache: false, dataType: "html", success: function (result) {
            Layout.CurrentPanelContent = $(panel).html();
            $(panel).html(result);
            jQuery.validator.unobtrusive.parse(panel);
            if (callback != null) {
                callback();
            }
        },
            error: function (request, errorMsg) {
                alert(request.responseText);
            }
        });
    },

    SwapPanelBack: function (panel) {
        $(panel).html('');
        $(panel).html(Layout.CurrentPanelContent);
        Layout.CurrentPanelContent = '';
    },

    SwapPanelBackHide: function (panel) {
        $(panel).hide();
        $(panel).html(Layout.CurrentPanelContent);
        Layout.CurrentPanelContent = '';
    },

    SetCurrentPanelContent: function (path, panel) {
        $.ajax({ url: path, async: false, cache: false, dataType: "html", success: function (result) {
            Layout.CurrentPanelContent = result;
        },
            error: function (request, errorMsg) {
                alert(request.responseText);
            }
        });
    },

    RefreshPanel: function (path, panel, callback) {
        $.ajax({ url: path, async: false, cache: false, dataType: "html", success: function (result) {
            $(panel).html(result);
            jQuery.validator.unobtrusive.parse(panel);
            if (callback != null) {
                callback();
            }
        },
            error: function (request, errorMsg) {
                alert(request.responseText);
            }
        });
    },

    LoadContentPanel: function (path, callback) {
        //        var slideshowElement = $('#slideshow');
        //        if (slideshowElement != null) {
        //            var $element = $('#slideshow').slideshow(); // create slideshow, store element
        //            if ($element != null) {
        //                $element.slideshow('stop'); // $element API
        //                var slideshow = $element.data('slideshow'); // get slideshow instance
        //                if (slideshow != null) {
        //                    slideshow.stop(); // object API
        //                }
        //            }
        //        }
        $.ajax({ url: path, async: false, cache: false, dataType: "html", success: function (result) {
            $(Layout.IndexContentPanel).html(result);
            jQuery.validator.unobtrusive.parse(Layout.IndexContentPanel);
            if (callback != null) {
                callback();
            }
        },
            error: function (request, errorMsg) {
                alert(request.responseText);
            }
        });
    },

    LoadTabPanel: function (tab, path, validationCallback) {
        $.ajax({ url: path, async: false, cache: false, dataType: "html", success: function (result) {
            $(tab).html(result);
            jQuery.validator.unobtrusive.parse(Layout.ContentPanel);
            if (validationCallback != null) {
                validationCallback();
            }
        },
            error: function (request, errorMsg) {
                alert(request.responseText);
            }
        });

    }

}

