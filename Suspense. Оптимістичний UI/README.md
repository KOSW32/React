# Suspense. Оптимістичний UI

React 19 + TypeScript + Vite.

    npm install
    npm run dev

Що реалізовано:
- `Suspense` + `use()` для компонента `AssignmentDetails`
- `AssignmentDetailsSkeleton` — скелет на час завантаження
- Кнопка «Виконати завдання»: симуляція запиту до БД, 5% помилок (`FAIL_RATE` у `src/api/assignmentsApi.ts`),
  оптимістичний UI через `useOptimistic` (при помилці статус відкочується назад)
