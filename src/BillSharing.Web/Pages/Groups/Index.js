$(function () {

    const l = abp.localization.getResource('BillSharing');

    // Navigate to details
    $('.group-card').on('click', function (e) {
        const url = $(this).data('url');
        if (!url) return;

        e.preventDefault();
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
});
