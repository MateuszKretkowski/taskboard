import { useEffect, useState } from "react";
import { Link, useParams } from "react-router";
import { getProject } from "../api/projects";
import type { ProjectResponse } from "../api/types";
import type { LoadState } from "../loadState";
import { ApiError, readMessage } from "../api/client";
import { NotFoundPage } from "./NotFoundPage";
import { ErrorMessage } from "../components/ErrorMessage";
import { ProjectTasksPanel } from "./ProjectTasksPanel";

type ProjectResult = {
    projectId: string,
    state: LoadState<ProjectResponse> | { kind: 'notFound' }
}

export function ProjectPage() {
    const { projectId = '' } = useParams();
    const [result, setResult] = useState<ProjectResult | null>(null);

    useEffect(() => {
        const conrtoller = new AbortController();

        getProject(projectId, conrtoller.signal)
        .then(p => {
            setResult({ projectId, state: { kind: 'loaded', data: p } });
        })
        .catch(error => {
            if (conrtoller.signal.aborted) {
                return;
            }

            const notFound = error instanceof ApiError && error.status == 404;
            setResult({ projectId, state: notFound ? { kind: 'notFound' } : { kind: 'error', message: readMessage(error) } });
        });
    }, [projectId]);

    const state = result !== null && result.projectId === projectId ? result.state : { kind: 'loading' as const };

    if (state.kind === 'notFound') {
        return <NotFoundPage />
    }

    return (
        <>
            <p>
                <Link to='/projects'>
                    Wszystkie Projekty
                </Link>
            </p>

            {state.kind === 'loading' && <p className="text-gray-500">Ladowanie Projektu...</p>}
            {state.kind === 'error' && <ErrorMessage message={state.message} />}
            {state.kind === 'loaded' && (
                <ProjectTasksPanel key={state.data.id} projectId={state.data.id} title={state.data.title} />
            )}
        </>
    );
}