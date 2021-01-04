function addSpinner() {
    var submit =
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>Loading...';
    $('#upload-file').prop('disabled', true).html(submit);
    $('.excel-file').hide();
    var spinner = '<div class="spinner-border text-primary" role="status" style="width: 4rem; height: 4rem;"><span class="sr-only">Loading...</span></div>';
    //$('table').append('<div class="overlay-spinner text-center">' + spinner + '</div>');
}

function ajaxQuery(id, url) {
    $.post(
        url + id,
        onAjaxSuccess
    );
}

function onAjaxSuccess() {
    location.reload();
}