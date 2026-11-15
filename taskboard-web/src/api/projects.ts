import { apiFetch } from "./client";
import type { ProjectResponse, ProjectSummaryResponse } from "./types";

export async function getProjects(signal?: AbortSignal): Promise<ProjectSummaryResponse[]> {
    return await apiFetch<ProjectSummaryResponse[]>('/projects', { signal });
}

export async function createProject(name: string): Promise<ProjectResponse> {
    return await apiFetch<ProjectResponse>('/projects', { method: 'POST', body: { name } });
}

export async function archiveProject(id: string): Promise<ProjectResponse> {
    return await apiFetch<ProjectResponse>(`/projects/${id}/archive`, { method: 'POST' });
}

export async function restoreProject(id: string): Promise<ProjectResponse> {
    return await apiFetch<ProjectResponse>(`/projects/${id}/restore`, { method: 'POST' });
}