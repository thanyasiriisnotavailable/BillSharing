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

        createExpenseModal.open({
            groupId: groupId
        });
    });


    $('.expense-paid-checkbox').change(function () {

        var checkbox = $(this);
        var expenseId = checkbox.data('expense-id');
        var isChecked = checkbox.is(':checked');

        var confirmTitle = isChecked
            ? abp.localization.localize('Expense:ConfirmMarkPaidTitle', 'BillSharing')
            : abp.localization.localize('Expense:ConfirmUnpaidTitle', 'BillSharing');

        var confirmMessage = isChecked
            ? abp.localization.localize('Expense:ConfirmMarkPaid', 'BillSharing')
            : abp.localization.localize('Expense:ConfirmUnpaid', 'BillSharing');

        var modal = new bootstrap.Modal(document.getElementById('paymentConfirmModal'));

        $('#paymentConfirmTitle').text(confirmTitle);
        $('#paymentConfirmMessage').text(confirmMessage);

        var confirmBtn = $('#paymentConfirmBtn');

        confirmBtn
            .removeClass('btn-success btn-warning btn-primary')
            .addClass('btn-primary')
            .text(abp.localization.localize('Confirm', 'BillSharing')
            );

        modal.show();

        confirmBtn.off('click').on('click', function () {

            checkbox.prop('disabled', true);

            billSharing.expenses.expense
                .setMyPaymentStatus(expenseId, isChecked)
                .then(function () {

                    modal.hide();

                    abp.notify.success(
                        abp.localization.localize('Expense:PaymentStatusUpdated', 'BillSharing')
                    );

                    setTimeout(function () {
                        window.location.reload();
                    }, 500);
                })
                .catch(function () {
                    checkbox.prop('checked', !isChecked);
                })
                .always(function () {
                    checkbox.prop('disabled', false);
                });

        });

        // If modal closed without confirming → revert toggle
        $('#paymentConfirmModal').off('hidden.bs.modal').on('hidden.bs.modal', function () {
            checkbox.prop('checked', !isChecked);
        });

    });
});
