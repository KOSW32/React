import HomeworkCard from './components/HomeworkCard'
import Section from './components/Section'

const homeworkItems = [
  {
    id: 1,
    title: 'Створення картки профілю',
    course: 'Основи React',
    isCompleted: true,
    score: 95,
  },
  {
    id: 2,
    title: 'Список товарів з компонентів',
    course: 'Компонентний підхід',
    isCompleted: true,
  },
  {
    id: 3,
    title: 'Форма реєстрації',
    course: 'Робота з формами',
    isCompleted: false,
  },
]

export default function App() {
  return (
    <main className="page">
      <header className="page-header">
        <div className="page-header__badge">
          <span className="page-header__badge-dot" /> Навчальний кабінет
        </div>
        <h1>Маленькі кроки — великий прогрес.</h1>
        <p>Виконуй завдання, відстежуй результат і рухайся далі.</p>
      </header>

      <Section title="Мої домашки">
        {homeworkItems.map((homework) => (
          <HomeworkCard key={homework.id} {...homework} />
        ))}
      </Section>

      <footer className="page-footer">Продовжуй у своєму темпі ✨</footer>
    </main>
  )
}
