$(function () {

    var createExpenseModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Expenses/CreateModal',
        modalClass: 'ExpenseCreateModal'
    });

    createExpenseModal.onResult(function () {
        location.reload();
    });

    $('#NewExpenseButton').on('click', function (e) {
        e.preventDefault();

        var groupId = $(this).attr('data-group-id');

        console.log("GroupId:", groupId); // DEBUG

        createExpenseModal.open({
            groupId: groupId
        });
    });

});
