import type { TaskResponse } from "./api/types";
import { Panel } from "./components/Panel";
import { TaskList } from "./components/TaskList";
import { sampleTasks } from "./data/sampleTasks";

export function App() {
  const unDone: TaskResponse[] = sampleTasks.filter((t) => t.status !== "Done");
  const done: TaskResponse[] = sampleTasks.filter((t) => t.status === "Done");

  return (
    <div className="mx-auto max-w-3xl px-4 pt-6 pb-12">
      <header className="mb-6 flex items-center justify-between">
        <h1>TaskBoard</h1>
        <span className="text-gray-500">Otwarte: {unDone.length}</span>
      </header>

      <Panel title="Do zrobienia">
        <TaskList tasks={unDone} emptyText="Wszystko zrobione!" />
      </Panel>

      <Panel title="Zrobione">
        <TaskList tasks={done} />
      </Panel>
    </div>
  );
}
