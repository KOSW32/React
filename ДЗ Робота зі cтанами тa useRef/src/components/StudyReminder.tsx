import { useEffect, useRef, useState } from 'react'
import toast from 'react-hot-toast'

function StudyReminder() {
  const [text, setText] = useState<string>('')
  const [seconds, setSeconds] = useState<string>('5')
  const [isActive, setIsActive] = useState<boolean>(false)

  const timerId = useRef<ReturnType<typeof setTimeout> | null>(null)

  useEffect(() => {
    return () => {
      if (timerId.current !== null) {
        clearTimeout(timerId.current)
      }
    }
  }, [])

  const handleStart = () => {
    const delay = Number(seconds)

    if (text.trim() === '') {
      toast.error('Введіть текст нагадування')
      return
    }

    if (!Number.isFinite(delay) || delay <= 0) {
      toast.error('Вкажіть кількість секунд більше нуля')
      return
    }

    if (timerId.current !== null) {
      clearTimeout(timerId.current)
    }

    const reminderText = text.trim()

    timerId.current = setTimeout(() => {
      toast(reminderText, { icon: '⏰', duration: 6000 })
      timerId.current = null
      setIsActive(false)
    }, delay * 1000)

    setIsActive(true)
    toast.success(`Таймер встановлено на ${delay} с`)
  }

  const handleCancel = () => {
    if (timerId.current !== null) {
      clearTimeout(timerId.current)
      timerId.current = null
    }

    setIsActive(false)
    toast('Таймер скасовано')
  }

  return (
    <div className="reminder">
      <label className="reminder__field" htmlFor="reminder-text">
        Текст нагадування
        <input
          id="reminder-text"
          type="text"
          value={text}
          onChange={(event) => setText(event.target.value)}
          placeholder="Наприклад: Час повторити хуки!"
        />
      </label>

      <label className="reminder__field" htmlFor="reminder-seconds">
        Час, секунд
        <input
          id="reminder-seconds"
          type="number"
          min="1"
          value={seconds}
          onChange={(event) => setSeconds(event.target.value)}
        />
      </label>

      <div className="reminder__actions">
        <button type="button" className="reminder__start" onClick={handleStart}>
          Запустити таймер
        </button>

        {isActive && (
          <button type="button" className="reminder__cancel" onClick={handleCancel}>
            Скасувати
          </button>
        )}
      </div>

      {isActive && <p className="reminder__status">Таймер працює...</p>}
    </div>
  )
}

export default StudyReminder
