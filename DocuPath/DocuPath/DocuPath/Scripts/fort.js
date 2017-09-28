var Fort = {
    clean: function clean() {
        var forms = document.querySelectorAll(".form");
        //console.log(forms.length);
        for (var i = forms.length; i--;) {
            var nonemptyelements = forms[i].querySelectorAll("input, select");
            //Array.prototype.forEach.call(nonemptyelements, function (el) {
            //    //console.log('ID: ' + el.id + '\t\t\t\t\tTag: ' + el.tagName.toLowerCase() + '\t\t\t\t\tLength: ' + el.value.length + '\t\t\t\t\tSelectedIndex: ' + el.selectedIndex);
            //    if (el.value.length != 0) {
            //        if (el.classList)
            //            el.classList.add('ignore');
            //        else
            //            el.className += ' ' + 'ignore';
            //    } else if (el.selectedIndex != 0) {
            //        if (el.classList)
            //            el.classList.add('ignore');
            //        else
            //            el.className += ' ' + 'ignore';
            //    }                
            //});
            //var selectelements = forms[i].querySelectorAll("select");
            //Array.prototype.forEach.call(selectelements, function (el) {
            //    if (el.selectedIndex != 0)
            //        if (el.classList)
            //            el.classList.add('ignore');
            //        else
            //            el.className += ' ' + 'ignore';
            //});
            var hiddenInputs = forms[i].querySelectorAll('input[type=hidden], input[readonly=true]')
            Array.prototype.forEach.call(hiddenInputs, function (el) {
                if (el.classList)
                    el.classList.add('ignore');
                else
                    el.className += ' ' + 'ignore';
            });
        };


        
        //nonemptyelements. selectelements;
        //console.log(nonemptyelements);
    },

    solid: function solid(hex, targetId/*, ckEmpty*/) {
        Fort.clean();
        var target = document.querySelector(targetId);
        target.innerHTML = '<div class="top-one" id="top1"><div class="colors"></div></div>' + target.innerHTML;

        var forms = document.querySelectorAll(".form"),
            inputs = [];
        for (var i = forms.length; i--;) {
            var els = forms[i].querySelectorAll("input, select");
            //console.log(els.length);
            for (var j = els.length; j--;) {
                classes = els[j].className.replace(/\s+/g, ' ').split(' ');
                //console.log('Classes for: ' + els[j].id + ' = ' + classes);
                ignore = false;
                for (var k = classes.length; k--;) {
                    if (classes[k] == "ignore") {
                        ignore = true;
                        break;
                    }
                }
                if (els[j].type != "button" && els[j].type != "submit" && !ignore) {
                    inputs.push(els[j]);
                    els[j].addEventListener("input", cback, false);
                }
            }
        }
        //console.log(inputs);

        function cback(e) {
            var t = [];
            //console.log(inputs.length);
            for (var n = inputs.length; n--;) {
                if (!inputs[n].value.length) { t.push(inputs[n]) }
                else if (inputs[n].selectedIndex == 0) { t.push(inputs[n]) }
                //else if ($.trim(inputs[n]) == '') { t.push(inputs[n]) }
            }
            var r = t.length;
            var i = inputs.length;
            var s = document.querySelectorAll(".top-one");
            for (var o = s.length; o--;) {
                var width = 100 - (r / i * 100) + "%";
                //console.log(width);
                s[o].style.width = width;
                if (width == '0%') {
                    document.getElementById("top1").style.visibility = 'hidden';
                } else {
                    s[o].innerHTML = Math.round(100 - r / i * 100, 2) + "%";
                }
            }

            //Set color of bar as solid
            document.getElementById("top1").style.background = hex;
        }
    },

    //solid: function solid(hex, formId, pbarContainerId, targetBarId) {
    //    //Fort.clean();
    //    //function clean() {
    //    //}
    //    var form = document.querySelector(formId);
    //    //console.log(form.childNodes.length);
    //    //for (var i = form.childNodes.length; i--;) {

    //        var nonemptyelements = form.querySelectorAll("input, textarea");
    //        Array.prototype.forEach.call(nonemptyelements, function (el) {
    //            if (el.value.length != 0)
    //                if (el.classList)
    //                    el.classList.add('ignore');
    //                else
    //                    el.className += ' ' + 'ignore';
    //        });
    //        var nondefaultselectelements = form.querySelectorAll("select");
    //        Array.prototype.forEach.call(nondefaultselectelements, function (el) {
    //            if (el.selectedIndex != -1)
    //                if (el.classList)
    //                    el.classList.add('ignore');
    //                else
    //                    el.className += ' ' + 'ignore';
    //        });
    //        var hiddenInputs = form.querySelectorAll('input[type=hidden]')
    //        Array.prototype.forEach.call(hiddenInputs, function (el) {
    //            if (el.classList)
    //                el.classList.add('ignore');
    //            else
    //                el.className += ' ' + 'ignore';
    //        });
    //    //};
    //    //console.log(nonemptyelements);
    //    //console.log(selectelements);
    //    //console.log(hiddenInputs);
    //    //var allForms = document.querySelectorAll('.form');
    //    //var formLength = allForms.length;
    //    //for (var index = formLength; index--;) {
    //        //console.log(index);
    //        //alert('wait');
    //        //var $target = document.getElementById(target);
    //        //console.log($target);
    //    //var $target = $(this).closest('.fort-container');
    //    var target = document.querySelector(pbarContainerId);
    //    //console.log(target);
    //    //for (var c = $target.length; c--;) {
    //    target.innerHTML = '<div class="actual-pbar" id="' + targetBarId + '"><div class="some-olors"></div></div>' + target.innerHTML;
    //    //console.log(target);
    //    console.log('fine up to innerHTML injection');
    //    //}
    //        //$target.innerHTML = '<div class="top-one" id="top1"><div class="colors"></div></div>' + $target.innerHTML;
    //    //var form = document.querySelector(formId), // Necessary? Exists already?
    //    var inputs = [];
    //        //for (var i = form.length; i--;) {
    //            var els = form.querySelectorAll("input, textarea, select");
    //            for (var j = els.length; j--;) {
    //                classes = els[j].className.replace(/\s+/g, ' ').split(' ');
    //                ignore = false;
    //                for (var k = classes.length; k--;) {
    //                    if (classes[k] == "ignore") {
    //                        ignore = true;
    //                        break;
    //                    }
    //                }
    //                if (els[j].type != "button" && els[j].type != "submit" && !ignore) {
    //                    inputs.push(els[j]);
    //                    els[j].addEventListener("input", cback, false);
    //                }
    //            }
    //        //}    
    //    console.log('Number Of Inputs: '+inputs.length);

    //        function cback(e) {
    //            var t = [];
    //            for (var n = inputs.length; n--;) {
    //                console.log(inputs.length);
    //                if (!inputs[n].value.length) t.push(inputs[n]);
    //            }
    //            console.log(t);
    //            var r = t.length;
    //            var i = inputs.length;
    //            var s = document.querySelector(targetBarId);
    //            console.log(s);
    //            //for (var o = s.length; o--;) {
    //            //s[o].style.width = 100 - r / i * 100 + "%";
    //            s.style.width = 100 - r / i * 100 + "%";
    //                //$target.innerHTML = 100 - r / i * 100 + "%";
    //            //}

    //            //Set color of bar as solid
    //            //document.getElementById("top1").style.background = hex;
    //            document.querySelector(targetBarId).style.background = hex;
    //        }
    //    //}
    //},

    config: function (settings) {
        var t1 = document.querySelector('#top1');
        var t2 = document.querySelector('#top2') || { style: {} };
        if (settings.height) {
            t1.style.height = settings.height;
            t2.style.height = settings.height;
        }
        if (settings.alignment) {
            if (settings.alignment === 'top') {
                t1.style.top = 0;
                t1.style.bottom = 'auto';
                t2.style.top = 0;
                t2.style.bottom = 'auto';
            } else {
                t1.style.top = 'auto';
                t1.style.bottom = 0;
                t2.style.top = 'auto';
                t2.style.bottom = 0;
            }
        }
        if (settings.duration) {
            t1.style.transitionDuration = settings.duration;
            t2.style.transitionDuration = settings.duration;
        }
    }
};

