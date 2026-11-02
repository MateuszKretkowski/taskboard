import type { TaskResponse } from "../api/types";
import { TaskCard } from "./TaskCard";

type TaskListProps = {
    tasks: TaskResponse[];
    emptyText?: string;
};

export function TaskList({ tasks, emptyText }: TaskListProps) {
    if (tasks.length == 0) {
        return (<p className="text-gray-500">{(tasks.length == 0 && emptyText !== null) && emptyText}</p>);
    }
    return (
        <ul className="grid gap-2">
            {tasks.map(t => <TaskCard key={t.id} task={t} />)}
        </ul>
    );
}