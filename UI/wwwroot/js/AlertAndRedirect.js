function refuseArticle(id) {
    let confirmAction = confirm("Makale Talebini Reddetmek İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Admin/ArticleSetStatusRefuse?id=${id}`).then((res) => {
            location.reload()
        })
    }
}

function confirmArticle(id) {
    let confirmAction = confirm("Makale Talebini Onaylamak İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Admin/ArticleSetStatusConfirm?id=${id}`).then(function (res) {
            location.reload()
        })
    }
}



function SetArticlePassive(id) {
    let confirmAction = confirm("Makaleyi kaldırmak İstediğinizden Emin Misiniz?");
    if (confirmAction) {
        fetch(`/Admin/ArticleSetStatusPassive?id=${id}`).then((res) => {
            location.reload()
        })
    }
}
