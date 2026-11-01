import { Toaster } from 'react-hot-toast'
import StudyReminder from './components/StudyReminder'

function App() {
  return (
    <section className="reminders">
      <h2 className="reminders__title">Нагадування про навчання</h2>
      <StudyReminder />
      <Toaster position="top-right" />
    </section>
  )
}

export default App
