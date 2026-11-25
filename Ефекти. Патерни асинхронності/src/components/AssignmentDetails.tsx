import type { Assignment } from "../types/assignment";
import { formatDate } from "./AssignmentCard";

interface Props {
  assignment: Assignment | null;
  loading: boolean;
  error: string | null;
}

export default function AssignmentDetails({ assignment, loading, error }: Props) {
  if (loading) return <div className="details">Завантаження з БД...</div>;
  if (error) return <div className="details error">{error}</div>;
  if (!assignment) return <div className="details">Оберіть завдання, щоб побачити деталі.</div>;

  return (
    <div className="details">
      <h2>{assignment.courseName}</h2>
      <p><b>Модуль:</b> {assignment.moduleName}</p>
      <p><b>Опис:</b> {assignment.description}</p>
      <p><b>Дата видачі:</b> {formatDate(assignment.issueDate)}</p>
      <p><b>Дедлайн:</b> {formatDate(assignment.deadline)}</p>
      <p>
        <b>Статус:</b>{" "}
        <span className={assignment.status === "Здано" ? "done" : "not-done"}>
          {assignment.status}
        </span>
      </p>
    </div>
  );
}
