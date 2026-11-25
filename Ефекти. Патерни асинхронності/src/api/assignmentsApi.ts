import { assignments } from "../data/assignments";
import type { Assignment } from "../types/assignment";

const randomDelay = () => 400 + Math.random() * 600;

/** Симуляція запиту до БД: SELECT * FROM assignments */
export function fetchAssignments(): Promise<Assignment[]> {
  return new Promise((resolve) => {
    setTimeout(() => resolve([...assignments]), randomDelay());
  });
}

/** Симуляція запиту до БД: SELECT * FROM assignments WHERE id = ? */
export function fetchAssignmentById(id: number): Promise<Assignment> {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const found = assignments.find((a) => a.id === id);
      if (found) resolve({ ...found });
      else reject(new Error(`Завдання з id=${id} не знайдено`));
    }, randomDelay());
  });
}
