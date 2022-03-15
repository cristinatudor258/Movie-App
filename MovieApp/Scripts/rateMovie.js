
    function getBaseUrl() {
        var pathArray = location.href.split('/');
        var protocol = pathArray[0];
        var host = pathArray[2];
        var url = protocol + '//' + host + '/';

        return url;
    }

    function sendReview(id, movieTitle) {
        var rating = $('#opinionArea').val();
        $.ajax({
            url: getBaseUrl()+"Home/RateMovie?id="+id+"&title=" + movieTitle + "&rating=" + rating,
            contentType: 'json',
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

function showRatingInput() {
    var reviewArea = document.getElementById("ratingZone");
    if (reviewArea.style.display === "none") {
        reviewArea.style.display = "block";
        } else {
        reviewArea.style.display = "none";
        }
}

