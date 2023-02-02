import { Component, OnInit } from '@angular/core';
import { MatListOption } from '@angular/material/list';
import { first } from 'rxjs';
import { Category } from 'src/app/models/category';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  categoryTypes!: Category[];
  selectedOptions: any[] = [];

  constructor(
    private _auth: AuthenticationService,
    private _products: ProductsService
  ) {}

  ngOnInit(): void {
    this._products
      .getCategories()
      .pipe(first())
      .subscribe({
        next: (result: Category[]) => {
          this.categoryTypes = result;
          console.log(this.categoryTypes);
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  onGroupsChange(options: MatListOption[]) {
    this.selectedOptions = options.map((o) => o.value);
  }

  onFilter() {
    console.log(this.selectedOptions);
  }

  get isModerator() {
    return this._auth.user?.role?.toString() == 'Moderator';
  }
}
