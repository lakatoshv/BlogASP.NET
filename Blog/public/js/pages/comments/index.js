function setDeleteCommentValues(id) {
    $("#delete-id").val(id);
}

function deleteComment(url) {
    var id = $("#delete-id").val();
    ajaxQuery(id, url);
}