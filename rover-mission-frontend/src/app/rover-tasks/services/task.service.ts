import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Task {
  roverName: string;
  startsAt: string;
  durationMinutes: number;
  taskType: number;
  status: number;
  latitude: number;
  longitude: number;
}

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private readonly baseUrl = 'https://localhost:7025/api/rovers';
  private readonly roverListUrl = 'https://localhost:7025/api/rovers';

  constructor(private http: HttpClient) {}

  //  Obtener tareas por rover y fecha
  getTasks(roverName: string, date: string): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.baseUrl}/${roverName}/tasks?date=${date}`);
  }

  //  Agregar tarea
  addTask(task: Task): Observable<Task> {
    return this.http.post<Task>(`${this.baseUrl}/${task.roverName}/tasks`, task);
  }

  //  Eliminar tarea aun nop
  deleteTask(roverName: string, taskId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${roverName}/tasks/${taskId}`);
  }

  //  Obtener lista de rovers disponibles
  getRovers(): Observable<string[]> {
    return this.http.get<string[]>(this.roverListUrl);
  }


}
