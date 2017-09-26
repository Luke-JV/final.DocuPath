/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    config.toolbarGroups = [
		{ name: 'editing', groups: ['find'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'basicstyles', groups: ['basicstyles'] },
		{ name: 'document', groups: ['mode', 'document'] },
		{ name: 'colors' },
		{ name: 'insert' },
		{ name: 'styles' },
		{ name: 'paragraph', groups: ['list', 'indent', 'align'] },
		{ name: 'tools' },
    ];
    config.removeButtons = 'Image,Flash,Table,Smiley,Iframe,Source,Print,Preview,NewPage,Save,Formattingstyles,Styles,PageBreak,Undo,Redo,Strikethrough,ShowBlocks,PasteFromWord,Font,Replace';

    // Set the most common block elements.
    config.format_p = { element: 'p', attributes: { 'class': 'ckp' } };
    config.format_h1 = { element: 'p', attributes: { 'class': 'ckh1' } };
    config.format_h2 = { element: 'p', attributes: { 'class': 'ckh2' } };
    config.format_h3 = { element: 'p', attributes: { 'class': 'ckh3' } };
    config.format_h4 = { element: 'p', attributes: { 'class': 'ckh4' } };
    config.format_tags = 'p;h1;h2;h3;h4';

    // Simplify the dialog windows.
    config.removeDialogTabs = 'image:advanced;link:advanced';
    //config.stylesSet = false;
    config.skin = "office2013";
};
