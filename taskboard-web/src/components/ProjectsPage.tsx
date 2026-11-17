import { useEffect, useState } from "react";
import { type LoadState } from "../loadState";
import type { ProjectSummaryResponse } from "../api/types";
import { createProject, getProjects } from "../api/projects";
import { readMessage } from "../api/client";
import { Panel } from "./Panel";
import { TextForm } from "./TextForm";
import { ErrorMessage } from "./ErrorMessage";
import { ProjectList } from "./ProjectList";
import { ProjectTasksPanel } from "../pages/ProjectTasksPanel";

export function ProjectsPage() {
    const [state, setState] = useState<LoadState<ProjectSummaryResponse[]>>({ kind: 'loading' });
    const [reloadToken, setReloadToken] = useState(0);
    const [selectedId, setSelectedId] = useState<string | null>(null);

    useEffect(() => {
        const controller = new AbortController();

        getProjects(controller.signal)
        .then(data => setState({ kind: 'loaded', data }))
        .catch(error => {
            if (!controller.signal.aborted) {
                setState({ kind: 'error' , message: readMessage(error)});
            }
        })

        return () => controller.abort();
    }, [reloadToken]);

    function reload() {
        setReloadToken(t => t+1);
    }

    async function handleCreate(name: string) {
        const project = await createProject(name);
        setSelectedId(project.id);
        reload();
    }

    const selected = state.kind === 'loaded' ? state.data.find(p => p.projectId===selectedId) : undefined;

    return (
        <>
            <Panel title="Projekty">
                <TextForm label="Nowy projekt" placeHolder="np. Dom" maxLength={100} submitLabel="Utworz" serverField="Name" onSubmit={handleCreate} />
                {state.kind === 'loading' && <p className="text-gray-500">Ładowanie projektów…</p>}
                {state.kind === 'error' && <ErrorMessage message={state.message} />}
                {state.kind === 'loaded' && <ProjectList projects={state.data} selectedId={selectedId} onSelect={setSelectedId} />}
            </Panel>
            {selected !== undefined && <ProjectTasksPanel key={selected.projectId} project={selected} onTasksChanged={reload} />}
        </>
    );
}