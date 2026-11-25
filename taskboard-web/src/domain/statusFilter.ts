import type { TaskStatus } from "../api/types";

export type StatusFilterValue = TaskStatus | 'All';

const filterValues: StatusFilterValue[] = ['All', 'Todo', 'InProgress', 'All'];

export function parseStatusFilter(value: string | null): StatusFilterValue {
    return filterValues.find(v => v === value) ?? 'All';
}