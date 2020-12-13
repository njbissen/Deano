
function loadCreateAccount() {
    Layout.RefreshPanel('/Users/CreateAccount', Layout.WalleyesPanel);
}

function loadForgotPassword() {
    $('#Username').val('');
    $('#Password').val('');
    Layout.SwapPanelForward('/Users/ForgotPassword', Layout.WalleyesPanel);
}

function unloadForgotPassword() {
    Layout.SwapPanelBack(Layout.WalleyesPanel);
}

function unloadAccessDenied() {
    $(Layout.AccountContentPanel).show();
    $(Layout.EditPanel).hide();
}

function loadAccessDenied() {
    $(Layout.AccountContentPanel).hide();
    $(Layout.EditPanel).show();
    Layout.RefreshPanel('/Users/AccessDenied', Layout.EditPanel);
    Layout.CurrentPanelContent = $(Layout.AccountContentPanel).html();

    //Layout.SwapPanelForward('/Users/AccessDenied', Layout.AccountContentPanel);
}

function loadFishReports() {
    Layout.LoadContentPanel('/Home/FishReports');
}

function loadRates() {
    Layout.LoadContentPanel('/Home/Rates');
}

function loadDeano() {
    Layout.LoadContentPanel('/Home/Deano');
}

function loadContactUs() {
    Layout.LoadContentPanel('/Home/ContactUs');
}

function loadLocalEvents() {
    Layout.LoadContentPanel('/Home/LocalEvents');
}

function loadSlideshow() {
    Layout.LoadContentPanel('/Home/Slideshow');
}

function loadSponsors() {
    Layout.LoadContentPanel('/Home/Sponsors');
}

function loadTestimonials() {
    Layout.LoadContentPanel('/Home/Testimonials');
}

function loadDirections() {
    Layout.LoadContentPanel('/Home/Directions');
}

function loadMemberIndex() {
    window.location = '/Accounts/Member/Index';
    //Layout.RefreshPanel('Accounts/Member/Index', Layout.ContentPanel);
}

function loadMemberHome() {
    Layout.RefreshPanel('/Accounts/Member/Home', Layout.AccountContentPanel);
}

function loadMemberAccount() {
    Layout.RefreshPanel('/Accounts/Member/Account', Layout.AccountContentPanel);
}

function unloadMemberAccount() {
    Layout.RefreshPanel('/Accounts/Member/Home', Layout.AccountContentPanel);
}

function loadMemberCredentials() {
    Layout.RefreshPanel('/Accounts/Member/Credentials', Layout.AccountContentPanel);
}

function unloadMemberCredentials() {
    Layout.RefreshPanel('/Accounts/Member/Home', Layout.AccountContentPanel);
}

function loadCreateFishReport(id, track) {
    if (track == true) {
        //Layout.SwapPanelForward('/FishReports/Details/' + id, Layout.AccountContentPanel);
        $(Layout.AccountContentPanel).hide();
        $(Layout.EditPanel).show();
        Layout.RefreshPanel('/FishReports/Details/' + id, Layout.EditPanel);
        Layout.CurrentPanelContent = $(Layout.AccountContentPanel).html();
    }
    else {
        Layout.RefreshPanel('/FishReports/Details/' + id, Layout.EditPanel);
        Layout.SetCurrentPanelContent('/Accounts/Member/Home', Layout.AccountContentPanel);
    }
}

function unloadCreateFishReport() {
    $(Layout.EditPanel).hide();
    $(Layout.AccountContentPanel).show();
    loadReports('recent');
//    if (threadId <= 0) {
//        Layout.SwapPanelBack(Layout.AccountContentPanel);
//    }
//    else {
//        loadPosts(threadId);
//    }
//    Layout.SwapPanelBack(Layout.AccountContentPanel);
}

function markReport(reportId, source) {
    var path = '/FishReports/SaveBookmark';
    var hdn = $(source).siblings('.marked');
    var marked = $(hdn).val();

    var values = { reportId: reportId,
        mark: marked
    };
    $.get(path, values);
    if (marked == false || marked == 'false') {
        source.innerHTML = 'Remove from Favorites';
        source.innerHTML = '';
        $(hdn).val('true');
    }
    else {
        source.innerHTML = 'Add to Favorites';
        $(hdn).val('false');
    }
}

function loadCreatePost(postId, threadId) {
    $(Layout.AccountContentPanel).hide();
    $(Layout.EditPanel).show();
    Layout.RefreshPanel('/Forums/PostDetails?threadId=' + threadId + '&postId=' + postId, Layout.EditPanel);
    Layout.CurrentPanelContent = $(Layout.AccountContentPanel).html();
    //Layout.SwapPanelForward('/Forums/PostDetails?threadId=' + threadId + '&postId=' + postId, Layout.AccountContentPanel);
}

function markPost(postId, source) {
    var path = '/Forums/SaveBookmark';
    var hdn = $(source).siblings('.marked');
    var marked = $(hdn).val();

    var values = { postId: postId,
        mark: marked
    };
    $.get(path, values);
    if (marked == false || marked == 'false') {
        source.innerHTML = 'Remove from Favorites';
        source.innerHTML = '';
        $(hdn).val('true');
    }
    else {
        source.innerHTML = 'Add to Favorites';
        $(hdn).val('false');
    }
}

function unloadCreatePost(threadId) {
    $(Layout.EditPanel).hide();
    $(Layout.AccountContentPanel).show();
    if (threadId <= 0) {
        Layout.SwapPanelBack(Layout.AccountContentPanel);
    }
    else {
        loadPosts(threadId);
    }
}

function loadCreateThread(threadId, forumId) {
    $(Layout.AccountContentPanel).hide();
    $(Layout.EditPanel).show();
    Layout.RefreshPanel('/Forums/ThreadDetails?forumId=' + forumId + '&threadId=' + threadId, Layout.EditPanel);
    Layout.CurrentPanelContent = $(Layout.AccountContentPanel).html();
    //Layout.SwapPanelForward('/Forums/ThreadDetails?forumId=' + forumId + '&threadId=' + threadId, Layout.AccountContentPanel);
}

function unloadCreateThread(forumId) {
    $(Layout.EditPanel).hide();
    $(Layout.AccountContentPanel).show();
    if (forumId <= 0) {
        Layout.SwapPanelBack(Layout.AccountContentPanel);
    }
    else {
        loadThreads(forumId);
    }
}

function unloadPosts(id) {
    Layout.RefreshPanel('/Forums/Threads?forumId=' + id, Layout.AccountContentPanel);
}

function loadCreateForum(id) {
    Layout.SwapPanelForward('/Forums/ForumDetails?forumId=' + id, Layout.AccountContentPanel);
}

function unloadCreateForum() {
    Layout.SwapPanelBack(Layout.AccountContentPanel);
}