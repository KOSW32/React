import clsx from 'clsx'

interface LessonCardProps {
  topic: string
  date: string
  isOnline: boolean
  zoomLink?: string
}

export default function LessonCard({
  topic,
  date,
  isOnline,
  zoomLink,
}: LessonCardProps) {
  return (
    <article
      className={clsx('lesson-card', {
        'lesson-card--online': isOnline,
        'lesson-card--offline': !isOnline,
      })}
    >
      <div className="lesson-card__details">
        <h3 className="lesson-card__topic">{topic}</h3>
        <time className="lesson-card__date">{date}</time>
        <span className="lesson-card__format">
          {isOnline ? 'Онлайн' : 'Офлайн'}
        </span>
      </div>

      {isOnline && zoomLink && (
        <a
          className="lesson-card__button"
          href={zoomLink}
          target="_blank"
          rel="noreferrer"
        >
          Підключитися до Zoom
        </a>
      )}

      {!isOnline && (
        <p className="lesson-card__notice">
          Аудиторія 404. Не забудьте ноутбук!
        </p>
      )}
    </article>
  )
}
