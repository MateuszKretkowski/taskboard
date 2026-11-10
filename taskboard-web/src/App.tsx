import { useState } from "react";
import type { TaskResponse } from "./api/types";
import { Panel } from "./components/Panel";
import { TaskList } from "./components/TaskList";
import { sampleTasks } from "./data/sampleTasks";
import { applyActionLocally, createTaskLocally } from "./data/localTasks";
import type { TaskAction } from "./domain/taskActions";
import { NewTaskForm } from "./components/NewTaskForm";

export function App() {
  const [tasks, setTasks] = useState<TaskResponse[]>(sampleTasks);

  const unDone: TaskResponse[] = tasks.filter((t) => t.status !== "Done");
  const done: TaskResponse[] = tasks.filter((t) => t.status === "Done");

  function handleAction(taskId: string, action: TaskAction) {
    const now = new Date().toISOString();
    setTasks(current => current.map(t => (t.id == taskId ? applyActionLocally(t, action, now) : t)));
  }

  function handleCreate(title: string) {
    const curr = createTaskLocally(title, new Date().toISOString());
    setTasks(current => [...current, curr]);
  }

  return (
    <div className="mx-auto max-w-3xl px-4 pt-6 pb-12">
      <header className="mb-6 flex items-center justify-between">
        <h1>TaskBoard</h1>
        <span className="text-gray-500">Otwarte: {unDone.length}</span>
      </header>

      <Panel title="Nowe zadanie">
        <NewTaskForm onCreate={(title) => (handleCreate(title))} />
      </Panel>

      <Panel title="Do zrobienia">
        <TaskList tasks={unDone} emptyText="Wszystko zrobione!" onAction={(taskId, action) => (handleAction(taskId, action))} />
      </Panel>

      <Panel title="Zrobione">
        <TaskList tasks={done} emptyText="Wszystko zrobione!" onAction={(taskId, action) => (handleAction(taskId, action))} />
      </Panel>
    </div>
  );
}
