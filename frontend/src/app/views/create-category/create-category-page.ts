import { Component, inject, OnInit, signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { PagePadder } from '../../components/page-padder/page-padder';
import { ACCESS_TOKEN_KEY } from '../../constants/auth';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-create-category-page',
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    PagePadder,
    RouterLink,
  ],
  templateUrl: './create-category-page.html',
})
export class CreateCategoryPage implements OnInit {
  private readonly router = inject(Router);
  private readonly categoryService = inject(CategoryService);

  loading = signal(false);
  error = signal<string | null>(null);
  successMessage = signal<string | null>(null);

  form = new FormGroup({
    name: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(20),
      ],
    }),
  });

  ngOnInit(): void {
    if (!sessionStorage.getItem(ACCESS_TOKEN_KEY)) {
      void this.router.navigate(['/login']);
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.successMessage.set(null);

    this.categoryService.createCategory({ name: this.form.controls.name.value }).subscribe({
      next: (response) => {
        this.loading.set(false);
        this.successMessage.set(`Category created (id: ${response.categoryId}).`);
        this.form.reset();
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Failed to create category. You need admin access.');
      },
    });
  }
}
