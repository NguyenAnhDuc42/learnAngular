import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  http = inject(HttpClient);
  registerMode = false;
  user : any;

  ngOnInit(): void {
    this.getUser()
  }
  registerToggle(){
    this.registerMode = !this.registerMode
  }
  cancelRegisterMode(event:boolean){
    this.registerMode = event
  }
  getUser(){
    this.http.get('http://localhost:5001/api/user').subscribe({
      next: responce => this.user = responce,
      error: error => console.log(error),
      complete: () => console.log('complete')
    })
  }
}
