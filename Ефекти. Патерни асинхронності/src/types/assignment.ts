export type AssignmentStatus = "Здано" | "Не здано";

export interface Assignment {
  id: number;
  courseName: string;
  moduleName: string;
  description: string;
  issueDate: string;    // ISO: YYYY-MM-DD
  deadline: string;     // ISO: YYYY-MM-DD
  status: AssignmentStatus;
}
