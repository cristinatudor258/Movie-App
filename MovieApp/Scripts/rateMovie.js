function sendReview(id, movieTitle) {
    var rating = $('#opinionArea').val();
    $.ajax({
        url: id + "/RateMovie?title=" + movieTitle+"&rating=" + rating,
        contentType: 'application/json',
        type: 'POST',
        cache: false,
        success: function (data) {
            alert("Thank you for review!")
        },
        error: function (xhr) {
            alert("Please try again later!");
        }
    });
}
