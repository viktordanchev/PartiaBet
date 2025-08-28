import React from 'react';
import Board from './Board';

const ChessMatch = () => {
    return (
        <section className="flex gap-3">
            <Board />
            <article className="flex flex-col justify-between text-gray-300">
                <div className="space-y-3">
                    <div className="p-2 flex items-center gap-3 bg-gray-900 rounded border border-gray-500 shadow-xl shadow-gray-900">
                        <img src="" className="rounded-lg border border-gray-500 h-12 w-12" />
                        <div>
                            <p className="font-semibold">1234567890123456</p>
                            <p className="text-xs">Raiting: 2789</p>
                        </div>
                    </div>
                    <article className="h-fit w-fit bg-gray-300 p-2 text-3xl text-center font-semibold rounded border border-gray-500 shadow-xl shadow-gray-900">
                        <p className="text-gray-800">10:33</p>
                    </article>
                </div>
                <p className="text-2xl font-semibold">Total 40$</p>
                <div className="space-y-3">
                    <div className="p-2 flex items-center gap-3 bg-gray-900 rounded border border-gray-500 shadow-xl shadow-gray-900">
                        <img src="" className="rounded-lg border border-gray-500 h-12 w-12" />
                        <div>
                            <p className="font-semibold">1234567890123456</p>
                            <p className="text-xs">Raiting: 2789</p>
                        </div>
                    </div>
                    <article className="h-fit w-fit bg-gray-300 p-2 text-3xl text-center font-semibold rounded border border-gray-500 shadow-xl shadow-gray-900">
                        <p className="text-gray-800">10:33</p>
                    </article>
                </div>
            </article>
        </section>
    );
};

export default ChessMatch;