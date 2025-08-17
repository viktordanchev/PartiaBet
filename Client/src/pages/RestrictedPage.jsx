import React from 'react';
import AccessDenied from '../assets/images/access-denied.png';

function RestrictedPage() {
    return (
        <section className="w-full flex justify-center items-center">
            <img src={AccessDenied} />
        </section>
    );
}

export default RestrictedPage;