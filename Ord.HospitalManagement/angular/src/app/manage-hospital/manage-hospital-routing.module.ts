import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageHospitalComponent } from './manage-hospital.component';
import { CreateUpdateHospitalComponent } from './create-update-hospital/create-update-hospital.component';

const routes: Routes = [
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  { path: 'manage-hospital', redirectTo: 'list', pathMatch: 'full' },
  { path: 'list', component: ManageHospitalComponent },
  {
    path: 'action-hospital',
    component: CreateUpdateHospitalComponent,
    children: [
      {
        path: '',
        component: CreateUpdateHospitalComponent,
      },
      {
        path: ':slug',
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
