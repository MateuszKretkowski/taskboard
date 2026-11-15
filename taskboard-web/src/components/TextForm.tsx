import { useState, type SubmitEvent } from "react";
import { ApiError, readMessage } from "../api/client";

type textFormProps = {
    label: string,
    placeHolder: string,
    maxLength: number,
    submitLabel: string,
    serverField: string,
    onSubmit: (value: string) => Promise<void>
};

export function TextForm({ label, placeHolder, maxLength, submitLabel, serverField, onSubmit }: textFormProps) {
    const [value, setValue] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [serverError, setServerError] = useState<string | null>(null);

    const trimmed = value.trim();
    const localError = trimmed.length > maxLength ? `Najwyzej ${maxLength} znakow` : null;
    const canSubmit = trimmed.length > 0 && localError == null && !submitting;

    async function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        if (!canSubmit) {
            return;
        }
        setSubmitting(true);
        setServerError(null);
        try {
            await onSubmit(value);
            setValue('');
        }
        catch (error) {
            setServerError(error instanceof ApiError ? (error.fieldError(serverField) ?? error.message) : readMessage(error));
        }
        finally {
            setSubmitting(false);
        }
    }

    const shownError = localError ?? serverError;

    return (
    <form onSubmit={handleSubmit}>
      <div className="flex flex-wrap items-end gap-2.5">
        <label className="flex-1">
          {label}
          <input value={value} onChange={event => setValue(event.target.value)} placeholder={placeHolder} />
        </label>
        <button type="submit" className="border-blue-700 bg-blue-700 text-white" disabled={!canSubmit}>
          {submitting ? 'Zapisuję…' : submitLabel}
        </button>
      </div>
      {shownError !== null && <span className="text-sm text-red-700">{shownError}</span>}
    </form>
    );
}