import { useEffect, useState } from "react";
import type { ProjectSummaryResponse } from "../api/types";
import type { LoadState } from "../loadState";
import { createProject, getProjects } from "../api/projects";
import { ErrorMessage } from "../components/ErrorMessage";
import { ProjectList } from "../components/ProjectList";
import { TextForm } from "../components/TextForm";
import { Panel } from "../components/Panel";
import { readMessage } from "../api/client";
import { ProjectTasksPanel } from "./ProjectTasksPanel";

export function ProjectsPage() {
    const [state, setState] = useState<LoadState<ProjectSummaryResponse[]>>({ kind: 'loading' });
    const [reloadToken, setReloadToken] = useState(0);
    const [selectedId, setSelectedId] = useState<string | null>(null);

    useEffect(() => {
        const controller = new AbortController();

        getProjects(controller.signal)
        .then(data => setState({kind: 'loaded', data: data}))
        .catch(error => setState({ kind: 'error', message: readMessage(error) }))

        return () => controller.abort();
    }, [reloadToken])

    function reload() {
        setReloadToken(token => token + 1);
    }

    async function handleCreate(name: string) {
        const project = await createProject(name);
        setSelectedId(project.id);
        reload();
    }

    const selected = state.kind === 'loaded' ? state.data.find(p => p.id === selectedId) : undefined;

    return (
    <>
      <Panel title="Projekty">
        <TextForm
          label="Nowy projekt"
          placeHolder="np. Dom"
          maxLength={100}
          submitLabel="Utwórz"
          serverField="Name"
          onSubmit={handleCreate}
        />
        {state.kind === 'loading' && <p className="text-gray-500">Ładowanie projektów…</p>}
        {state.kind === 'error' && <ErrorMessage message={state.message} />}
        {state.kind === 'loaded' && <ProjectList projects={state.data} selectedId={selectedId} onSelect={setSelectedId} />}
      </Panel>

      { selected !== undefined && <ProjectTasksPanel key={selected.id} project={selected} onTasksChanged={reload} /> }
    </>
  );
}