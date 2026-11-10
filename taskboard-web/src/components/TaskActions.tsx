import type { TaskStatus } from "../api/types"
import type { TaskAction } from "../domain/taskActions"

import { allowedActions, actionLabels } from "../domain/taskActions";

type TaskActionsProps = {
    status: TaskStatus,
    onAction: (action: TaskAction) => void
};

export function TaskActions({ status, onAction }: TaskActionsProps  ) {
    return(
        <div>
            {allowedActions(status).map(a => (
                <button key={a} type="button" onClick={() => (onAction(a))}>{actionLabels[a]}</button>
            ))}
        </div>
    );
}