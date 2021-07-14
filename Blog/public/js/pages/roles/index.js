function setDeleteRoleValues(id) {
    $("#delete-id").val(id);
}

function deleteRole(url) {
    var id = $("#delete-id").val();
    ajaxQuery(id, url);
}

$("select").change(function() {
    $(this).next("button").show();
});

$('.upload-btn').on('click', function() {
    $('.upload-form').toggle();
});

function cheked(select) {
    // берём значение из select и что-то с ним делаем
    return select.val();
};

$("#search-btn").click(function() {
    var search = $("#search").val();
    location.href = '@Url.Action("Index", "Posts")?search=' + search;
});