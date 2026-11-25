import { assignments } from "../data/assignments";
import type { Assignment } from "../types/assignment";

/** Імовірність помилки при виконанні завдання (5%). */
export const FAIL_RATE = 0.05;

/** "База даних" у пам'яті — копія, щоб зміни статусу зберігалися між запитами. */
const db: Assignment[] = assignments.map((a) => ({ ...a }));

const delay = (min = 400, max = 1000) =>
  new Promise<void>((resolve) => setTimeout(resolve, min + Math.random() * (max - min)));

/** SELECT * FROM assignments */
export async function fetchAssignments(): Promise<Assignment[]> {
  await delay();
  return db.map((a) => ({ ...a }));
}

/** SELECT * FROM assignments WHERE id = ? */
export async function fetchAssignmentById(id: number): Promise<Assignment> {
  await delay(800, 1500);
  const found = db.find((a) => a.id === id);
  if (!found) throw new Error(`Завдання з id=${id} не знайдено`);
  return { ...found };
}

/** UPDATE assignments SET status = 'Здано' WHERE id = ?  (в 5% випадків — помилка) */
export async function completeAssignment(id: number): Promise<void> {
  await delay(1000, 1800);
  if (Math.random() < FAIL_RATE) {
    throw new Error("Не вдалося виконати завдання: помилка сервера. Спробуйте ще раз.");
  }
  const found = db.find((a) => a.id === id);
  if (!found) throw new Error(`Завдання з id=${id} не знайдено`);
  found.status = "Здано";
}

/* ---------- Кеш промісів для Suspense + use() ---------- */
const cache = new Map<number, Promise<Assignment>>();

export function getAssignmentPromise(id: number): Promise<Assignment> {
  let promise = cache.get(id);
  if (!promise) {
    promise = fetchAssignmentById(id);
    cache.set(id, promise);
  }
  return promise;
}

export function invalidateAssignment(id: number): void {
  cache.delete(id);
}
