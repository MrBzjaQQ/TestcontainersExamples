import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {switchMap} from 'rxjs';

interface CreateEmployeeRequest {
  phone: string;
  userName: string;
  email: string;
  position: string;
}

interface CreateEmployeeResponse {
  id: string;
}

interface GetEmployeeDto {
  id: string;
  phone: string;
  userName: string;
  email: string;
  position: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public isSubmitted = false;
  public getEmployeeDto?: GetEmployeeDto;
  public formGroup: FormGroup = new FormGroup({
    phone: new FormControl("", Validators.required),
    userName: new FormControl("", Validators.required),
    email: new FormControl("", Validators.required),
    position: new FormControl("", Validators.required),
  });

  constructor(private http: HttpClient) {}

  ngOnInit() {
  }

  public createEmployee() {
    this.formGroup.updateValueAndValidity();
    if (this.formGroup.invalid)
      return;

    const formValue : CreateEmployeeRequest = this.formGroup.value;
    this.isSubmitted = true;
    this.http.post<CreateEmployeeResponse>('/api/employees', formValue)
      .pipe(
        switchMap(response => this.http.get<GetEmployeeDto>(`/api/employees/${response.id}`))
      ).subscribe(employee => {
        this.getEmployeeDto = employee;
    });
  }

  title = 'seleniumexample.portal.client';
  protected readonly JSON = JSON;
}
