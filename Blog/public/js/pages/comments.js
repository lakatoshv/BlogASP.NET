function setEditCommentValues(id) {
    var author = $("#author" + id).text();
    var comment = $("#content" + id).text();
    $("#edit-id").val(id);
    $("#edit-author").val(author);
    $("#edit-content").val(comment);
}
function setDeleteCommentValues(id) {
    $("#delete-comment-id").val(id);
    $("#delete-id").val(id);
}

function deleteComment(url) {
    var id = $("#delete-id").val();
    ajaxQuery(id, url);
}