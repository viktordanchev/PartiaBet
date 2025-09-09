import * as Yup from "yup";
import { authErrors, requiredField } from "./errorMessages";

const isProduction = import.meta.env.VITE_API_URL === 'production';

export const loginSchema = Yup.object({
    email: Yup.string()
        .email()
        .required('Email' + requiredField),
    password: Yup.string()
        .required('Password' + requiredField)
});

export const registerSchema = Yup.object({
    username: Yup.string()
        .required('Username' + requiredField),
    email: Yup.string()
        .email(authErrors.InvalidEmail)
        .required('Email' + requiredField),
    vrfCode: Yup.string().test(
        'vrf-required',
        'Verification code' + requiredField,
        value => !isProduction || !!value
    ),
    password: Yup.string()
        .min(6, authErrors.InvalidPass)
        .required('Password' + requiredField),
    confirmPassword: Yup.string()
        .required('Confirm password' + requiredField)
        .oneOf([Yup.ref('password'), null], authErrors.PassMatch)
});