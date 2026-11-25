import type { Assignment } from "../types/assignment";

interface Props {
  assignment: Assignment;
  selected: boolean;
  onClick: (id: number) => void;
}

export function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString("uk-UA");
}

export default function AssignmentCard({ assignment, selected, onClick }: Props) {
  return (
    <div
      className={`assignment-card${selected ? " selected" : ""}`}
      onClick={() => onClick(assignment.id)}
      role="button"
      tabIndex={0}
      onKeyDown={(e) => e.key === "Enter" && onClick(assignment.id)}
    >
      <h3>{assignment.courseName}</h3>
      <p>Дата видачі: {formatDate(assignment.issueDate)}</p>
    </div>
  );
}