//var Fort = {
//    clean: function clean() {
//        var forms = document.querySelectorAll(".form-group");
//        for (var i = forms.length; i--;) {
//            var nonemptyelements = forms[i].querySelectorAll("input, textarea, select");
//            Array.prototype.forEach.call(nonemptyelements, function (el) {
//                if (el.value.length != 0)
//                    if (el.classList)
//                        el.classList.add('ignore');
//                    else
//                        el.className += ' ' + 'ignore';
//            });
//            var hiddenInputs = forms[i].querySelectorAll('input[type=hidden]')
//            Array.prototype.forEach.call(hiddenInputs, function (el) {
//                if (el.classList)
//                    el.classList.add('ignore');
//                else
//                    el.className += ' ' + 'ignore';
//            });
//        };
//    },
    
//    gradient: function (firstColor, secondColor, targetProgressBar, targetContainer) {
//        Fort.clean();
//        var target = document.getElementById(String(targetProgressBar));
//        var container = document.getElementById(String(targetContainer));
//        console.log(firstColor);
//        console.log(secondColor);
//        console.log(target);
//        alert('stop1')
//        console.log(container);
//        alert('stop2')
//        //document.body.innerHTML = '<div class="top-one" id="top1"><div class="colors"></div></div>' + document.body.innerHTML;

