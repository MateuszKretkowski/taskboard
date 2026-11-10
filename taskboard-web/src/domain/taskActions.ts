import type { TaskStatus } from "../api/types";

export type TaskAction = "Start" | "Complete" | "Reopen"

export function allowedActions(action: TaskStatus): TaskAction[] {
    switch (action) {
        case "Todo":
            return ["Start", "Complete"];
        case "Done":
            return ["Reopen"]
        case "InProgress":
            return ["Complete"]
    }
}

export const actionLabels: Record<TaskAction, string> = {
    "Start": "Rozpocznij",
    "Complete": "Ukoncz",
    "Reopen": "Otworz"
};