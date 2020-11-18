function addItem(rootElement, value) {
    rootElement.innerHTML += '<input value="' + value.replace('"', '\\"') + '" name="' + rootElement.id + '">';
}

function removeItem(rootElement, value) {
    for (let item = 0; item < rootElement.childNodes.length; item++) {
        const element = rootElement.childNodes[item];
        if (element.tagName === "INPUT" && element.value === value.replace('"', '\\"')) {
            rootElement.removeChild(element);
            return;
        }
        else if (element.childElementCount > 0) {
            removeItem(element, value);
        }
    }
}
/**
 * Get list of the values of input elements that are children of the root element supplied
 * @param {HTMLElement} rootElement Element to start processed
 */
function getChildInputElements(rootElement) {
    let elements = [];
    for (let item = 0; item < rootElement.childNodes.length; item++) {
        const element = rootElement.childNodes[item];
        if (element.tagName === "INPUT") {
            elements.push(element.value);
        }
        else if (element.childElementCount > 0) {
            elements = elements.concat(getChildInputElements(element));
        }
    }
    return elements;
}

function populateModal(values, rootElement) {
    let modal = document.getElementById("items");
    modal.innerHTML = "";
    for (let index = 0; index < values.length; index++) {
        modal.innerHTML += '<div class="input-group">' +
        '<input readonly type="text" class="form-control rounded-right" value="' + values[index] + '">' +
        '<div class="input-group-append">' +
        '<button class="btn btn-danger" onclick="removeItem(document.getElementById(\'' + rootElement.id + '\'),\'' + values[index] + 
        '\');populateModal(getChildInputElements(document.getElementById(\'' + rootElement.id + '\')), document.getElementById(\'' + rootElement.id + '\'))' +'">X</button>' +
        '</div>' +
        '</div><br>';
    }
    let addItemForm = document.getElementById("addItemForm");
    addItemForm.removeEventListener("submit", (event) =>{return false;})
    addItemForm.addEventListener("submit", (event) =>{
        addItem(rootElement, document.getElementById("addItemInput").value);
        populateModal(getChildInputElements(document.getElementById(rootElement.id)), document.getElementById(rootElement.id));
        return false;
    })
    
}