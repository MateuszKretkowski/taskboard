import type { TaskStatus } from "../api/types"
import type { TaskAction } from "../domain/taskActions"

import { allowedActions, actionLabels } from "../domain/taskActions";

type TaskActionsProps = {
    status: TaskStatus,
    disabled?: boolean,
    onAction: (action: TaskAction) => void
};

export function TaskActions({ status, disabled, onAction }: TaskActionsProps  ) {
    return(
        <div>
            {allowedActions(status).map(a => (
                <button key={a} type="button" disabled={disabled} onClick={() => (onAction(a))}>{actionLabels[a]}</button>
            ))}
        </div>
    );
}