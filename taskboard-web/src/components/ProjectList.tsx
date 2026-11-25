import { Link } from "react-router";
import type { ProjectSummaryResponse } from "../api/types";

type projectListProps = {
    projects: ProjectSummaryResponse[],
}

export function ProjectList({ projects }: projectListProps) {
    if (projects.length === 0) {
        return <p className="text-gray-500">Nie masz jeszcze projektów.</p>;
    }

    return (
    <ul className="grid gap-2">
      {projects.map(project => (
        <li key={project.id} className="rounded-lg border border-gray-200 bg-white px-3 py-2">
          <div className="flex items-center justify-between gap-3">
            <span className="font-semibold">
              {project.name} 
              {project.isArchived 
                && 
                <span className="rounded-full px-2 py-0.5 text-xs font-semibold whitespace-nowrap text-white bg-gray-500">Archiwum</span>}
            </span>
            <Link className="font-semibold" to={`/projects/${project.id}`}>{project.name}</Link>
          </div>
          <small>
            {project.openTaskCount} otwartych, {project.doneTaskCount} ukończonych
          </small>
        </li>
      ))}
    </ul>
  );
}