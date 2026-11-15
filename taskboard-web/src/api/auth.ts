import { ApiError, apiFetch } from "./client";
import type { CurrentUserResponse } from "./types";

export async function getCurrentUser(): Promise<CurrentUserResponse | null> {
    try {
        return await apiFetch<CurrentUserResponse>('/auth/me');
    }
    catch (error) {
        if (error instanceof ApiError && error.status == 401) {
            return null;
        }
        throw error;
    }
}

export async function login(email: string, password: string ): Promise<CurrentUserResponse> {
    return apiFetch<CurrentUserResponse>('/auth/login', { method: 'POST', body: { email, password } });
}

export async function register(email: string, password: string): Promise<CurrentUserResponse> {
    return apiFetch<CurrentUserResponse>('/auth/register', { method: 'POST', body: { email, password } });
}

export async function logout(): Promise<void> {
    return apiFetch('/auth/logout', { method: 'POST' });
}