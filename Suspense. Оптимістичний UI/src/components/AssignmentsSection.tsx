import { Suspense, useEffect, useState } from "react";
import type { Assignment } from "../types/assignment";
import { fetchAssignments } from "../api/assignmentsApi";
import AssignmentCard from "./AssignmentCard";
import AssignmentDetails from "./AssignmentDetails";
import AssignmentDetailsSkeleton from "./AssignmentDetailsSkeleton";
import ErrorBoundary from "./ErrorBoundary";

export default function AssignmentsSection() {
  const [list, setList] = useState<Assignment[]>([]);
  const [listLoading, setListLoading] = useState(true);
  const [selectedId, setSelectedId] = useState<number | null>(null);

  useEffect(() => {
    fetchAssignments()
      .then(setList)
      .finally(() => setListLoading(false));
  }, []);

  return (
    <section className="assignments-section">
      <h1>Завдання</h1>
      {listLoading ? (
        <p>Завантаження...</p>
      ) : (
        <div className="assignments-layout">
          <div className="cards">
            {list.map((a) => (
              <AssignmentCard
                key={a.id}
                assignment={a}
                selected={a.id === selectedId}
                onClick={setSelectedId}
              />
            ))}
          </div>

          {selectedId === null ? (
            <div className="details">Оберіть завдання, щоб побачити деталі.</div>
          ) : (
            // key — щоб при зміні картки скидались і помилка, і стан деталей
            <ErrorBoundary key={selectedId}>
              <Suspense fallback={<AssignmentDetailsSkeleton />}>
                <AssignmentDetails assignmentId={selectedId} />
              </Suspense>
            </ErrorBoundary>
          )}
        </div>
      )}
    </section>
  );
}