//        function cback(e) {
//            var t = [];
//            for (var n = inputs.length; n--;) {
//                if (!inputs[n].value.length) t.push(inputs[n]);
//            }
//            var r = t.length;
//            var i = inputs.length;
//            var s = document.querySelectorAll(targetProgressBar);
//            for (var o = s.length; o--;) {
//                s[o].style.width = 100 - r / i * 100 + "%";
//            }
//            orientation = 'to right';
//            var string = 'linear-gradient(' + orientation + ', ' + firstColor + ', ' + secondColor + ')';
//            target.style.background = string;
//            target.innerHTML = 100 - r / i * 100 + "%";
//        }
//        var forms = container.querySelectorAll(".form-group"),
//            inputs = [];
//        for (var i = forms.length; i--;) {
//            var els = forms[i].querySelectorAll("input, textarea, select");
//            for (var j = els.length; j--;) {
//                classes = els[j].className.replace(/\s+/g, ' ').split(' ');
//                ignore = false;
//                for (var k = classes.length; k--;) {
//                    if (classes[k] == "ignore") {
//                        ignore = true;
//                        break;
//                    }
//                }
//                if (els[j].type != "button" && els[j].type != "submit" && !ignore) {
//                    inputs.push(els[j]);
//                    els[j].addEventListener("input", cback, false);
//                }
//            }
//        }
//    },
//    config: function (settings) {
//        var t1 = document.querySelector('#top1');
//        var t2 = document.querySelector('#top2') || { style: {} };
//        if (settings.height) {
//            t1.style.height = settings.height;
//            t2.style.height = settings.height;
//        }
//        if (settings.alignment) {
//            if (settings.alignment === 'top') {
//                t1.style.top = 0;
//                t1.style.bottom = 'auto';
//                t2.style.top = 0;
//                t2.style.bottom = 'auto';
//            } else {
//                t1.style.top = 'auto';
//                t1.style.bottom = 0;
//                t2.style.top = 'auto';
//                t2.style.bottom = 0;
//            }
//        }
//        if (settings.duration) {
//            t1.style.transitionDuration = settings.duration;
//            t2.style.transitionDuration = settings.duration;
//        }
//    }
//};