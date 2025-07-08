// src/app/rover-tasks/rover-tasks.routes.ts
import { Routes } from '@angular/router';

export const RoverTasksRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/task-list/task-list.component').then(m => m.TaskListComponent)
  },
  {
    path: 'crear',
    loadComponent: () =>
      import('./pages/tasks-form/task-form.component').then(m => m.TaskFormComponent)
  }
];
