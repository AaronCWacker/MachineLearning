
class HtmlExtensions {
    constructor() {}

    // javascript include tag to allow html to be broken into multiple files.
    // sample use
    //    <div id='adt_inbnd_log_search' w3-include-html="DashboardDiv_AdtInboundLogSearch.html"></div>

    // this method should be executed at the end of the file when using w3-include-html attribute.
    static includeHTML() {
        let z, i, elmnt, file, xhttp;
        /* Loop through a collection of all HTML elements: */
        z = document.getElementsByTagName("*");
        for (i = 0; i < z.length; i++) {
            elmnt = z[i];
            /* search for elements with this attribute */
            file = elmnt.getAttribute("w3-include-html");
            if (file) {
                /* Make an HTTP request using the attribute value as the file name: */
                xhttp = new XMLHttpRequest();
                xhttp.onreadystatechange = function () {
                    if (this.readyState === 4) {
                        if (this.status === 200) {
                            elmnt.innerHTML = this.responseText;
                        }
                        if (this.status === 404) {
                            elmnt.innerHTML = "Page not found.";
                        }
                        /* Remove the attribute, and call this function once more: */
                        elmnt.removeAttribute("w3-include-html");
                        HtmlExtensions.includeHTML();
                    }
                };
                xhttp.open("GET", file, true);
                xhttp.send();
                return;
            }
        }
    }

    // allow a section to open and close as a collapsible event
    // sample
    //         <button class="collapsible">Raw Response</button>
    static addEventToClassCollapsible() {
        let coll = document.getElementsByClassName("collapsible");
        let i;

        for (i = 0; i < coll.length; i++) {
            coll[i].addEventListener("click", function () {
                this.classList.toggle("active");
                let content = this.nextElementSibling;
                if (content.style.display === "block") {
                    content.style.display = "none";
                } else {
                    content.style.display = "block";
                }
            });
        }

    }
}

class AxiosHelper {
    constructor(targetData, successFn) {
        this.resultElement = document.getElementById('callResult');
        this.resultData = document.getElementById(targetData || 'defaultData');
        this.resultData.innerHTML = 'Processing ...';
        this.resultElement.innerHTML = 'Waiting for response...';
        this.successFn = successFn
    }
    
    responseThen(response) {
        this.resultElement.innerHTML = '<h4>Result</h4>' +
            '<h5>Status:</h5> ' +
            '<pre>' + response.status + ' ' + response.statusText + '</pre>' +
            '<h5>Headers:</h5>' +
            '<pre>' + JSON.stringify(response.headers, null, '\t') + '</pre>' +
            '<h5>Data:</h5>' +
            '<pre>' + response.data + '</pre>';
        this.resultData.innerHTML = response.data;
        if (typeof this.successFn === 'function') {
            this.successFn(response.data);
        }
    }

    responseCatch(error) {
        this.resultData.innerHTML = 'Failed. ';
        this.resultElement.innerHTML = '<h3>' + error + '<h3>';
        this.resultElement.innerHTML += '<h4>Result</h4>' + '<h5>Message:</h5> ' + error.message;
        if (error.response) {
            this.resultElement.innerHTML += '<h5>Status:</h5> ' +
                '<pre>' + error.response.status + ' ' + error.response.statusText + '</pre>';
            this.resultElement.innerHTML += '<h5>Headers:</h5>' +
                '<pre>' + JSON.stringify(error.response.headers, null, '\t') + '</pre>';
            this.resultElement.innerHTML += '<h5>Data:</h5>' +
                '<pre>' + JSON.stringify(error.response.data, null, '\t') + '</pre>';
            this.resultData.innerHTML += '<pre>' + JSON.stringify(error.response.data, null, '\t') + '</pre>';
        } else {
            this.resultElement.innerHTML += '<h5>No error response.</h5> ';
            this.resultData.innerHTML += 'No error response returned.';
        }
    }
}
