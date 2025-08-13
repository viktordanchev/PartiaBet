import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiRequest from '../servives/apiRequest';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-regular-svg-icons';
import { useLoading } from '../contexts/LoadingContext';

function RegisterPage() {
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
        <section className="w-90 mx-auto p-8 border border-maincolor bg-gray-900 rounded-xl shadow-lg shadow-gray-900 space-y-6">
            <p className="text-3xl text-center text-maincolor">Welocme</p>
            <Formik
                initialValues={{ username: '', password: '', rememberMe: false }}
                onSubmit={handleLogin}
            >
                <Form className="flex flex-col text-maincolor">
                    <div>
                        <Field
                            className="w-full py-1 px-2 border-b border-maincolor mb-6 focus:border-white focus:outline-none"
                            placeholder="Email"
                            type="email"
                            name="email"
                        />
                        <ErrorMessage name="email" component="div" className="text-red-500" />
                    </div>
                    <div>
                        <Field
                            className="w-full py-1 px-2 border-b border-maincolor mb-6 focus:border-white focus:outline-none"
                            placeholder="Username"
                            type="text"
                            name="username"
                        />
                        <ErrorMessage name="email" component="div" className="text-red-500" />
                    </div>
                    <div>
                        <div className="relative mb-6">
                            <Field
                                className="w-full py-1 px-2 border-b border-maincolor focus:border-white focus:outline-none pr-8"
                                placeholder="Password"
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
                    <div>
                        <div className="relative">
                            <Field
                                className="w-full py-1 px-2 border-b border-maincolor focus:border-white focus:outline-none pr-8"
                                placeholder="Password"
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
                    <div className="my-4 flex items-center before:mt-0.5 before:flex-1 before:border-t before:border-neutral-300 after:mt-0.5 after:flex-1 after:border-t after:border-neutral-300 dark:before:border-neutral-500 dark:after:border-neutral-500">
                        <p className="mx-4 mb-0 text-center font-semibold dark:text-white">or</p>
                    </div>
                    <button className="w-full font-medium rounded py-2 bg-white text-gray-800 flex items-center justify-center space-x-2 hover:cursor-pointer hover:bg-maincolor">
                        <svg className="w-5" viewBox="0 0 533.5 544.3">
                            <path
                                d="M533.5 278.4c0-18.5-1.5-37.1-4.7-55.3H272.1v104.8h147c-6.1 33.8-25.7 63.7-54.4 82.7v68h87.7c51.5-47.4 81.1-117.4 81.1-200.2z"
                                fill="#4285f4" />
                            <path
                                d="M272.1 544.3c73.4 0 135.3-24.1 180.4-65.7l-87.7-68c-24.4 16.6-55.9 26-92.6 26-71 0-131.2-47.9-152.8-112.3H28.9v70.1c46.2 91.9 140.3 149.9 243.2 149.9z"
                                fill="#34a853" />
                            <path
                                d="M119.3 324.3c-11.4-33.8-11.4-70.4 0-104.2V150H28.9c-38.6 76.9-38.6 167.5 0 244.4l90.4-70.1z"
                                fill="#fbbc04" />
                            <path
                                d="M272.1 107.7c38.8-.6 76.3 14 104.4 40.8l77.7-77.7C405 24.6 339.7-.8 272.1 0 169.2 0 75.1 58 28.9 150l90.4 70.1c21.5-64.5 81.8-112.4 152.8-112.4z"
                                fill="#ea4335" />
                        </svg>
                        <span>Sign up with Google</span>
                    </button>
                    <div className="text-center mt-6">
                        <button
                            className="bg-maincolor text-gray-800 font-medium py-2 px-4 rounded hover:bg-[#81e4dc] hover:cursor-pointer"
                            type="submit">
                            Sign Up
                        </button>
                    </div>
                </Form>
            </Formik>
        </section>
    );
}

export default RegisterPage;