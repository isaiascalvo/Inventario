import { JwtResult } from "@/models/JwtResult";

/* eslint-disable no-useless-escape */
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
  const reg = new RegExp(
    /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/
  );
  Object.keys(obj).forEach(key => {
    if (
      key != "id" &&
      (!fieldStateValidation(obj[key]) ||
        (key === "mail" && !reg.test(obj[key])))
    ) {
      fieldList += key + ";";
    }
  });

  return fieldList;
}

export function getCurrentUser(): JwtResult | null {
  const cu = sessionStorage.getItem("currentUser");
  if (cu) {
    return JSON.parse(cu);
  } else {
    return null;
  }
}
