// ==========================================
// AJAX API Caller for Web Services
// ==========================================

// ==========================================
// MySQL Web Service
// ==========================================
function PageMethod(fn, paramArray, successFn, errorFn, asyncFn) {
    var pagePath = "http://localhost:63181/WebService.asmx";

    // Call the page method
    $.ajax({
        type: "POST",
        url: pagePath + "/" + fn,
        contentType: "application/json; charset=utf-8",
        data: paramArray,
        dataType: "json",
        success: successFn,
        error: errorFn,
        async: asyncFn
    });
}

// ==========================================
// MSSQL Web Service
// ==========================================
function PageMethodMSSQL(fn, paramArray, successFn, errorFn, asyncFn) {
    var pagePath = "http://localhost:64690/WebServiceMSSQL.asmx";

    // Call the page method
    $.ajax({
        type: "POST",
        url: pagePath + "/" + fn,
        contentType: "application/json; charset=utf-8",
        data: paramArray,
        dataType: "text",
        success: function (data) {
            try {
                // Parse the JSON response directly
                var jsonData = JSON.parse(data);
                successFn(jsonData);
            } catch (e) {
                console.error("JSON Parse Error:", e);
                console.error("Response data:", data);
                errorFn(null, "parse_error", e.message);
            }
        },
        error: errorFn,
        async: asyncFn
    });
}