function $axiosPost(url, data, params, showmask) {
    let loading = null;
    if (!(typeof (showmask) == "undefined") && !showmask) { } else {
        ///$('#Loading_dialog').modal('show');
    }
    //$('#Loading_dialog').modal('show');
    let header = { 'X-CSRF-TOKEN': `${$('input:hidden[name="__RequestVerificationToken"]').val()}` };
    // 不直接return的話 await 會失效
    return axios.post(url, data, { params: params, headers: header})
        .then(response => {
            //$('#Loading_dialog').modal('hide');
            if (loading!=null)
                loading.close();
            if (response.status === 200) {
                return response.data
            };
        })
        .catch(function (err) {
            //$('#Loading_dialog').modal('hide');
            if (loading != null)
                loading.close();
            alert(err);
        })
};


function $axiosGet(url, showmask) {
    let loading = null;
    if (!(typeof (showmask) == "undefined") && !showmask) { } else {
        //$('#Loading_dialog').modal('show');
        loading = ElementPlus.ElLoading.service({
            lock: true,
            text: 'Loading',
            background: 'rgba(0, 0, 0, 0.7)',
        })
    }

    //$('#Loading_dialog').modal('show');
    let header = { 'X-CSRF-TOKEN': `${$('input:hidden[name="__RequestVerificationToken"]').val()}` };
    // 不直接return的話 await 會失效
    return axios.get(url, { headers: header })
        .then(response => {
           // $('#Loading_dialog').modal('hide');
            if (loading != null)
                loading.close();
            if (response.status === 200) {
                return response.data
            };
        })
        .catch(function (err) {
            //$('#Loading_dialog').modal('hide');
            if (loading != null)
                loading.close();
            alert(err);
        })
};
// exportExcel
function $axiosExportExcelPost(url, data) {

    //$('#Exporting_dialog').modal('show');
    let loading = ElementPlus.ElLoading.service({
        lock: true,
        text: 'Loading',
        background: 'rgba(0, 0, 0, 0.7)',
    })
    let header = { 'X-CSRF-TOKEN': `${$('input:hidden[name="__RequestVerificationToken"]').val()}` };
    // 不直接return的話 await 會失效
    return axios.post(url, data, { headers: header })
        .then(response => {
            //$('#Exporting_dialog').modal('hide');
            if (loading != null)
                loading.close();
            if (response.status === 200) {
                return response.data
            };
        })
        .catch(function (err) {
            //$('#Exporting_dialog').modal('hide');
            if (loading != null)
                loading.close();
            alert(err);
        })
};