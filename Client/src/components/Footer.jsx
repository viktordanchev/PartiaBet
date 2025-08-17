import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSquareFacebook, faInstagram, faTiktok } from '@fortawesome/free-brands-svg-icons';

const Footer = () => {
    return (
        <footer className="w-full bg-gray-900 p-6 text-gray-300">
            <div className="flex flex-row justify-evenly">
                <div>
                    <p className="font-bold text-xl text-maincolor mb-3 underline underline-offset-4">Contact us</p>
                    <ul>
                        <li>+359888888888</li>
                        <li>Sofia, str. "Dimitar Stamov" 18</li>
                        <li>info@partiabet.com</li>
                    </ul>
                </div>
                <div>
                    <p className="font-bold text-xl mb-3 text-center text-maincolor underline underline-offset-4">Socials</p>
                    <ul className="text-3xl flex felx-row space-x-4">
                        <li><FontAwesomeIcon icon={faSquareFacebook}/></li>
                        <li><FontAwesomeIcon icon={faInstagram}/></li>
                        <li><FontAwesomeIcon icon={faTiktok}/></li>
                    </ul>
                </div>
            </div>
            <div className="text-center mt-8">
                &copy; 2025 PartiaBet. All rights reserved.
            </div>
        </footer>
    );
};

export default Footer;