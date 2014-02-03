require(["https://v1codesamples.azurewebsites.net/js/ajaxQuery.js"], function (ajaxQuery) {
    var findByNameSubstring = "Newbie";
    var server = "https://ec2-54-227-126-9.compute-1.amazonaws.com/VersionOne/rest-1.v1/Data/";
    var query = "PrimaryWorkitem?accept=application/json&sel=Name,Number&find=" + findByNameSubstring + "&findin=Name";
    // Note: console.console.log to print to the original browser console instead of the special output console.
    ajaxQuery(server, query, console.log);
});