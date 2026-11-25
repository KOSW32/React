export type AssignmentStatus = "Здано" | "Не здано";

export interface Assignment {
  id: number;
  courseName: string;
  moduleName: string;
  description: string;
  issueDate: string; // YYYY-MM-DD
  deadline: string;  // YYYY-MM-DD
  status: AssignmentStatus;
}
