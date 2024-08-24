import * as Yup from "yup";
import { toast } from "react-toastify";
import {
  ConfirmPasswordValidator,
  EmailValidator,
  nameValidator,
  PasswordValidator,
  phoneNumberValidator,
} from "./validationUtils";

export const getSignUpValidationSchema = () => {
  return Yup.object({
    firstName: nameValidator("First Name"),
    lastName: nameValidator("Last Name"),
    email: EmailValidator,
    phoneNumber: phoneNumberValidator,
    password: PasswordValidator,
    confirmPassword: ConfirmPasswordValidator,
  });
};

export const handleErrors = (error) => {
  if (error && error.errors) {
    error.errors.forEach((err) => {
      toast.error(err);
    });
  }
};

export const resetFormFields = (setters) => {
  setters.forEach((setter) => setter(""));
};

export const getSignValidationSchema = () => {
  return Yup.object({
    email: EmailValidator,
    password: PasswordValidator,
  });
};
export const emailValidationSchema = () => {
  return Yup.object({
    emailAddress: EmailValidator,
  });
};
