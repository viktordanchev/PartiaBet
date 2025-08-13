import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiRequest from '../servives/apiRequest';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { faEye, faEyeSlash } from '@fortawesome/free-regular-svg-icons';
import { useLoading } from '../contexts/LoadingContext';

function LoginPage() {
    const navigate = useNavigate();
    const { setIsLoading } = useLoading();
    const [showPassword, setShowPassword] = useState(false);

    const handleLogin = async (values) => {
        try {
            setIsLoading(true);

            const response = await apiRequest('account', 'login', values, undefined, 'POST', true);

            if (response.token) {
                navigate('/home');
            }
        } catch (error) {
            console.error(error);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <section className="w-80 border border-zinc-500 bg-maincolor rounded-xl shadow-2xl shadow-gray-400 px-8 py-8 sm:w-full">
            <p className="text-3xl text-center text-white">Login</p>
            <hr className="my-4" />
            <Formik
                initialValues={{ username: '', password: '', rememberMe: false }}
                onSubmit={handleLogin}
            >
                <Form className="flex flex-col space-y-2 text-gray-700">
                    <div>
                        <label className="font-medium">Email</label>
                        <Field
                            className="rounded w-full py-1 px-2 border-2 border-white focus:border-blue-500 focus:outline-none"
                            placeholder="user@mail.com"
                            type="text"
                            name="username"
                        />
                        <ErrorMessage name="email" component="div" className="text-red-500" />
                    </div>
                    <div>
                        <label className="font-medium">Password</label>
                        <div className="relative">
                            <Field
                                className="rounded w-full py-1 px-2 border-2 border-white focus:border-blue-500 focus:outline-none pr-8"
                                type={showPassword ? 'text' : 'password'}
                                name="password"
                            />
                            <FontAwesomeIcon
                                icon={showPassword ? faEye : faEyeSlash}
                                className="absolute right-2 top-1/2 transform -translate-y-1/2 cursor-pointer"
                                onClick={() => setShowPassword(!showPassword)}
                            />
                        </div>
                        <ErrorMessage name="password" component="div" className="text-red-500" />
                    </div>
                    <div className="flex items-center justify-between">
                        <label className="inline-flex items-center cursor-pointer">
                            <Field
                                type="checkbox"
                                name="rememberMe"
                                className="form-checkbox text-blue-600 cursor-pointer"
                            />
                            <span className="ml-1 text-white hover:text-gray-200">Remember me</span>
                        </label>
                        <a href="/account/recoverPassword" className="inline-block align-baseline text-sm text-blue-600 underline hover:text-blue-800">
                            Forgot Password?
                        </a>
                    </div>
                    <div className="text-center pt-6">
                        <button
                            className="bg-blue-500 border-2 border-blue-500 text-white font-medium py-1 px-2 rounded hover:bg-white hover:text-blue-500"
                            type="submit">
                            Sign In
                        </button>
                    </div>
                </Form>
            </Formik>
        </section>
    );
}

export default LoginPage;