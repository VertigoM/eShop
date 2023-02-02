import { Component, EventEmitter, Output } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
})
export class SidenavComponent {
  constructor(private _auth: AuthenticationService) {}

  @Output() close = new EventEmitter<any>();

  onToggleClose() {
    this.close.emit();
  }

  logout(): void {
    this._auth.logout();
  }
}
