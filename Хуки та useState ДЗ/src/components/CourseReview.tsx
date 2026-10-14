import { useState } from 'react'
import clsx from 'clsx'

const ratings = [1, 2, 3, 4, 5]

function CourseReview() {
  const [rating, setRating] = useState<number>(5)
  const [reviewText, setReviewText] = useState<string>('')
  const [isSubmitted, setIsSubmitted] = useState<boolean>(false)

  if (isSubmitted) {
    return (
      <div className="review review--thanks">
        <p>
          Дякуємо за відгук! Ваша оцінка: {rating}. Ваш коментар:{' '}
          {reviewText.trim() === '' ? '(без коментаря)' : reviewText}
        </p>
      </div>
    )
  }

  return (
    <form
      className="review"
      onSubmit={(event) => {
        event.preventDefault()
        setIsSubmitted(true)
      }}
    >
      <p className="review__label">Ваша оцінка курсу:</p>
      <div className="review__ratings">
        {ratings.map((value) => (
          <button
            key={value}
            type="button"
            className={clsx('review__rating', {
              'review__rating--active': value === rating,
            })}
            onClick={() => setRating(value)}
          >
            {value}
          </button>
        ))}
      </div>

      <label className="review__label" htmlFor="review-text">
        Ваш відгук:
      </label>
      <textarea
        id="review-text"
        className="review__textarea"
        rows={5}
        value={reviewText}
        onChange={(event) => setReviewText(event.target.value)}
        placeholder="Поділіться враженнями про курс"
      />

      <button type="submit" className="review__submit">
        Відправити відгук
      </button>
    </form>
  )
}

export default CourseReview
