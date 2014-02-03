require(["https://v1codesamples.azurewebsites.net/js/queryRun.js"], function (queryRun) {
    var findByNameSubstring = "Newbie";
    var server = "https://ec2-54-227-126-9.compute-1.amazonaws.com/VersionOne/rest-1.v1/Data/";
    var query = "PrimaryWorkitem?accept=application/json&sel=Name,Number&find=" + findByNameSubstring + "&findin=Name";
    queryRun(server, query, console.log);
});