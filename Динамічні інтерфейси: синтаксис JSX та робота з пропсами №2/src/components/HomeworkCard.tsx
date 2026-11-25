import clsx from 'clsx'

export interface HomeworkCardProps {
  title: string
  course: string
  isCompleted: boolean
  score?: number
}

export default function HomeworkCard({
  title,
  course,
  isCompleted,
  score,
}: HomeworkCardProps) {
  return (
    <article
      className={clsx('homework-card', {
        'homework-card--completed': isCompleted,
        'homework-card--pending': !isCompleted,
      })}
    >
      <div className="homework-card__content">
        <p className="homework-card__course">{course}</p>
        <h3 className="homework-card__title">{title}</h3>
        <span className="homework-card__status">
          {isCompleted ? 'Виконано' : 'Очікує виконання'}
        </span>
      </div>

      {isCompleted ? (
        <p className="homework-card__result">
          {score !== undefined ? `Оцінка: ${score}/100` : 'Очікує перевірки...'}
        </p>
      ) : (
        <button className="homework-card__button" type="button">
          Здати роботу
        </button>
      )}
    </article>
  )
}
