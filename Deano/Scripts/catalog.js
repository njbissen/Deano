function Catalog(path) {
    this.FirstIndex = 0;
    this.SelectedIndex = 0;
    this.Total = 0;
    this.Path = path;
    this.ItemPanel = '';
    this.ItemTemplate = '';
}

Catalog.prototype.GetData = function (index, callback) {
    var c = this;
    var url = '/FishReports/_Index?firstIndex=' + this.FirstIndex + '&selectedIndex=' + index;
    if (this.Path) {
        url = this.Path + '&firstIndex=' + this.FirstIndex + '&selectedIndex=' + index;
    }
    $.ajax({ url: url, async: false, cache: false, dataType: "json", success: function (result) {
        $('#pager').html('');
        $('#catalog').html('');
        c.Total = result.Pager.Total;
        c.FirstIndex = result.Pager.FirstIndex;
        $("#pagerTemplate").tmpl(result.Pager).appendTo('#pager');
        $(c.ItemTemplate).tmpl(result.Header).appendTo('#catalog');
        if (callback != null) {
            callback();
        }
    },
        error: function (request, errorMsg) {
            alert(request.responseText);
        }
    });
}


function GetActionData(url, container, template) {
    $.ajax({ url: url, async: false, cache: false, dataType: "json", success: function (result) {
        $(container).html('');
        $(template).tmpl(result).appendTo(container);
    },
        error: function (request, errorMsg) {
            alert(request.responseText);
        }
    });
}