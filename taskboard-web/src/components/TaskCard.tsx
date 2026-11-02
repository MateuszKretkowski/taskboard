import type { TaskResponse } from "../api/types"
import { StatusBadge } from "./StatusBadge";
import { formatDateTime } from "../format";

type TaskCardProps = {
    task: TaskResponse
}

export function TaskCard({ task }: TaskCardProps) {
    return (
        <li className="rounded-lg border border-gray-200 bg-white px-3 py-2">
            <div className="flex items-center justify-between gap-3">
                <span className="font-semibold">
                    {task.title}
                </span>
                <StatusBadge status={task.status} />
                <small>
                    Utworzone: {formatDateTime(task.createdAt)}
                    {task.completedAt !== null && <> Ukonczone: {formatDateTime(task.completedAt)}</>}
                </small>
            </div>
        </li>
    );
}