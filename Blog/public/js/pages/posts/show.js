function setEditCommentValues(id) {
    var author = $("#author" + id).text();
    var comment = $("#content" + id).text();
    $("#edit-id").val(id);
    $("#edit-author").val(author);
    $("#edit-content").val(comment);
}
function setDeleteCommentValues(id) {
    $("#delete-comment-id").val(id);
}

function setDeletePostValues(id) {
    $("#delete-post-id").val(id);
}

function deletePost(url) {
    var id = $("#delete-post-id").val();
    ajaxQuery(id, url);
}

function deleteComment(url) {
    var id = $("#delete-comment-id").val();
    ajaxQuery(id, url);
}