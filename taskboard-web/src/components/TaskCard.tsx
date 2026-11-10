import { StatusBadge } from "./StatusBadge";
import type { TaskResponse } from "../api/types"
import { formatDateTime } from "../format";
import { TaskActions } from "./TaskActions";
import type { TaskAction } from "../domain/taskActions";

type TaskCardProps = {
    task: TaskResponse,
    onAction: (taskId: string, action: TaskAction) => void
}

export function TaskCard({ task, onAction }: TaskCardProps) {
    return (
        <li className="rounded-lg border border-gray-200 bg-white px-3 py-2">
            <div className="flex items-center justify-between gap-3">
                <span className="font-semibold">
                    {task.title}
                </span>
                <StatusBadge status={task.status} />
            </div>
            <div className="flex items-center justify-between gap-3">
                <small>
                    Utworzone: {formatDateTime(task.createdAt)}
                    {task.completedAt !== null && <> Ukonczone: {formatDateTime(task.completedAt)}</>}
                </small>
                <TaskActions status={task.status} onAction={(action) => (onAction(task.id, action))} />
            </div>
        </li>
    );
}