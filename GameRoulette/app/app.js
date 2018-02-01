var hub = $.connection.chatHub;
hub.client.message = function (msg) {
    $("#message").append("<li>" + msg + "</li><br />")
}

hub.client.user = function (msg) {
    $("#user").append("<li>" + msg + "</li><br />")
}

$.connection.hub.start(function () {
    $("#send").click(function () {
        hub.server.send($("#txt").val());
        $("#txt").val(" ");
    })
})