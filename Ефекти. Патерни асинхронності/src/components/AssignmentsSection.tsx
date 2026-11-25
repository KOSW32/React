import { useEffect, useState } from "react";
import type { Assignment } from "../types/assignment";
import { fetchAssignments, fetchAssignmentById } from "../api/assignmentsApi";
import AssignmentCard from "./AssignmentCard";
import AssignmentDetails from "./AssignmentDetails";

export default function AssignmentsSection() {
  const [list, setList] = useState<Assignment[]>([]);
  const [listLoading, setListLoading] = useState(true);

  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [details, setDetails] = useState<Assignment | null>(null);
  const [detailsLoading, setDetailsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchAssignments()
      .then(setList)
      .finally(() => setListLoading(false));
  }, []);

  const handleCardClick = async (id: number) => {
    setSelectedId(id);
    setDetails(null);
    setError(null);
    setDetailsLoading(true);
    try {
      const data = await fetchAssignmentById(id);
      setDetails(data);
    } catch (e) {
      setError(e instanceof Error ? e.message : "Невідома помилка");
    } finally {
      setDetailsLoading(false);
    }
  };

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
                onClick={handleCardClick}
              />
            ))}
          </div>
          <AssignmentDetails assignment={details} loading={detailsLoading} error={error} />
        </div>
      )}
    </section>
  );
}
