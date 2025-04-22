import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  // see the instruction in https://v17.angular.io/api/router/CanActivateFn
  canActivate(): boolean {
    if (!this.authService.getToken()) {
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }
}
