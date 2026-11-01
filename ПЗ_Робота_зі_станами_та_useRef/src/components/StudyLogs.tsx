import { useRef, useState } from 'react'

const courses = ['React Frontend', 'TypeScript Basics', 'Node.js API']

function StudyLogs() {
  const logs = useRef<string[]>([])
  const lessonCounters = useRef<Record<string, number>>({})
  const [showLogs, setShowLogs] = useState<boolean>(false)

  const addLog = (courseTitle: string, lessonNumber: number) => {
    const time = new Date().toLocaleTimeString('uk-UA', { hour12: false })
    const line = `[${time}] Пройдено урок №${lessonNumber} курсу '${courseTitle}'`
    logs.current.push(line)
  }

  const completeLesson = (courseTitle: string) => {
    const nextLesson = (lessonCounters.current[courseTitle] ?? 0) + 1
    lessonCounters.current[courseTitle] = nextLesson
    addLog(courseTitle, nextLesson)
  }

  return (
    <div className="logs">
      <div className="logs__courses">
        {courses.map((course) => (
          <div key={course} className="logs__course">
            <span className="logs__course-title">{course}</span>
            <button
              type="button"
              className="logs__lesson-button"
              onClick={() => completeLesson(course)}
            >
              Пройти урок
            </button>
          </div>
        ))}
      </div>

      <button
        type="button"
        className="logs__toggle"
        onClick={() => setShowLogs(!showLogs)}
      >
        {showLogs ? 'Сховати історію логів' : 'Показати історію логів'}
      </button>

      {showLogs && (
        <div className="logs__history">
          {logs.current.length === 0 ? (
            <p className="logs__empty">Логів поки немає</p>
          ) : (
            <ul className="logs__list">
              {logs.current.map((line, index) => (
                <li key={index}>{line}</li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  )
}

export default StudyLogs
