import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent {
  private fb = inject(FormBuilder);
  private taskService = inject(TaskService);
  private http = inject(HttpClient);

  @Output() tareaCreada = new EventEmitter<void>();

  submitted = false;
  errorMessage = '';
  rovers: string[] = [];

  taskForm = this.fb.group({
    roverName: ['', Validators.required],
    startsAt: ['', Validators.required],
    durationMinutes: [60, [Validators.required, Validators.min(1), Validators.max(180)]],
    taskType: [null, Validators.required],
    status: [null, Validators.required],
    latitude: [0, [Validators.required, Validators.min(-90), Validators.max(90)]],
    longitude: [0, [Validators.required, Validators.min(-180), Validators.max(180)]],
  });

  constructor() {
    this.cargarRovers();
  }

  cargarRovers() {
    this.http.get<string[]>('https://localhost:7025/api/rovers').subscribe({
      next: (data) => {
        this.rovers = data;
      },
      error: (error) => {
        console.error('Error cargando rovers:', error);
        this.rovers = [];
      }
    });
  }

  submit() {
    this.submitted = true;
    this.errorMessage = '';

    if (this.taskForm.invalid) return;

    const formValue = this.taskForm.value;

    this.taskService.addTask({
      roverName: formValue.roverName!,
      startsAt: formValue.startsAt!,
      durationMinutes: formValue.durationMinutes!,
      taskType: Number(formValue.taskType),
      status: Number(formValue.status),
      latitude: formValue.latitude!,
      longitude: formValue.longitude!
    }).subscribe({
      next: () => {
        this.taskForm.reset();
        this.submitted = false;
        this.tareaCreada.emit();

        const modalElement = document.getElementById('modalCrearTarea');
        if (modalElement) {
          const modalInstance = (window as any).bootstrap?.Modal.getInstance(modalElement);
          modalInstance?.hide();
        }
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === 400 && err.error?.errors) {
          this.errorMessage = Object.values(err.error.errors).flat().join(', ');
        } else {
          this.errorMessage = 'Error al crear tarea: ' + err.message;
        }
      }
    });
  }
}
