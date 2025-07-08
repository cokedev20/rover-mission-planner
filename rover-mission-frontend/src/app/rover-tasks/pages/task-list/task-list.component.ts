import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskFormComponent } from '../../pages/tasks-form/task-form.component';
import { TaskService, Task } from '../../services/task.service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule, TaskFormComponent],
  templateUrl: './task-list.component.html',
})
export class TaskListComponent {
  private taskService = inject(TaskService);

  tareas: Task[] = [];
  rovers: string[] = [];
  roverSeleccionado: string = '';

  ngOnInit() {
    this.cargarRovers();
  }

  cargarRovers() {
    this.taskService.getRovers().subscribe({
      next: (data) => {
        this.rovers = data;
        if (this.rovers.length > 0) {
          this.roverSeleccionado = this.rovers[0];
          this.cargarTareas();
        }
      },
      error: (err) => console.error('Error cargando rovers:', err),
    });
  }

  cargarTareas() {
    if (!this.roverSeleccionado) return;

    const hoy = new Date().toISOString().split('T')[0]; // yyyy-MM-dd
    this.taskService.getTasks(this.roverSeleccionado, hoy).subscribe({
      next: (data) => {
        this.tareas = data;
      },
      error: (err) => console.error('Error cargando tareas:', err),
    });
  }

  formatearFecha(fechaIso: string): string {
    const fecha = new Date(fechaIso);
    return fecha.toLocaleString('es-CL', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  }
}
