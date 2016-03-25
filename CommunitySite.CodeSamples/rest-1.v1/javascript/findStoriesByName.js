require(["https://v1codesamples.azurewebsites.net/js/ajaxQuery.js"], function (ajaxQuery) {
    var server = "http://thingproxy.freeboard.io/fetch/https://www14.v1host.com/v1sdktesting/rest-1.v1/Data/";
    var query = "Member/20?accept=application/json&sel=Name";
    // Note: console.console.log to print to the original browser console instead of the special output console.
    ajaxQuery(server, query, console.log);
});
