require(["https://v1codesamples.azurewebsites.net/js/ajaxQuery.js"], function (ajaxQuery) {
    var findByNameSubstring = "Newbie";
    var server = "https://ec2-54-227-126-9.compute-1.amazonaws.com/VersionOne/rest-1.v1/Data/";
    var query = "PrimaryWorkitem?accept=application/json&sel=Name,Number&find=" + findByNameSubstring + "&findin=Name";
    ajaxQuery(server, query, console.log); // uses our special console.log replacement function
});