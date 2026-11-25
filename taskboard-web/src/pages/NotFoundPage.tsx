import { Link } from "react-router";
import { Panel } from "../components/Panel";

export function NotFoundPage() {
    return (
        <Panel title="Nie ma takiej strony">
            <p>
                <Link to="/projects">Wroc do Projektow</Link>
            </p>
        </Panel>
    )
}