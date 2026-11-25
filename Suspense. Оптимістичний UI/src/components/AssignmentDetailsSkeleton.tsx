export default function AssignmentDetailsSkeleton() {
  return (
    <div className="details" aria-busy="true" aria-label="Завантаження деталей завдання">
      <div className="skeleton skeleton-title" />
      <div className="skeleton skeleton-line" />
      <div className="skeleton skeleton-line skeleton-line-long" />
      <div className="skeleton skeleton-line skeleton-line-long" />
      <div className="skeleton skeleton-line" />
      <div className="skeleton skeleton-line" />
      <div className="skeleton skeleton-badge" />
      <div className="skeleton skeleton-button" />
    </div>
  );
}
