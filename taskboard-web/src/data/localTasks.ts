import type { TaskResponse } from '../api/types';
import type { TaskAction } from '../domain/taskActions';

export function applyActionLocally(task: TaskResponse, action: TaskAction, nowIso: string): TaskResponse {
  switch (action) {
    case "Start":
      return { ...task, status: 'InProgress' };
    case "Complete":
      return { ...task, status: 'Done', completedAt: nowIso };
    case 'Reopen':
      return { ...task, status: 'Todo', completedAt: null };
  }
}

export function createTaskLocally(title: string, nowIso: string): TaskResponse {
    return {id: crypto.randomUUID(), projectId: 'local', title, status: "Todo", createdAt: nowIso, completedAt: null};
}