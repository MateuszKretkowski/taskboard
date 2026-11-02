import type { ProjectSummaryResponse } from "../api/types";

type ProjectHeaderProps = {
    projectSummaryResponse: ProjectSummaryResponse
}

export function ProjectHeader({ projectSummaryResponse }: ProjectHeaderProps) {
    const total: number = projectSummaryResponse.doneTaskCount + projectSummaryResponse.openTaskCount;
    return (
        <div>
            <div>
                <p>{projectSummaryResponse.name} {projectSummaryResponse.isArchived && <span>Archiwum</span>}</p>
            </div>
            <div>
                <p>{total === 0 ? 'Brak zadań' : `${projectSummaryResponse.openTaskCount} zadan otwartych, ${projectSummaryResponse.doneTaskCount} zadan zamknietych`}</p>
            </div>
        </div>
    );
}