import type { ProblemDetails } from "./types";

export class ApiError extends Error {
    readonly status: number;
    readonly problem: ProblemDetails;

    constructor(status: number, problem: ProblemDetails) {
        super(problem.detail ?? problem.title ?? `Blad HTTP {status}`);
        this.name = 'ApiError';
        this.status = status;
        this.problem = problem;
    }

    fieldError(field: string): string | undefined {
        return this.problem.errors?.[field]?.[0];
    }
}

type ApiOptions = {
    method?: 'GET' | "POST" | 'PUT' | 'DELETE';
    body?: unknown;
    signal?: AbortSignal;
};

export async function apiFetch<T>(path: string, options: ApiOptions = {}): Promise<T> {
    const response = await fetch(`/api${path}`, {
        method: options.method ?? 'GET',
        headers: options.body === undefined ? undefined : { 'Content-Type': 'application/json' },
        body: options.signal === undefined ? undefined : JSON.stringify(options.body),
        signal: options.signal
    });

    if (!response.ok) {
        throw new ApiError(response.status, await readProblem(response));
    }

    if (response.status === 204) {
        return undefined as T
    }

    return (await response.json()) as T
}

async function readProblem(response: Response): Promise<ProblemDetails> {
    try {
        return (await response.json()) as ProblemDetails;
    }
    catch {
        return { status: response.status, title: response.statusText };
    }
}

export function readMessage(error: unknown): string {
    if (error instanceof ApiError) {
        return error.message;
    }
    if (error instanceof Error) {
        return error.message;
    }
    return 'Nieznany blad';
}