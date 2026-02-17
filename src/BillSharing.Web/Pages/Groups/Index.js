$(function () {
    const l = abp.localization.getResource('BillSharing');

    $('.group-card').on('click', function (e) {
        const url = $(this).data('url');
        if (!url) return;

        e.preventDefault();
        window.location.href = url;
    });

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
});
