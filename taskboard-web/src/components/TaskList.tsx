import type { TaskResponse } from "../api/types";
import type { TaskAction } from "../domain/taskActions";
import { TaskCard } from "./TaskCard";

type TaskListProps = {
    tasks: TaskResponse[];
    onAction: (taskId: string, action: TaskAction) => void;
    emptyText?: string;
};

export function TaskList({ tasks, onAction, emptyText = "Brak zadan"}: TaskListProps) {
    if (tasks.length == 0) {
        return (<p className="text-gray-500">{(tasks.length == 0 && emptyText !== null) && emptyText}</p>);
    }
    return (
        <ul className="grid gap-2">
            {tasks.map(t => <TaskCard key={t.id} onAction={onAction} task={t} />)}
        </ul>
    );
}