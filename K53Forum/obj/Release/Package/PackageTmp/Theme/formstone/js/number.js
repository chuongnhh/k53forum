/*! formstone v1.3.0 [number.js] 2016-10-23 | GPL-3.0 License | formstone.it */

!function(a){"function"==typeof define&&define.amd?define(["jquery","./core"],a):a(jQuery,Formstone)}(function(a,b){"use strict";function c(){v=b.$body}function d(a){var b=parseFloat(this.attr("min")),c=parseFloat(this.attr("max"));a.min=!(!b&&0!==b)&&b,a.max=!(!c&&0!==c)&&c,a.step=parseFloat(this.attr("step"))||1,a.timer=null,a.digits=o(a.step),a.disabled=this.is(":disabled")||this.is("[readonly]");var d="";d+='<button type="button" class="'+[s.arrow,s.up].join(" ")+'" aria-hidden="true" tabindex="-1">'+a.labels.up+"</button>",d+='<button type="button" class="'+[s.arrow,s.down].join(" ")+'" aria-hidden="true" tabindex="-1">'+a.labels.down+"</button>",this.wrap('<div class="'+[s.base,a.theme,a.customClass,a.disabled?s.disabled:""].join(" ")+'"></div>').after(d),a.$container=this.parent(r.base),a.$arrows=a.$container.find(r.arrow),this.on(t.focus,a,i).on(t.blur,a,j).on(t.keyPress,a,k),a.$container.on([t.touchStart,t.mouseDown].join(" "),r.arrow,a,l),n(a,0)}function e(a){a.$arrows.remove(),this.unwrap().off(t.namespace)}function f(a){a.disabled&&(this.prop("disabled",!1),a.$container.removeClass(s.disabled),a.disabled=!1)}function g(a){a.disabled||(this.prop("disabled",!0),a.$container.addClass(s.disabled),a.disabled=!0)}function h(a){var b=parseFloat(a.$el.attr("min")),c=parseFloat(a.$el.attr("max"));a.min=!(!b&&0!==b)&&b,a.max=!(!c&&0!==c)&&c,a.step=parseFloat(a.$el.attr("step"))||1,a.timer=null,a.digits=o(a.step),a.disabled=a.$el.is(":disabled")||a.$el.is("[readonly]"),n(a,0)}function i(a){a.data.$container.addClass(s.focus)}function j(a){n(a.data,0),a.data.$container.removeClass(s.focus)}function k(a){var b=a.data;38!==a.keyCode&&40!==a.keyCode||(a.preventDefault(),n(b,38===a.keyCode?b.step:-b.step))}function l(b){u.killEvent(b),m(b);var c=b.data;if(!c.disabled&&b.which<=1){var d=a(b.target).hasClass(s.up)?c.step:-c.step;c.timer=u.startTimer(c.timer,300,function(){c.timer=u.startTimer(c.timer,125,function(){n(c,d,!1)},!0)}),n(c,d),v.on([t.touchEnd,t.mouseUp].join(" "),c,m)}}function m(a){u.killEvent(a);var b=a.data;u.clearTimer(b.timer,!0),v.off(t.namespace)}function n(b,c){var d=parseFloat(b.$el.val()),e=c;"undefined"===a.type(d)||isNaN(d)?e=b.min!==!1?b.min:0:b.min!==!1&&d<b.min?e=b.min:e+=d;var f=(e-b.min)%b.step;0!==f&&(e-=f),b.min!==!1&&e<b.min&&(e=b.min),b.max!==!1&&e>b.max&&(e=b.max),e!==d&&(e=p(e,b.digits),b.$el.val(e).trigger(t.raw.change,[!0]))}function o(a){var b=String(a);return b.indexOf(".")>-1?b.length-b.indexOf(".")-1:0}function p(a,b){var c=Math.pow(10,b);return Math.round(a*c)/c}var q=b.Plugin("number",{widget:!0,defaults:{customClass:"",labels:{up:"Up",down:"Down"},theme:"fs-light"},classes:["arrow","up","down","disabled","focus"],methods:{_setup:c,_construct:d,_destruct:e,enable:f,disable:g,update:h}}),r=q.classes,s=r.raw,t=q.events,u=q.functions,v=null});