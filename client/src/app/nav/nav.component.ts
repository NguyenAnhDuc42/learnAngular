import { NgForOf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule,BsDropdownModule,RouterLink,RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService =inject(AccountService)
  private router = inject(Router)
  private toastr = inject(ToastrService)
  model : any ={};
  Login() {
   this.accountService.login(this.model).subscribe({
     next : () =>{
      this.router.navigateByUrl('/members')
     },
     error : error => this.toastr.error(error.error)
   })
  }

  Logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }
}
