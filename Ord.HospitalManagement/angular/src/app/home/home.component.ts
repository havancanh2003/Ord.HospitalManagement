import { AuthService, ConfigStateService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService, private config: ConfigStateService) {}
  ngOnInit(): void {
    const config = this.config.getAll();
    console.log(config.currentUser);
  }

  login() {
    this.authService.navigateToLogin();
  }
}
