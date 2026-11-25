import type { TaskStatus } from "../api/types";
import { parseStatusFilter, type StatusFilterValue } from "../domain/statusFilter";

type StatusFilterProps = {
    value: StatusFilterValue
    onChange: (value: StatusFilterValue) => void
}

export function StatusFilter({ value, onChange }: StatusFilterProps) {
    return (
        <div>
            Pokaz   
            <select value={value} onChange={event => onChange(parseStatusFilter(event.target.value))}>
                <option value="All">All</option>
                <option value="Todo">Otwarte</option>
                <option value="InProgress">Rozpoczete</option>
                <option value="Done">Zakonczone</option>
            </select>
        </div>
    );
}