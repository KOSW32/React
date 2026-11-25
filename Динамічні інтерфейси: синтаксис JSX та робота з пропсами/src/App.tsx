import LessonCard from './components/LessonCard'
import Section from './components/Section'

interface Lesson {
  id: number
  topic: string
  date: string
  isOnline: boolean
  zoomLink?: string
}

const lessons: Lesson[] = [
  {
    id: 1,
    topic: 'Компоненти та пропси у React',
    date: '26 листопада, 10:00',
    isOnline: true,
    zoomLink: 'https://zoom.us/j/1234567890',
  },
  {
    id: 2,
    topic: 'Робота зі станом компонентів',
    date: '27 листопада, 12:30',
    isOnline: false,
  },
  {
    id: 3,
    topic: 'React Hooks: useState та useEffect',
    date: '28 листопада, 09:00',
    isOnline: true,
    zoomLink: 'https://zoom.us/j/2345678901',
  },
  {
    id: 4,
    topic: 'Практика: створення вебінтерфейсу',
    date: '30 листопада, 14:00',
    isOnline: false,
  },
]

export default function App() {
  return (
    <main className="page">
      <header className="page-header">
        <p className="page-header__eyebrow">Мій навчальний план</p>
        <h1>Навчання без пропусків</h1>
        <p className="page-header__description">
          Перевірте час і формат найближчих занять.
        </p>
      </header>

      <Section title="Розклад">
        {lessons.map((lesson) => (
          <LessonCard key={lesson.id} {...lesson} />
        ))}
      </Section>
    </main>
  )
}
