$(function () {

    const l = abp.localization.getResource('BillSharing');

    // Navigate to details
    $(document).on('click', '.group-card', function (e) {

        const card = $(this);

        // Do not navigate if editing
        if (card.hasClass('editing')) {
            return;
        }

        if ($(e.target).closest('button, input, textarea, .edit-actions').length) {
            return;
        }

        const url = $(this).data('url');
        if (!url) return;

        window.location.href = url;
    });

    // Copy invite code
    $('.copy-btn').on('click', async function (e) {
        e.stopPropagation();

        const code = $(this).data('code');
        if (!code) return;

        try {
            await navigator.clipboard.writeText(code);
            abp.notify.success(l('InviteCodeCopied'));
        } catch {
            abp.notify.error(l('CopyFailed'));
        }
    });

    // Share invite
    $('.share-btn').on('click', async function (e) {
        e.stopPropagation();

        const code = $(this).data('code');
        const groupName = $(this).data('group');
        if (!code || !groupName) return;

        const shareText = l('ShareGroupMessage', groupName, code);

        try {
            if (navigator.share) {
                await navigator.share({
                    title: groupName,
                    text: shareText
                });
            } else {
                await navigator.clipboard.writeText(shareText);
                abp.notify.info(l('ShareNotSupportedCopied'));
            }
        } catch {
            abp.notify.error(l('ShareFailed'));
        }
    });

    // Create Group
    $('#CreateGroupModal .btn-primary').click(function (e) {
        e.preventDefault();

        const form = $('#CreateGroupForm');

        if (!form.valid()) return;

        const input = {
            name: form.find('[name="Name"]').val(),
            description: form.find('[name="Description"]').val()
        };

        abp.ajax({
            url: '/api/app/group',
            type: 'POST',
            data: JSON.stringify(input)
        }).done(function () {

            abp.notify.success(l('Group:CreatedSuccessfully'));

            const modal = $('#CreateGroupModal');

            modal.one('hidden.bs.modal', function () {
                location.reload();
            });

            modal.modal('hide');

        }).fail(function (error) {

            abp.notify.error(error.responseJSON?.error?.message || 'Error');

        });
    });

    // Enable edit mode
    $(document).on('click', '.edit-btn', function (e) {
        e.stopPropagation();

        const btn = $(this);
        const id = btn.data('id');
        const card = btn.closest('.card');
        const mode = btn.data('mode');

        const nameElement = card.find('.group-name');
        const descElement = card.find('.group-description');

        // Edit Mode
        if (mode === "edit") {

            card.addClass('editing');

            const currentName = nameElement.text().trim();
            const currentDesc = descElement.text().trim();

            card.data('original-name', currentName);
            card.data('original-desc', currentDesc);

            nameElement.html(`<input type="text" class="form-control form-control-sm edit-name" value="${currentName}" />`);
            descElement.html(`<textarea class="form-control form-control-sm edit-description">${currentDesc}</textarea>`);

            btn
                .data('mode', 'save')
                .removeClass('btn-light')
                .addClass('btn-success')
                .html('<i class="fa fa-check"></i>');

            btn.after(`
                <button class="btn btn-sm btn-secondary cancel-edit-btn">
                    <i class="fa fa-times"></i>
                </button>
            `);


            return;
        }

        // Save Mode
        if (mode === "save") {

            const newName = card.find('.edit-name').val();
            const newDesc = card.find('.edit-description').val();

            abp.ajax({
                url: `/api/app/group/${id}`,
                type: 'PUT',
                data: JSON.stringify({
                    name: newName,
                    description: newDesc
                })
            }).done(function () {

                nameElement.html(newName);
                descElement.html(newDesc);

                card.removeClass('editing');

                btn
                    .data('mode', 'edit')
                    .removeClass('btn-success')
                    .addClass('btn-light')
                    .html('<i class="fa fa-pen"></i>');

                abp.notify.success("Group updated successfully");

            }).fail(function (error) {
                abp.notify.error(error.responseJSON?.error?.message || 'Error');
            });
        }
    });

    // Cancel edit
    $(document).on('click', '.cancel-edit-btn', function (e) {

        e.stopPropagation();

        const card = $(this).closest('.card');
        const btn = card.find('.edit-btn');

        const originalName = card.data('original-name');
        const originalDesc = card.data('original-desc');

        card.find('.group-name').html(originalName);
        card.find('.group-description').html(originalDesc);

        card.removeClass('editing');

        btn
            .data('mode', 'edit')
            .removeClass('btn-success')
            .addClass('btn-light')
            .html('<i class="fa fa-pen"></i>');

        $(this).remove();
    });
});
