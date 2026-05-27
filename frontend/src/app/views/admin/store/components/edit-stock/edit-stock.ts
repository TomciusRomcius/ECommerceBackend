import { Component, inject, input, OnInit } from '@angular/core';
import { ModalTemplate } from "@components/modal-template/modal-template";
import { FormControl, FormGroup, ReactiveFormsModule, Validators, ɵInternalFormsSharedModule } from "@angular/forms";
import { MatFormField, MatLabel } from "@angular/material/form-field";
import { MatInput } from "@angular/material/input";
import { MatAnchor } from "@angular/material/button";
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface EditStockDialogData {
  initialStock: number;
}

@Component({
  selector: 'app-edit-stock',
  imports: [ModalTemplate, ɵInternalFormsSharedModule, MatFormField, MatLabel, MatInput, ReactiveFormsModule, MatAnchor],
  templateUrl: './edit-stock.html',
  styleUrl: './edit-stock.css',
})
export class EditStock implements OnInit {
  data = inject<EditStockDialogData>(MAT_DIALOG_DATA);
  form = new FormGroup({
    stock: new FormControl(0, {
      nonNullable: true,
      validators: [Validators.required, Validators.min(0), Validators.max(10000)]
    }),
  });

  ngOnInit(): void {
    console.log(this.data.initialStock);
    this.form.patchValue({
      stock: this.data.initialStock
    });
  }

  submit() {
    if (!this.form.valid) {
      this.form.markAllAsTouched(); 
    }

    const { stock } = this.form.getRawValue();
  }
}
