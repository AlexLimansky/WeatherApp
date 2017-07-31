$(document).ready(function() {
    getName();
    $(".winForm").hide();
});

function getName() {
    $.ajax({
        url: url + 'account/getname',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data == null) {
                var path = url + "login.html";
                $(location).attr("href", path);
            }
            else {
                $(".nameLabel").text(data);
            }

        }
    });
};

$("#logout").click(function (event) {
    event.preventDefault();
    $.ajax({
        url: url + "account/logout",
        type: "POST",
        success: function (data) {
            var path = url + "login.html";
            $(location).attr("href", path);
        }
    });
});

$("#start").click(function (event) {
    event.preventDefault();
    var number = $("#number").val();
    $.ajax({
        url: url + 'game/start/' + number ,
        type: 'POST',
        success: function (data) {
            $("#number").val("");
        }
    });
});

$("#guess").click(function (event) {
    event.preventDefault();
    var number = $("#number").val();
    $.ajax({
        url: url + 'game/guess/' + number,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data != "Win")
            {
                $(".guessResult").text(data);
                $("#number").val("");
            }
        }
    });
});

function GetLog() {
    $.ajax({
        url: url + 'game/log',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            ShowLog(data);
        }
    });
}

function ShowLog(data) {
    var strResult = '<table class=' + '"table"' + "><tr><th>Started Game</th><th>Player Guessed</th><th>Number Offered</th>";
    strResult += "<th>Number Was</th><th>Date</th><th>Time</th></tr>";
    $.each(data, function (index, entry) {
        var time = new Date(entry.time);
        strResult += "<tr><td>" + entry.playerProposed + "</td><td> " + entry.playerGuessed + "</td><td>" +
            entry.yourNumber + "</td><td>" + entry.realNumber +
            "</td><td>" + time.getDate() + "-" + time.getMonth() + "-" + time.getFullYear() + "</td><td>" + time.getHours() + ":" + time.getMinutes() + "</td></tr>";
        });
        strResult += "</table>";
        $("#tableBlock").html(strResult);
        $(".winForm").show();
}