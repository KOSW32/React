import type { ReactNode } from 'react'

interface SectionProps {
  title: string
  children: ReactNode
}

export default function Section({ title, children }: SectionProps) {
  return (
    <section className="schedule-section">
      <h2 className="schedule-section__title">{title}</h2>
      <div className="schedule-section__lessons">{children}</div>
    </section>
  )
}
