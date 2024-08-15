import * as Yup from "yup";

export const ConfirmPasswordValidator = Yup.string()
  .oneOf(
    [Yup.ref("password"), Yup.ref("newPassword"), null],
    "Password must match"
  )
  .required("Confirm Password is required");

const phoneRegExp =
  /^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$/;

export const PasswordValidator = Yup.string()
  .matches(
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})/,
    "Must Contain at least 8 Characters, One Uppercase, One Lowercase, One Number and One Special Case Character"
  )
  .required("Password is required");

export const phoneNumberValidator = Yup.string()
  .matches(phoneRegExp, "Phone number is not valid")
  .required("Phone Number is Required");

export const EmailValidator = Yup.string()
  .email("Email is invalid")
  .required("Email is Required");

export const nameValidator = (name = "") =>
  Yup.string().required(name ? `${name} is required` : "Required");

export const GeneralValidator = Yup.string().required("Required");

export const NumberValidator = Yup.number().required("Required");
