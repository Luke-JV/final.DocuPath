/*!
 * bootstrap-fileinput v4.4.3
 * http://plugins.krajee.com/file-input
 *
 * Font Awesome icon theme configuration for bootstrap-fileinput. Requires font awesome assets to be loaded.
 *
 * Author: Kartik Visweswaran
 * Copyright: 2014 - 2017, Kartik Visweswaran, Krajee.com
 *
 * Licensed under the BSD 3-Clause
 * https://github.com/kartik-v/bootstrap-fileinput/blob/master/LICENSE.md
 */
(function ($) {
    "use strict";

    $.fn.fileinputThemes.fa = {
        fileActionSettings: {            
            removeIcon: '<i class="mdl2icon mdl2-delete text-danger"></i>',
            uploadIcon: '<i class="mdl2icon mdl2-upload text-info"></i>',
            uploadRetryIcon: '<i class="mdl2icon mdl2-reset text-info"></i>',
            zoomIcon: '<i class="mdl2icon mdl2-view"></i>',
            dragIcon: '<i class="mdl2icon mdl2-more"></i>',
            indicatorNew: '<i class="mdl2icon mdl2-asterisk text-warning"></i>',
            indicatorSuccess: '<i class="mdl2icon mdl2-success text-success"></i>',
            indicatorError: '<i class="mdl2icon mdl2-error text-danger"></i>',
            indicatorLoading: '<i class="mdl2icon mdl2-loading text-muted"></i>'
        },
        layoutTemplates: {
            fileIcon: '<i class="mdl2icon mdl2-file kv-caption-icon"></i> '
        },
        previewZoomButtonIcons: {
            prev: '<i class="mdl2icon mdl2-left"></i>',
            next: '<i class="mdl2icon mdl2-right"></i>',
            toggleheader: '<i class="mdl2icon mdl2-toggleheader"></i>',
            fullscreen: '<i class="mdl2icon mdl2-expand"></i>',
            borderless: '<i class="mdl2icon mdl2-borderless"></i>',
            close: '<i class="mdl2icon mdl2-cancel"></i>'
        },
        previewFileIcon: '<i class="mdl2icon mdl2-view"></i>',
        browseIcon: '<i class="mdl2icon mdl2-browse"></i>',
        removeIcon: '<i class="mdl2icon mdl2-delete"></i>',
        cancelIcon: '<i class="mdl2icon mdl2-cancel"></i>',
        uploadIcon: '<i class="mdl2icon mdl2-upload"></i>',
        msgValidationErrorIcon: '<i class="mdl2icon mdl2-error"></i> '
    };
})(window.jQuery);