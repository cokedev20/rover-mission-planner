import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms'; 

import { RoverTasksRoutes } from './rover-tasks/rover-tasks.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(RoverTasksRoutes),
    importProvidersFrom(HttpClientModule),
    importProvidersFrom(ReactiveFormsModule) 
  ]
};
