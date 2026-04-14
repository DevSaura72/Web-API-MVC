    /// <reference path="Common.js" />
function LoadGridSettings(grid) {
    $.fn.dataTable.render.ellipsis = function (cutoff, wordbreak, escapeHtml) {
        var esc = function (t) {
            return t
                .replace(/&/g, '&amp;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;')
                .replace(/"/g, '&quot;');
        };

        return function (d, type, row) {
            // Order, search and type get the original data
            if (type !== 'display') {
                return d;
            }

            if (typeof d !== 'number' && typeof d !== 'string') {
                return d;
            }

            d = d.toString(); // cast numbers

            if (d.length <= cutoff) {
                return d;
            }

            var shortened = d.substr(0, cutoff - 1);

            // Find the last white space character in the string
            if (wordbreak) {
                shortened = shortened.replace(/\s([^\s]*)$/, '');
            }

            // Protect against uncontrolled HTML input
            if (escapeHtml) {
                shortened = esc(shortened);
            }
            //alert(shortened);
            return '<span class="ellipsis" data-toggle="tooltip" title="' + esc(d) + '">' + shortened + '&#8230;</span>';
        };
    };

    //$.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap pull-right';

    //new $.fn.dataTable.Buttons(grid, {
    //    buttons: [
    //      {
    //          "extend": "copy",
    //          "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
    //          "className": "btn btn-white btn-primary btn-bold"
    //      },
    //      {
    //          "extend": "csv",
    //          "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
    //          "className": "btn btn-white btn-primary btn-bold",
    //          exportOptions: { modifier: { search: 'none' } },
    //          filename: 'Materials'
    //      },
    //      {
    //          "extend": "excel",
    //          "text": "<i class='fa fa-file-excel-o bigger-110 green'></i> <span class='hidden'>Export to Excel</span>",
    //          "className": "btn btn-white btn-primary btn-bold",
    //          exportOptions: { modifier: { search: 'none' } },
    //          filename: 'Materials'
    //      },
    //      {
    //          "extend": "pdf",
    //          "text": "<i class='fa fa-file-pdf-o bigger-110 red'></i> <span class='hidden'>Export to PDF</span>",
    //          "className": "btn btn-white btn-primary btn-bold",
    //          exportOptions: { modifier: { search: 'none' } },
    //          filename: 'Materials'
    //      },
    //      {
    //          "extend": "print",
    //          "text": "<i class='fa fa-print bigger-110 grey'></i> <span class='hidden'>Print</span>",
    //          "className": "btn btn-white btn-primary btn-bold",
    //          autoPrint: false,
    //          message: ''
    //      }
    //    ]
    //});

    //$("<div class='row' style='padding-bottom:5px;'><div id='dataTables_buttons' class='col-xs-12'></div></div>").index(grid.table().container());
    //$("#dataTables_buttons").append(grid.buttons(0, null).container());

    ////style the message box
    //var defaultCopyAction = grid.button(1).action();
    //grid.button(1).action(function (e, dt, button, config) {
    //    defaultCopyAction(e, dt, button, config);
    //    $('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
    //});


    //var defaultColvisAction = grid.button(0).action();
    //grid.button(0).action(function (e, dt, button, config) {

    //    defaultColvisAction(e, dt, button, config);
    //    if ($('.dt-button-collection > .dropdown-menu').length == 0) {
    //        $('.dt-button-collection')
    //        .wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
    //        .find('a').attr('href', '#').wrap("<li />")
    //    }
    //    $('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
    //});

    //setTimeout(function () {
    //    $($('.dataTables_wrapper')).find('a.dt-button').each(function () {
    //        var div = $(this).find(' > div').first();
    //        if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
    //        else $(this).tooltip({ container: 'body', title: $(this).text() });
    //    });
    //}, 500);

    //$('.dataTables_filter').addClass('pull-right');
    ////$('.dataTables_filter > input').addsty('pull-right');
    //$('.dataTables_paginate').addClass('pull-right');

}

function DataTableFormatedDate(value) {
    if (value === null) return "";
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function DataTableFormatedDateTime(value) {
    if (value === null) return "";
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear()+" "+dt.getHours()+":"+dt.getMinutes()+":"+dt.getSeconds()+":"+dt.getMilliseconds();
}

function loadTooltip() {
    $('[data-toggle="tooltip"]').tooltip();
}


function display_kendoui_grid_error(e) {
    if (e.errors) {
        if ((typeof e.errors) == 'string') {
            //single error
            //display the message
            alert(e.errors);
        } else {
            //array of errors
            //source: http://docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/faq#how-do-i-display-model-state-errors?
            var message = "The following errors have occurred:";
            //create a message containing all errors.
            $.each(e.errors, function (key, value) {
                if (value.errors) {
                    message += "\n";
                    message += value.errors.join("\n");
                }
            });
            //display the message
            alert(message);
        }
        //ignore empty error
    } else if (e.errorThrown) {
        alert('Error happened');
    }
}

// CSRF (XSRF) security
function addAntiForgeryToken(data) {
 
    //if the object is undefined, create a new one.
    if (!data) {
        data = {};
    }
    //add token
    var tokenInput = $('input[name=__RequestVerificationToken]');
    //alert(tokenInput);
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};

function DisableCopyPaste(textbox){
    textbox.bind("cut copy paste", function (e) {
        e.preventDefault();
    });
}

// date validation for format dd-MMM-yyyy
$(function () {
    $.validator.addMethod('date',
    function (value, element) {
        //debugger;
        if (this.optional(element)) {
            return true;
        }
        var valid = true;
        try {
            var format = element.attributes['dataformat'].value;
            if (format == null) {
                return kendo.parseDate(value) != null;
            }
            return kendo.parseDate(value, format) != null;
        }
        catch (err) {
            valid = false;
        }
        return valid;
    });
});

// delete back button in browser
jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();

(function (global) {

    if (typeof (global) === "undefined") {
        throw new Error("window is undefined");
    }

    var _hash = "!";
    var noBackPlease = function () {
        global.location.href += "#";

        // making sure we have the fruit available for juice....
        // 50 milliseconds for just once do not cost much (^__^)
        global.setTimeout(function () {
            global.location.href += "!";
        }, 50);
    };

    // Earlier we had setInerval here....
    global.onhashchange = function () {
        if (global.location.hash !== _hash) {
            global.location.hash = _hash;
        }
    };

    global.onload = function () {

        noBackPlease();

        // disables backspace on page except on input fields and textarea..
        document.body.onkeydown = function (e) {
            var elm = e.target.nodeName.toLowerCase();
            if (e.which === 8 && (elm !== 'input' && elm !== 'textarea')) {
                e.preventDefault();
            }
            // stopping event bubbling up the DOM tree..
            e.stopPropagation();
        };

    };

})(window);

