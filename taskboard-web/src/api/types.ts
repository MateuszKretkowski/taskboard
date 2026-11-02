export type TaskStatus = "Todo" | "InProgress" | "Done";

export type TaskResponse = {
    id: string;
    projectId: string;
    title: string;
    status: TaskStatus;
    createdAt: string;
    completedAt: string | null;
}

export type ProjectResponse = {
    id: string;
    title: string;
    isArchived: boolean;
    craetedAt: string;
}

export type ProjectSummaryResponse = {
    projectId: string;
    name: string;
    isArchived: boolean;
    openTaskCount: number;
    doneTaskCount: number;
}

export type CurrentUserResponse = {
    id: string;
    email: string
}

export type PagedResponse<T> = {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export type ProblemDetails = {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    traceId?: string;
    errors?: Record<string, string[]>;
}