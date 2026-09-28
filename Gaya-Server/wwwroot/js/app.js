const api = "/api/GayaProject";

const form = document.querySelector("#calculator");
const fieldA = document.querySelector("#field-a");
const fieldB = document.querySelector("#field-b");
const operatorSelect = document.querySelector("#operator");
const calculateButton = document.querySelector("#calculate");
const editButton = document.querySelector("#edit-operators");
const editPanel = document.querySelector("#edit-panel");
const operatorList = document.querySelector("#operator-list");
const doneButton = document.querySelector("#done-editing");
const output = document.querySelector("#output");
const currentResult = document.querySelector("#current-result");
const previousList = document.querySelector("#previous-calculations");
const monthlyCount = document.querySelector("#monthly-count");
const errorEl = document.querySelector("#error");

let editMode = false;
let activeOperators = [];

fieldA.addEventListener("input", updateCalculateButton);
fieldB.addEventListener("input", updateCalculateButton);
operatorSelect.addEventListener("change", updateCalculateButton);
form.addEventListener("submit", calculate);
editButton.addEventListener("click", openEditMode);
doneButton.addEventListener("click", closeEditMode);

loadActiveOperators().catch((error) => showError(error.message));

function updateCalculateButton() {
    const fieldsFilled = fieldA.value.trim() !== "" && fieldB.value.trim() !== "";
    const operatorSelected = operatorSelect.value !== "";
    const hasActiveOperators = activeOperators.length > 0;

    calculateButton.disabled = editMode
        || !fieldsFilled
        || !operatorSelected
        || !hasActiveOperators;
}

async function loadActiveOperators() {
    const selected = operatorSelect.value;
    const body = await request(`${api}/GetActiveOperators`);
    activeOperators = body.value ?? [];

    operatorSelect.replaceChildren(option("", "Select operator"));
    for (const name of activeOperators) {
        operatorSelect.append(option(name, name));
    }

    operatorSelect.value = activeOperators.includes(selected) ? selected : "";
    updateCalculateButton();
}

async function openEditMode() {
    editMode = true;
    editPanel.hidden = false;
    updateCalculateButton();
    clearError();

    try {
        await loadAllOperators();
    } catch (error) {
        showError(error.message);
    }
}

function closeEditMode() {
    editMode = false;
    editPanel.hidden = true;
    updateCalculateButton();
}

async function loadAllOperators() {
    const body = await request(`${api}/GetAllOperators`);
    operatorList.replaceChildren();

    for (const operator of body.value ?? []) {
        const name = document.createElement("span");
        name.className = "name";
        name.textContent = operator.name;

        const status = document.createElement("span");
        status.className = "status";
        status.textContent = operator.isActive ? "Active" : "Inactive";

        const toggle = document.createElement("button");
        toggle.type = "button";
        toggle.textContent = operator.isActive ? "Deactivate" : "Activate";
        toggle.addEventListener("click", () => toggleOperator(operator));

        const item = document.createElement("li");
        item.className = operator.isActive ? "active" : "inactive";
        item.append(name, status, toggle);
        operatorList.append(item);
    }
}

async function toggleOperator(operator) {
    clearError();
    const path = operator.isActive
        ? `${api}/DeleteOperator/${operator.id}`
        : `${api}/AddOperator/${operator.id}`;
    const method = operator.isActive ? "DELETE" : "PUT";

    try {
        await request(path, { method });
        await Promise.all([loadActiveOperators(), loadAllOperators()]);
    } catch (error) {
        showError(error.message);
    }
}

async function calculate(event) {
    event.preventDefault();
    clearError();

    try {
        const body = await request(`${api}/Calculate`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                operator: operatorSelect.value,
                fieldA: fieldA.value.trim(),
                fieldB: fieldB.value.trim(),
            }),
        });
        showCalculation(body.value);
    } catch (error) {
        showError(error.message);
    }
}

function showCalculation(value) {
    output.hidden = false;
    currentResult.textContent = value.result ?? "";

    const operations = value.lastOperations ?? [];
    if (operations.length === 0) {
        const empty = document.createElement("li");
        empty.textContent = "No previous calculations.";
        previousList.replaceChildren(empty);
    } else {
        previousList.replaceChildren(...operations.map((operation) => {
            const item = document.createElement("li");
            item.textContent = `${operation.fieldA} ${operation.operator} ${operation.fieldB} = ${operation.result}`;
            return item;
        }));
    }

    const count = value.monthlyOperationCount ?? 0;
    monthlyCount.textContent = `Used ${count} ${count === 1 ? "time" : "times"} this month.`;
}

async function request(url, options) {
    const response = await fetch(url, options);
    let body = null;

    try {
        body = await response.json();
    } catch {
        body = null;
    }

    if (!response.ok) {
        throw new Error(body?.message || "The request failed.");
    }

    return body;
}

function option(value, text) {
    const element = document.createElement("option");
    element.value = value;
    element.textContent = text;
    return element;
}

function showError(message) {
    errorEl.hidden = false;
    errorEl.textContent = message;
}

function clearError() {
    errorEl.hidden = true;
    errorEl.textContent = "";
}
