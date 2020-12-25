
// Value is the element to be validated, params is the array of name/value pairs of the parameters extracted from the HTML, element is the HTML element that the validator is attached to
$.validator.addMethod("dategreaterthan", function (value, element, params) {
    let now = new Date();
    return Date.parse(value) < now;
});

$.validator.unobtrusive.adapters.add("dategreaterthan", ["otherpropertyname"], function (options) {
    options.rules["dategreaterthan"] = "#" + options.params.otherpropertyname;
    options.messages["dategreaterthan"] = options.message;
});