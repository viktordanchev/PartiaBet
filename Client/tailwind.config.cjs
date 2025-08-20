/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
    plugins: [
        require('tailwind-scrollbar'),
        require('tailwind-scrollbar-hide')
    ]
}