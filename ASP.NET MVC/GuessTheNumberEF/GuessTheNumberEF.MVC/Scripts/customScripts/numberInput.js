$(".game-btn").click(function (event) {
    event.preventDefault();
    var action = $(this).val();
    var number = $("#number").val();
    $.ajax({
        url: baseUrl + 'Game/SetNumber',
        data: {
            number: number,
            action: action
        },
        type: 'POST',
        success: function (data) {
            $("#result").text(data);
            $(".message").text("Game is runnung!");
        },
        error: function () {
            $("#result").text("No number was entered!");
        }
    });
});