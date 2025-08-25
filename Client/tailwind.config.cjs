/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./src/**/*.{js,jsx,ts,tsx}",
    ],
    theme: {
        extend: {
            backgroundImage: {
                'bk': "url('/images/chess/black-king.svg')",
                'wp': "url('/images/chess/white-pawn.svg')",
            },
        },
    },
    plugins: [
        require('tailwind-scrollbar'),
        require('tailwind-scrollbar-hide')
    ]
}