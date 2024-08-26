import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.css'
})
export class TestErrorComponent {
  baseUrl = 'http://localhost:5001/api/';
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get400error(){ 
    this.http.get(this.baseUrl +'buggy/bad-request').subscribe({
      next : response =>console.log(response),
      error: error =>console.log(error)
    })
  }
  get401error(){ 
    this.http.get(this.baseUrl +'buggy/auth').subscribe({
      next : response =>console.log(response),
      error: error =>console.log(error)
    })
  }
  get404error(){ 
    this.http.get(this.baseUrl +'buggy/not-found').subscribe({
      next : response =>console.log(response),
      error: error =>console.log(error)
    })
  }
  get500error(){ 
    this.http.get(this.baseUrl +'buggy/server-error').subscribe({
      next : response =>console.log(response),
      error: error =>console.log(error)
    })
  }
  get400Validationerror(){ 
    this.http.post(this.baseUrl +'account/register',{}).subscribe({
      next : response =>console.log(response),
      error: error =>{
        console.log(error)
        this.validationErrors = error;
      }
    })
  }
}
