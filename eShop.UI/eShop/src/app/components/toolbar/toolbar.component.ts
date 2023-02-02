import { Component, EventEmitter, Output } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent {
  constructor(private _auth: AuthenticationService) {}

  get isAuthenticated(): boolean {
    return this._auth.user != null;
  }

  @Output() toggle = new EventEmitter<any>();
  openSidenav() {
    this.toggle.emit();
  }
}
