import { useEffect, useState } from "react";
import { TaskList } from "../components/TaskList";
import type { PagedResponse, ProjectSummaryResponse, TaskResponse } from "../api/types";
import type { LoadState } from "../loadState";
import { StatusFilter, type StatusFilterValue } from "../components/StatusFilter";
import { applyTaskAction, createTask, searchTasks } from "../api/tasks";
import { readMessage } from "../api/client";
import type { TaskAction } from "../domain/taskActions";
import { TextForm } from "../components/TextForm";
import { ErrorMessage } from "../components/ErrorMessage";
import { Pager } from "../components/Pager";

type TaskResult = {
    key: string,
    state: LoadState<PagedResponse<TaskResponse>>
};

type ProjectTasksPanelProps = {
    project: ProjectSummaryResponse,
    onTasksChanged: () => void
};

export function ProjectTasksPanel({ project, onTasksChanged }: ProjectTasksPanelProps) {
    const [result, setResult] = useState<TaskResult | null>(null);
    const [filter, setFilter] = useState<StatusFilterValue>('All');
    const [page, setPage] = useState(1);
    const [reloadToken, setReloadToken] = useState(0);
    const [busyTaskId, setBusyTaskId] = useState<string | null>(null);
    const [actionError, setActionError] = useState<string | null>(null);

    const queryKey = `${project.id}|${filter}|${page}|${reloadToken}`

    const pageSize = 5;

    useEffect(() => {
        const conrtoller = new AbortController();
        const status = filter === 'All' ? undefined : filter;

        searchTasks(project.id, { status, page, pageSize }, conrtoller.signal)
        .then(r => {
            setResult({ key: queryKey, state: { kind: 'loaded', data: r }});
        }).catch(error => {
            setResult({ key: queryKey, state: { kind: 'error', message: readMessage(error) } });
        })

        return () => conrtoller.abort();
    }, [project.id, filter, page, queryKey]);

    const state: LoadState<PagedResponse<TaskResponse>> = result?.state ?? { kind: 'loading' };
    const refreshing = result !== null && result.key !== queryKey;

    function handleFilterChange(value: StatusFilterValue) {
        setFilter(value);
        setPage(1);
    }

    async function handleCreate(title: string) {
        await createTask(project.id, title);
        setReloadToken(t => t + 1);
        onTasksChanged();
    }

    async function handleAction(taskId: string, action: TaskAction) {
        setBusyTaskId(taskId);
        setActionError(null);
        try {
            const updated = await applyTaskAction(taskId, action);

            setResult(current => {
                if (current === null || current.state.kind !== 'loaded') {
                    return current;
                }
                const items = current.state.data.items.map(i => i.id===updated.id ? updated : i);
                return { ...current, state: { kind: 'loaded', data: { ...current.state.data, items } } }
            })
        }
        catch(error) {
            setActionError(readMessage(error));
        }
        finally {
            setBusyTaskId(null);
        }
    }

    return (
    <section className="mb-4 rounded-xl border border-gray-200 bg-white px-5 py-4">
      <h2>{project.name}</h2>
      <TextForm
        label="Nowe zadanie"
        placeHolder="np. Kupic mleko"
        maxLength={200}
        submitLabel="Dodaj"
        serverField="Title"
        onSubmit={handleCreate}
      />
      <StatusFilter value={filter} onChange={handleFilterChange} />
      <ErrorMessage message={actionError} />
      {refreshing && <p className="text-gray-500">Odświeżam…</p>}

      {state.kind === 'loading' && <p className="text-gray-500">Ładowanie zadan...</p>}
      {state.kind === 'error' && <ErrorMessage message={state.message} />}
      {state.kind === 'loaded' && (
        <>
          <TaskList tasks={state.data.items} busyTaskId={busyTaskId} onAction={handleAction} emptyText="Brak zadan." />
          <Pager page={state.data.page} totalPages={state.data.totalPages} onChange={setPage} />
        </>
      )}
    </section>
  );
}