import type { TaskAction } from "../domain/taskActions";
import { apiFetch } from "./client";
import type { PagedResponse, TaskResponse, TaskStatus } from "./types"

export type TaskQuery = {
    status?: TaskStatus,
    page: number,
    pageSize: number
}

export async function searchTasks(projectId: string, query: TaskQuery, signal?: AbortSignal): Promise<PagedResponse<TaskResponse>> {
    const params = new URLSearchParams({ page: String(query.page), pageSize: String(query.pageSize) });
    if (query.status !== undefined) {
        params.set('status', query.status);
    }
    return await apiFetch<PagedResponse<TaskResponse>>(`/projects/${projectId}/tasks?${params}`, { signal });
}

export async function createTask(projectId: string, title: string): Promise<TaskResponse> {
    return await apiFetch(`/projects/${projectId}/tasks`, { method: 'POST', body: { title } });
}

export async function applyTaskAction(taskId: string, action: TaskAction): Promise<TaskResponse> {
    return await apiFetch(`/tasks/${taskId}/${action}`, { method: 'PUT' });
}