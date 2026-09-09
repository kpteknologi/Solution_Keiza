// ==========================================
// AJAX API Caller for Web Services
// ==========================================

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
                // Try parsing the whole response first, then fall back to extracting the first valid JSON object
                var jsonData = safeParseJSON(data);
                successFn(jsonData);
            } catch (e) {
                console.error("JSON Parse Error:", e);
                console.error("Response data:", data);
                if (typeof errorFn === "function") {
                    errorFn(null, "parse_error", e.message);
                }
            }
        },
        error: errorFn,
        async: asyncFn
    });

    function safeParseJSON(text) {
        if (text === null || text === undefined) {
            return null;
        }

        text = String(text).trim();

        // Try direct parse
        try {
            return JSON.parse(text);
        } catch (fullParseError) {
            // Attempt to find the first valid JSON object by trying substrings that end at each '}'.
            // This handles cases where the server appends extra payloads (e.g. ...}{"d":null}).
            for (var i = 0; i < text.length; i++) {
                if (text.charAt(i) === '}') {
                    var candidate = text.substring(0, i + 1);
                    try {
                        var parsed = JSON.parse(candidate);
                        var trailing = text.substring(i + 1).trim();
                        if (trailing.length > 0) {
                            console.warn("safeParseJSON: trailing data ignored after first JSON object:", trailing);
                        } else {
                            console.info("safeParseJSON: parsed full response after trimming.");
                        }
                        return parsed;
                    } catch (partialError) {
                        // not a complete JSON yet; continue searching
                    }
                }
            }

            // No valid JSON found
            throw fullParseError;
        }
    }
}
