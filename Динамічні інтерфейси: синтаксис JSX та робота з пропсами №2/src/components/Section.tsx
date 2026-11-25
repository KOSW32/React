import type { ReactNode } from 'react'

interface SectionProps {
  title: string
  children: ReactNode
}

export default function Section({ title, children }: SectionProps) {
  return (
    <section className="homework-section" aria-labelledby="homework-title">
      <div className="homework-section__heading">
        <div>
          <p className="homework-section__eyebrow">Твій прогрес</p>
          <h2 className="homework-section__title" id="homework-title">
            {title}
          </h2>
        </div>
        <span className="homework-section__count">3 завдання</span>
      </div>
      <div className="homework-section__list">{children}</div>
    </section>
  )
}
