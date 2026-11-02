import type { ReactNode } from "react";

type PanelProps = {
    title: string;
    children: ReactNode;
};

export function Panel({ title, children }: PanelProps) {
    return (
        <div className="mb-4 rounded-xl border border-gray-200 bg-white px-5 py-4">
            <h2>{title}</h2>
            {children}
        </div>
    );
}