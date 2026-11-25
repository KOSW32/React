# Форма редагування профілю

React + TypeScript + Vite, `react-hook-form`, `react-select`.

## Запуск

```bash
npm install
npm run dev
```

Відкрити http://localhost:5173

## Що реалізовано

1. ПІБ студента (обов'язкове, мін. 5 символів)
2. Електронна пошта (валідація формату)
3. Номери телефонів: динамічний список (`useFieldArray`), можна додавати/видаляти
4. Володіння мовами: `react-select` з множинним вибором через `Controller`
