import type { TaskStatus } from "../api/types";

type StatusFilterProps = {
    value: StatusFilterValue
    onChange: (value: StatusFilterValue) => void
}

export type StatusFilterValue = "All" | TaskStatus;

export function StatusFilter({ value, onChange }: StatusFilterProps) {
    return (
        <div>
            Pokaz   
            <select value={value} onChange={event => onChange(event.target.value as StatusFilterValue)}>
                <option value="All">All</option>
                <option value="Todo">Otwarte</option>
                <option value="InProgess">Rozpoczete</option>
                <option value="Done">Zakonczone</option>
            </select>
        </div>
    );
}