import React from 'react';
import QuestionMark from '../../assets/images/profile-photo.jpg';

const Matches = () => {
    return (
        <article className="col-span-2 bg-gray-800 p-3 rounded text-gray-300 border border-gray-500 shadow-xl shadow-gray-900 flex flex-col space-y-6">
            <h2 className="text-2xl font-bold text-center">
                Open Tables
            </h2>
            <section className="flex justify-between">
                <article className="w-1/2 flex flex-col gap-6">
                    <h3 className="text-xl text-center">Play with bets</h3>
                    <ul className="grid grid-cols-2 gap-3">
                        <li className="p-2 flex flex-col justify-between items-center rounded-xl shadow-md shadow-gray-900 border border-gray-700 bg-gray-900">
                            <div className="w-full flex justify-evenly items-center">
                                <div className="flex flex-col items-center">
                                    <img src="" className="rounded-lg border border-gray-500 h-15 w-15" />
                                    <p className="font-semibold">1234567890123456</p>
                                    <p>Raiting: 2789</p>
                                </div>
                                <p className="text-2xl font-semibold">VS</p>
                                <div className="flex flex-col items-center">
                                    <img src={QuestionMark} className="rounded-lg border border-gray-500 h-15 w-15" />
                                    <p className="font-semibold">123456</p>
                                    <p>Raiting: 2789</p>
                                </div>
                            </div>
                            <button className="py-1 px-3 rounded-xl bg-green-600 text-white font-medium shadow-md transform transition-all duration-300 ease-in-out hover:cursor-pointer hover:scale-105">
                                Bet $20
                            </button>
                        </li>
                        <li className="h-15 rounded-xl shadow-md shadow-gray-900 border border-gray-700 bg-gray-900">
                        </li>
                        <li className="h-15 rounded-xl shadow-md shadow-gray-900 border border-gray-700 bg-gray-900">
                        </li>
                        <li className="h-15 rounded-xl shadow-md shadow-gray-900 border border-gray-700 bg-gray-900">
                        </li>
                        <li className="h-15 rounded-xl shadow-md shadow-gray-900 border border-gray-700 bg-gray-900">
                        </li>
                    </ul>
                </article>
                <div class="border-l border-gray-500 mx-6"></div>
                <article className="w-1/2">
                    <h3 className="text-xl text-center">Play for fun</h3>
                </article>
            </section>
        </article>
    );
};

export default Matches;