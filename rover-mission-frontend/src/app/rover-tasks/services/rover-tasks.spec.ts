import { TestBed } from '@angular/core/testing';

import { RoverTasks } from './rover-tasks';

describe('RoverTasks', () => {
  let service: RoverTasks;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoverTasks);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
