function setDeleteCommentValues(id) {
    $("#delete-id").val(id);
}

function deleteComment(url) {
    var id = $("#delete-id").val();
    ajaxQuery(id, url);
}

$('.upload-btn').on('click', function () {
    $('.upload-form').toggle();
});