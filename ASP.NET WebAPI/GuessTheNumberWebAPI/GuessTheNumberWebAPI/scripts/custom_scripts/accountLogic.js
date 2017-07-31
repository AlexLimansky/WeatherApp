$("#login").click(function (event) {
    event.preventDefault();
    var name = $("#name").val();
    $.ajax({
        url: url + "account/login/" + name,
        type: "POST",
        dataType: "json",
        success: function (data) {
            var path = url;
            $(location).attr("href", path);
        },
        error: function () {
            $(".errorMessage").text("No name entered!!!");
            $("#name").text("");
        }
    });
});