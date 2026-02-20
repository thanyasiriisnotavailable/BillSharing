var tooltipTriggerList = [].slice.call(document.querySelectorAll('[title]'))
tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})