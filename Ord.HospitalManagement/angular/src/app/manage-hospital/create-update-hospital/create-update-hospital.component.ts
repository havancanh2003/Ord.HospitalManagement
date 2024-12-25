import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-update-hospital',
  standalone: false,
  templateUrl: './create-update-hospital.component.html',
  styleUrl: './create-update-hospital.component.scss',
})
export class CreateUpdateHospitalComponent implements OnInit {
  form: FormGroup;
  constructor(private fb: FormBuilder) {
    this.buildForm();
  }
  ngOnInit(): void {
    console.log(123);
  }
  buildForm(): void {
    this.form = this.fb.group({
      userHospital: this.fb.group({
        userName: ['', [Validators.required]],
        emailAddress: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
      }),

      createHospital: this.fb.group({
        hospitalName: ['', [Validators.required]],
        provinceCode: [''],
        districtCode: [''],
        wardCode: [''],
        hospitalDetailAddress: [''],
        hospitalDescription: [''],
        hotline: ['', [Validators.required]],
      }),
    });
  }
  onSubmit(): void {
    // Kiểm tra form hợp lệ trước khi submit
    if (this.form.valid) {
      //const tenantHospitalData: CreateTenantHospitalDto = this.form.value;
      console.log('Tenant Hospital Data:', this.form.value);
    } else {
      console.log('Form is invalid');
    }
  }
}
