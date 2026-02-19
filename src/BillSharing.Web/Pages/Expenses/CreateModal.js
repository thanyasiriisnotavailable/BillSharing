var abp = abp || {};
(function () {

    abp.modals.ExpenseCreateModal = function () {
        var modalManager;
        var $container;
        var l = abp.localization.getResource('BillSharing');

        this.initModal = function (manager) {
            modalManager = manager;
            var $modal = modalManager.getModal();
            $container = $modal.find('#items-container');

            // ADD INITIAL ITEM
            if ($container.children('.item-row').length === 0) {
                addItem();
            }

            // ADD BUTTON
            $modal.on('click', '#AddItemButton', function (e) {
                e.preventDefault();
                addItem();
            });

            // REMOVE BUTTON
            $container.on('click', '.remove-item-button', function (e) {
                e.preventDefault();

                if ($container.children('.item-row').length > 1) {
                    $(this).closest('.item-row').remove();
                    reIndexItems();
                } else {
                    abp.notify.warn(l('Expense:AtLeastOneItemRequired'));
                }
            });
        };

        function addItem() {
            let currentIndex = $container.children('.item-row').length;
            let templateHtml = $('#item-template').html();
            let renderedHtml = templateHtml.replace(/{index}/g, currentIndex);
            $container.append(renderedHtml);
        }

        function reIndexItems() {
            $container.children('.item-row').each(function (index, element) {
                var $row = $(element);

                $row.find('input[name*="Expense.Items"]').each(function () {
                    var name = $(this).attr('name');
                    $(this).attr('name', name.replace(/\[\d+\]/, '[' + index + ']'));
                });

                $row.find('input[type="checkbox"]').each(function () {
                    var name = $(this).attr('name');
                    $(this).attr('name', name.replace(/\[\d+\]/, '[' + index + ']'));

                    var val = $(this).val();
                    var newId = 'user_' + index + '_' + val;
                    $(this).attr('id', newId);
                    $(this).next('label').attr('for', newId);
                });
            });
        }
    };

})();
