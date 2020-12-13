var Response = {
    onComplete: function (e) {

    },

    onClose: function (container, callback) {
        jsonResponseContainer.Container = container;
        $("#" + container).hide(500, callback);
    },

    cleanUp: function () {
        $('#jsonResponse' + jsonResponseContainer.Container).html("<div id='jsonSummary" + jsonResponseContainer.Container + "'></div>");
        //$("#" + jsonResponseContainer.Container).show();
    }
}

var jsonResponseContainer = {
    Container: ''
}

function jsonResponse(container, response) {
    this.Container = container
    var r = null;
    if (response != null) {
        r = $.parseJSON(response);
    }
    this.Initialize(r, initTemplate);
}

function initTemplate(jResponse, response) {
    jResponse.onSuccess(response);
}

jsonResponse.prototype.onBegin = function (e, container) {
    jsonResponseContainer.Container = container;
    $('#progress' + jsonResponseContainer.Container).show();
    $("#jsonSummary" + jsonResponseContainer.Container).hide();
}

jsonResponse.prototype.onComplete = function (e) {

}

jsonResponse.prototype.onSuccess = function (e) {
    var responseArray = [];
    if (e != null) {
        if (jsonResponseContainer.Container != null && jsonResponseContainer.Container.length > 0) {
            e.Container = jsonResponseContainer.Container;
            responseArray[0] = e;
            try {
                $('#jsonResponse' + jsonResponseContainer.Container).html("<div id='jsonSummary" + jsonResponseContainer.Container + "'></div>");
                $("#jsonResponse").tmpl(responseArray).appendTo('#jsonSummary' + jsonResponseContainer.Container);
                $("#jsonSummary" + jsonResponseContainer.Container).hide();
                $("#" + jsonResponseContainer.Container).show();
                $("#jsonSummary" + jsonResponseContainer.Container).show('Blind', null, 500, null);
            }
            catch (ex) {
                alert(ex.Message);
            }

            $('#progress' + jsonResponseContainer.Container).hide();
        }
    }
}

jsonResponse.prototype.Initialize = function (response, callback) {
    jsonResponseContainer.Container = this.Container;
    var jResponse = this;
    var arr = [];
    var responseArray = [];
    var item = {
        Container: jsonResponseContainer.Container
    };
    arr[0] = item;
    $("#jsonResponseLayout").tmpl(arr).appendTo('#' + jsonResponseContainer.Container);
    if (response != null) {
        this.onSuccess(response);

        //$("#jsonSummary" + jsonResponseContainer.Container).show('blind', null, 500, null);
        //                if (response != null) {
        //                    if (callback != null) {
        //                        callback(jResponse, response);
        //                    }
        //                }
    }
}

function done() {
    alert('done');
}

function jsonGridResponse(container, response) {
    this.displayResponse = true;
    this.jsonResponse = new jsonResponse(container, response);
}

jsonGridResponse.prototype.onRowDataBound = function (e) {
    if (this.displayResponse == true && e.dataItem.Tag != null) {
        var result = JSON.parse(e.dataItem.Tag);
        this.jsonResponse.onSuccess(result);
        this.displayResponse = false;
    }
}

jsonGridResponse.prototype.onSave = function (e) {
    this.displayResponse = true
    this.jsonResponse.onBegin(null, this.jsonResponse.Container);
}

jsonGridResponse.prototype.onDelete = function (e) {
    this.displayResponse = true
    this.jsonResponse.onBegin(null, this.jsonResponse.Container);
}