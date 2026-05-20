import { Component, inject, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ProductAdminService } from '../../services/product-admin.service';

@Component({
  selector: 'app-create-product-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './create-product-page.html',
})
export class CreateProductPage {
  private readonly productAdminService = inject(ProductAdminService);

  loading = signal(false);
  error = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  form = new FormGroup({
    name: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(5)],
    }),
    description: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(5)],
    }),
    price: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(0.1)],
    }),
    manufacturerId: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)],
    }),
    categoryId: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(1)],
    }),
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.successMessage.set(null);

    const { name, description, price, manufacturerId, categoryId } = this.form.getRawValue();

    this.productAdminService
      .createProduct({ name, description, price, manufacturerId, categoryId })
      .subscribe({
        next: (response) => {
          this.loading.set(false);
          this.successMessage.set(`Product created (id: ${response.productId}).`);
          this.form.reset({
            name: '',
            description: '',
            price: 0,
            manufacturerId: 0,
            categoryId: 0,
          });
        },
        error: () => {
          this.loading.set(false);
          this.error.set('Failed to create product. You may need admin access.');
        },
      });
  }
}
