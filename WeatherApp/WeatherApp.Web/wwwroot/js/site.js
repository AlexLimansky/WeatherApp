$(document).ready(function () {
    $('#myModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);//Button which is clicked
        var clickedButtonId = button.data('id');//Get id of the button
        // set id to the hidden input field in the form.
        $("#CityName").text(clickedButtonId);
        $("#city").val(clickedButtonId);
    });
});
