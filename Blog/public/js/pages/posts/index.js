function setDeletePostValues(id) {
    $("#delete-id").val(id);
}

function deletePost(url) {
    var id = $("#delete-id").val();
    ajaxQuery(id, url);
}

$("select").change(function() {
    $(this).next("button").show();
});

function changeStatus(id, url) {
    var value = $("#status-" + id).val();
    url += "/" + id + "?status=" + value;
    ajaxQuery('', url);
}

$('.upload-btn').on('click', function() {
    $('.upload-form').toggle();
});

function cheked(select) {
    // берём значение из select и что-то с ним делаем
    return select.val();
};

/* сортування */
$("#sorting").click(function() {
    var valueSort = cheked($("#sort-by"));
    var typeSort = cheked($("#order-by"));
    document.cookie = "sort=" + valueSort;
    document.cookie = "type_sort=" + typeSort;
    location.href = '@Url.Action("MyPosts", "Posts")?sortBy=' + valueSort + '&orderBy=' + typeSort;
});

$("#search-btn").click(function() {
    var search = $("#search").val();
    location.href = '@Url.Action("Index", "Posts")?search=' + search;
});