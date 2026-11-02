import type { TaskResponse } from "../api/types";

export const sampleTasks: TaskResponse[] = [
  {
    id: '01a10761-0000-7000-8000-000000000001',
    projectId: '01a10760-0000-7000-8000-000000000001',
    title: 'Kupić mleko',
    status: 'Done',
    createdAt: '2026-10-04T08:15:00Z',
    completedAt: '2026-10-04T09:02:00Z',
  },
  {
    id: '01a10761-0000-7000-8000-000000000002',
    projectId: '01a10760-0000-7000-8000-000000000001',
    title: 'Umyć auto',
    status: 'InProgress',
    createdAt: '2026-10-04T08:20:00Z',
    completedAt: null,
  },
  {
    id: '01a10761-0000-7000-8000-000000000003',
    projectId: '01a10760-0000-7000-8000-000000000001',
    title: 'Zapłacić rachunki',
    status: 'Todo',
    createdAt: '2026-10-04T08:31:00Z',
    completedAt: null,
  },
];