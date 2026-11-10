import { useState, type SubmitEvent } from "react";

type NewTaskFormProps = {
    onCreate: (value: string) => void
}

export function NewTaskForm({ onCreate }: NewTaskFormProps) {
    const maxTitleLength: number = 200;
    const [title, setTitle] = useState("");

    const trimmed = title.trim();
    const error =
    trimmed.length > maxTitleLength ? `Tytuł może mieć najwyżej ${maxTitleLength} znaków.` : null;
    const canSubmit = trimmed.length > 0 && error === null;

    function handleSubmit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        if (!canSubmit) {
            return;
        }
        onCreate(trimmed);
        setTitle('');
    }

    return (
    <form onSubmit={handleSubmit}>
      <div className="flex flex-wrap items-end gap-2.5">
        <label className="flex-1">
          Nowe zadanie
          <input value={title} onChange={event => setTitle(event.target.value)} placeholder="np. Kupić mleko" />
        </label>
        <button type="submit" className="border-blue-700 bg-blue-700 text-white" disabled={!canSubmit}>
          Dodaj
        </button>
      </div>
      {error !== null && <span className="text-sm text-red-700">{error}</span>}
    </form>
    );
}