import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageHospitalRoutingModule } from './manage-hospital-routing.module';
import { ManageHospitalComponent } from './manage-hospital.component';
import { SharedModule } from '../shared/shared.module';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzCardModule } from 'ng-zorro-antd/card'; // Card
import { NzGridModule } from 'ng-zorro-antd/grid'; // Grid module
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzTypographyModule } from 'ng-zorro-antd/typography';
import { CreateUpdateHospitalComponent } from './create-update-hospital/create-update-hospital.component';

@NgModule({
  declarations: [ManageHospitalComponent, CreateUpdateHospitalComponent],
  imports: [
    CommonModule,
    ManageHospitalRoutingModule,
    SharedModule,
    NzTableModule,
    NzPaginationModule,
    NzFormModule,
    ReactiveFormsModule, // Import ReactiveFormsModule
    NzButtonModule, // Import Button module
    FormsModule, // Import Form module
    NzInputModule, // Import Input module
    NzGridModule, // Import Grid module (cho layout nếu cần)
    NzTypographyModule, // Import Typography module
  ],
})
export class ManageHospitalModule {}
