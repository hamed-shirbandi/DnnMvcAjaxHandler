var dnn = (function () {
    var config = {
        activePageUrl: $("#dnn-active-page-full-url").val(),
        moduleId: $("#dnn-module-id").val(),
        moduleTabId: $("#dnn-module-tab-id").val()
    };

    var formPreventDefault = function () {
        $(document).on("keypress", function (e) {
            if (e.which == 13) {
                $("#Form").on("submit", function (e) {
                    e.preventDefault();
                });
            }
        });
    }

    return {
        config: config,
        formPreventDefault: formPreventDefault
    };
})();

var ajaxHelper = (function () {

    var loading = function (el) {
        if (el === undefined) { el = "#global-ajax-loading-inner"; }
        var el = $(el);

        show = function () {
            el.show();
        },

        hide = function () {
            el.hide();
        };

        return {
            show: show,
            hide: hide
        }
    };

    var call = function (actionName, controllerName, params, onConfirm, updateElementId, loadingElementId) {


        loading(loadingElementId).show();

        return $.ajax({
            type: 'get',
            url: "/DesktopModules/MVC/DnnMvcAjaxHandlerExample/" + controllerName + "/" + actionName,
            data: params,
            headers: {
                "ModuleId": dnn.config.moduleId,
                "TabId": dnn.config.moduleTabId,
                "RequestVerificationToken": $("input[name='__RequestVerificationToken']").val()
            },
            success: function (data) { },
            error: function () { loading(loadingElementId).hide(); }
        });
    };

    var search = function (actionName, controllerName, params, updateElementId, loadingElementId) {

        loading(loadingElementId).show();

        return $.ajax({
            type: 'get',
            // url: dnn.config.activePageUrl+"/moduleId/"+ dnn.config.moduleId +"/controller/"+controllerName+"/action/"+actionName,
            url: "/DesktopModules/MVC/DnnMvcAjaxHandlerExample/" + controllerName + "/" + actionName,
            data: params,
            headers: {
                "ModuleId": dnn.config.moduleId,
                "TabId": dnn.config.moduleTabId,
                "RequestVerificationToken": $("input[name='__RequestVerificationToken']").val()
            },
            success: function (data) {

                $(updateElementId).html(data);
                loading(loadingElementId).hide();
            },
            error: function () {
                loading(loadingElementId).hide();
            }
        });
    };

    return {
        call: call,
        search: search,
        loading: loading
    };
})();



/*///
description here
*///
function deleteRaw(rowId, fadeColor) {
    $('tr[data-item-id="' + rowId + '"]').css("color", "white").css("background-color", fadeColor).fadeOut("slow");
}



/*///
description here
*///
function updateElementHtml(updateElementId, data) {
    $(updateElementId).html(data);
}



/*///
description here
*///
function redirectToList(controllerName, result, msg) {
    if (result === true) {
        window.location.href = dnn.config.activePageUrl + "/moduleId/" + dnn.config.moduleId + "/controller/" + controllerName + "/action/index";
    } else {
        showMsg(result, msg);
    }
}



/*///
description here
*///
function reloadPage(result, msg) {
    if (result === true) {
        window.location.reload();
    } else {
        showMsg(result, msg);
    }
}



/*///
description here
*///
function showMsg(result, msg) {

    if (result === true) {

        noty({
            text: msg,
            type: 'success',
            layout: 'center'
        });
    } else {
        noty({
            text: msg,
            type: 'error',
            layout: 'center'
        });

    }
}




/*///
description here
*///
function toObject(arr) {
    var rv = {};

    for (var i = 0; i <= arr.length; i++) {
        for (var key in arr[i]) {
            rv[key] = arr[i][key];
        }
    }
    return rv;
}


$(document).on("click", ".ajax-call", function (e) {

    var self = $(e.target);

    var params = self.attr("data-params");
    var clickedItemId = self.attr("data-item-id");
    var actionName = self.attr("data-actionName");
    var controllerName = self.attr("data-controllerName");
    var fadeColor = self.attr("data-fade-color");
    var onConfirm = self.attr("data-onConfirm");
    var onSuccess = self.attr("data-onSuccess");
    var updateElementId = self.attr("data-update-element-id");
    var loadingElementId = self.attr("data-loading-element-id");

    var prom = function () {
        var promise = ajaxHelper.call(
            actionName,
            controllerName,
            params,
            onConfirm,
            updateElementId,
            loadingElementId);
        promise.success(function (data) {
            ajaxHelper.loading(loadingElementId).hide();
            if (onSuccess === "deleteRaw") {
                deleteRaw(clickedItemId, fadeColor);
            } else if (onSuccess === "reloadPage") {
                reloadPage(data.result, data.message);
            } else if (onSuccess === "updateElementHtml") {
                updateElementHtml(updateElementId, data);
            } else if (onSuccess === "redirectToList") {
                redirectToList(controllerName, data.result, data.message);

            } else if (onSuccess == "showMsg") {
                showMsg(data.result, data.message);
            }

        });
    };

    if (onConfirm !== "") {
        bootbox.confirm({
            size: "large",
            message: onConfirm,
            title: '<div class="alert alert-warning">Warns of data loss</div>',
            buttons: {
                confirm: {
                    label: '<i class="glyphicon glyphicon-ok"></i> Yes, im sure',
                    className: 'btn-success'
                },
                cancel: {
                    label: '<i class="glyphicon glyphicon-remove"></i> No, i dont',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    prom();
                }
            }
        });
    } else {
        prom();
    }

});

$(document).on("click", ".ajax-search", function (e) {

    e.preventDefault();
    var self = $(e.target);
    var targetFormId = self.attr("data-target-form-id");
    var actionName = self.attr("data-actionName");
    var controllerName = self.attr("data-controllerName");
    var onSuccess = self.attr("data-onSuccess");
    var updateElementId = self.attr("data-update-element-id");
    var loadingElementId = self.attr("data-loading-element-id");




    var paramsArray = [];
    $(targetFormId).find(':input').each(function (index, el) {
        var name = $(this).attr('name');
        var val = $(this).val();
        var el = {};
        el[name] = val;
        if (name !== undefined) {
            paramsArray.push(el);

        }
    })


    var params = toObject(paramsArray);

    var promise = ajaxHelper.search(actionName, controllerName, params, updateElementId, loadingElementId);


    promise.success(function () {
        ajaxHelper.loading(loadingElementId).hide();
    });

});

$(document).on("click", ".ajax-paging", function (e) {

    e.preventDefault();
    var self = $(this);
    var targetFormId = self.attr("data-target-form-id");
    var actionName = self.attr("data-actionName");
    var controllerName = self.attr("data-controllerName");
    var params = self.attr("data-params");
    var onSuccess = self.attr("data-onSuccess");
    var updateElementId = self.attr("data-update-element-id");
    var loadingElementId = self.attr("data-loading-element-id");

    ajaxHelper.search(actionName, controllerName, params, updateElementId, loadingElementId);


});


$(document).ready(function () {
    dnn.formPreventDefault();
});

