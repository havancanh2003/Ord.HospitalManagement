import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageHospitalComponent } from './manage-hospital.component';
import { CreateUpdateHospitalComponent } from './create-update-hospital/create-update-hospital.component';
import { ManagePatientComponent } from './manage-patient/manage-patient.component';
import { RoleGuard } from '../role.guard';

const routes: Routes = [
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  { path: 'manage-hospital', redirectTo: 'list', pathMatch: 'full' },
  { path: 'list', component: ManageHospitalComponent },
  { path: 'manage-patient', component: ManagePatientComponent, canActivate: [RoleGuard] },
  {
    path: 'action-hospital',
    children: [
      {
        path: ':id',
        component: CreateUpdateHospitalComponent,
      },
      {
        path: '',
        component: CreateUpdateHospitalComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManageHospitalRoutingModule {}
