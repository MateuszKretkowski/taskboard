import type { TaskStatus } from "../api/types"

export type StatusBadgeProps = {
    status: TaskStatus
}

const statusLabels : Record<TaskStatus, string> = {
    "Todo": "Do zrobienia",
    "InProgress": "W Toku",
    "Done": "Zrobione"
};

const statusColors: Record<TaskStatus, string> = {
  Todo: 'bg-gray-500',
  InProgress: 'bg-orange-600',
  Done: 'bg-green-700',
};

export function StatusBadge({status} : StatusBadgeProps) {
    return(
        <span className={`rounded-full px-2 py-0.5 text-xs font-semibold whitespace-nowrap text-white ${statusColors[status]}`}>
            {statusLabels[status]}
        </span>
    );
}