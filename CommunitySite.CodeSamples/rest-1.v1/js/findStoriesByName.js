var findByNameSubstring = "Newbie";

var server = "https://ec2-54-227-126-9.compute-1.amazonaws.com/VersionOne/rest-1.v1/Data/PrimaryWorkitem?accept=application/json&sel=Name,Number&find=" + findByNameSubstring + "&findin=Name";
var username = "admin";
var password = "admin"
function setAuthorizationHeaders(req) { req.setRequestHeader("Authorization", "Basic " + btoa(username + ":" + password)) }

var settings = {
    url: server,
    beforeSend: setAuthorizationHeaders
    // headers: { Authorization: "Basic " + btoa(username + ":" + password) } // Should also work
};
// Requires jQuery to be loaded:
$.ajax(settings).done(function (data) {
    console.log(JSON.stringify(data, null, 4));
}).fail(function (xhr) {
    console.log('Request failed:' + xhr.responseText);
});