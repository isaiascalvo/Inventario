export function fieldStateValidation(field: unknown) {
    if (field === undefined) return false;

    switch (typeof field) {
        case "number":
            return field >= 0;
        case "string":
            return field && field.length > 0 ? true : false;
        case "object":
            return field instanceof Date;
        case "boolean":
            return true;
        default:
            return false;
    }
}

export function formValidation(obj: never): string {
    let fieldList = "";
    Object.keys(obj).forEach(key => {
        if (key != "id" && !fieldStateValidation(obj[key])) {
            fieldList += key + ";";
        }
    });

    return fieldList;
}
