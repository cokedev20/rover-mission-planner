// src/app/app.routes.ts
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./rover-tasks/rover-tasks.routes').then(m => m.RoverTasksRoutes)
  }
];
