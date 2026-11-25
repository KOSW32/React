import { use, useOptimistic, useState, useTransition } from "react";
import type { AssignmentStatus } from "../types/assignment";
import { completeAssignment, getAssignmentPromise, invalidateAssignment } from "../api/assignmentsApi";
import { formatDate } from "./AssignmentCard";

interface Props {
  assignmentId: number;
}

export default function AssignmentDetails({ assignmentId }: Props) {
  // use() "підвішує" компонент, поки проміс не виконається -> спрацьовує <Suspense>
  const assignment = use(getAssignmentPromise(assignmentId));

  // Підтверджений (реальний) статус
  const [status, setStatus] = useState<AssignmentStatus>(assignment.status);
  // Оптимістичний статус: одразу показуємо "Здано", поки йде запит
  const [optimisticStatus, setOptimisticStatus] = useOptimistic<AssignmentStatus, AssignmentStatus>(
    status,
    (_current, next) => next
  );
  const [isPending, startTransition] = useTransition();
  const [error, setError] = useState<string | null>(null);

  const handleComplete = () => {
    setError(null);
    startTransition(async () => {
      setOptimisticStatus("Здано");
      try {
        await completeAssignment(assignment.id);
        invalidateAssignment(assignment.id); // наступне відкриття візьме свіжі дані
        startTransition(() => setStatus("Здано"));
      } catch (e) {
        // статус автоматично відкотиться до підтвердженого значення
        setError(e instanceof Error ? e.message : "Невідома помилка");
      }
    });
  };

  const isDone = optimisticStatus === "Здано";

  return (
    <div className="details">
      <h2>{assignment.courseName}</h2>
      <p><b>Модуль:</b> {assignment.moduleName}</p>
      <p><b>Опис:</b> {assignment.description}</p>
      <p><b>Дата видачі:</b> {formatDate(assignment.issueDate)}</p>
      <p><b>Дедлайн:</b> {formatDate(assignment.deadline)}</p>
      <p>
        <b>Статус:</b>{" "}
        <span className={`${isDone ? "done" : "not-done"}${isPending ? " pending" : ""}`}>
          {optimisticStatus}
          {isPending && " (зберігається...)"}
        </span>
      </p>

      <button className="complete-btn" onClick={handleComplete} disabled={isDone || isPending}>
        {isDone ? "Завдання виконано" : "Виконати завдання"}
      </button>

      {error && <p className="error-text" role="alert">{error}</p>}
    </div>
  );
}
