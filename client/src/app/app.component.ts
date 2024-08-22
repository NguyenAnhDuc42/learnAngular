import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  
  http = inject(HttpClient);
  title = 'datingapp';
  user : any;
  ngOnInit(): void {
    this.http.get('http://localhost:5001/api/user').subscribe({
      next: responce => this.user = responce,
      error: error => console.log(error),
      complete: () => console.log('complete')
    })
  }
}
