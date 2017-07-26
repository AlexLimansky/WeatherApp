$(".js-starter").click(function () {
    var withError = this.id == "btnGetDataWithError";
    $.ajax({
        url: baseUrl + '/Home/GetData?withError=' + withError,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            fillForms(data);
        },
        error: function () {
            alert("Error occured! No data received!");
        }
    });
});
var fillForms = function (data) {
    $("#id").text(data.Id);
    $("#name").text(data.Name);
    $("#age").text(data.Age);
    if (data.Sex == 0) {
        $("#male").attr("checked", "checked");
    }
    else {
        $("#female").attr("checked", "checked");
    }
}