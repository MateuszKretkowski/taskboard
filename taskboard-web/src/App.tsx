import { useState } from "react";
import type { TaskResponse } from "./api/types";
import { Panel } from "./components/Panel";
import { TaskList } from "./components/TaskList";
import { sampleTasks } from "./data/sampleTasks";
import { applyActionLocally, createTaskLocally } from "./data/localTasks";
import type { TaskAction } from "./domain/taskActions";
import { NewTaskForm } from "./components/NewTaskForm";
import { StatusFilter, type StatusFilterValue } from "./components/StatusFilter";

export function App() {
  const [tasks, setTasks] = useState<TaskResponse[]>(sampleTasks);
  const [filter, setFilter] = useState<StatusFilterValue>("All");
  const visibleTasks = filter == "All" ? tasks : tasks.filter(t => t.status==filter);
  const openCount = tasks.filter(t => t.status!=="Done").length;

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
        <span className="text-gray-500">Otwarte: {openCount}</span>
      </header>

      <Panel title="Nowe zadanie">
        <NewTaskForm onCreate={(title) => (handleCreate(title))} />
      </Panel>

      <Panel title="Zadania" >
        <StatusFilter value={filter} onChange={(value: StatusFilterValue) => {setFilter(value)}} />
        <TaskList tasks={visibleTasks} emptyText="Wszystko zrobione!" onAction={(taskId, action) => (handleAction(taskId, action))} />
      </Panel>
    </div>
  );
}
