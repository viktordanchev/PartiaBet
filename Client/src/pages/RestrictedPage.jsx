import React from 'react';
import AccessDenied from '../assets/images/access-denied.png';

function RestrictedPage() {
    return (
        <section className="flex-1 flex justify-center items-center">
            <img src={AccessDenied} />
        </section>
    );
}

export default RestrictedPage;