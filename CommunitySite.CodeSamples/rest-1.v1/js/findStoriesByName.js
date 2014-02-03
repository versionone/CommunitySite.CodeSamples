function queryRun(server, query, callback, username, password) {
    username = username || "admin";
    password = password || "admin";
    callback = callback || console.log;

    function setAuthorizationHeaders(req) { req.setRequestHeader("Authorization", "Basic " + btoa(username + ":" + password)) }

    var settings = {
        url: server + query,
        beforeSend: setAuthorizationHeaders
        // headers: { Authorization: "Basic " + btoa(username + ":" + password) } // Should also work
    };
    // Requires jQuery to be loaded:
    $.ajax(settings).done(function (data) {
        callback(JSON.stringify(data, null, 4));
    }).fail(function (xhr) {
        callback(JSON.stringify({ ErrorMessage: 'Request failed:' + xhr.responseText }));
    });
}

var findByNameSubstring = "Newbie";
var server = "https://ec2-54-227-126-9.compute-1.amazonaws.com/VersionOne/rest-1.v1/Data/";
var query= "PrimaryWorkitem?accept=application/json&sel=Name,Number&find=" + findByNameSubstring + "&findin=Name";
queryRun(server, query);