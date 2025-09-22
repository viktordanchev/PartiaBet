import React, { useEffect } from 'react';

function NotFoundPage() {
    useEffect(() => {
        document.title = '404 - Not Found';
    });

    return (
        <div className="flex-1 flex justify-center items-center">
            Not found
        </div>
    );
}

export default NotFoundPage;