import type { ProjectSummaryResponse } from "../api/types";

type projectListProps = {
    projects: ProjectSummaryResponse[],
    selectedId: string | null,
    onSelect: (id: string) => void
}

export function ProjectList({ projects, selectedId, onSelect }: projectListProps) {
    if (projects.length === 0) {
        return <p className="text-gray-500">Nie masz jeszcze projektów.</p>;
    }

    return (
    <ul className="grid gap-2">
      {projects.map(project => (
        <li key={project.projectId} className="rounded-lg border border-gray-200 bg-white px-3 py-2">
          <div className="flex items-center justify-between gap-3">
            <span className="font-semibold">
              {project.name} 
              {project.isArchived 
                && 
                <span className="rounded-full px-2 py-0.5 text-xs font-semibold whitespace-nowrap text-white bg-gray-500">Archiwum</span>}
            </span>
            <button type="button" disabled={project.projectId === selectedId} onClick={() => onSelect(project.projectId)}>
              {project.projectId === selectedId ? 'Otwarty' : 'Otwórz'}
            </button>
          </div>
          <small>
            {project.openTaskCount} otwartych, {project.doneTaskCount} ukończonych
          </small>
        </li>
      ))}
    </ul>
  );
}