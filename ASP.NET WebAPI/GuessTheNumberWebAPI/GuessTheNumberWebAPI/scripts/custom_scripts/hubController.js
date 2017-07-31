var hub = $.connection.gameHub;
hub.client.gameStarted = function (message) {
    $(".guessResult").text(message);
    $(".winForm").hide();
};
hub.client.gameFinished = function (message) {
    $(".guessResult").text(message);
    $("#number").val("");
    GetLog();
};
$.connection.hub.start();