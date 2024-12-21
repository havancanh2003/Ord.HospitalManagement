import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProvinceComponent } from './province/province.component';

const routes: Routes = [
  { path: '', redirectTo: 'province', pathMatch: 'full' },
  { path: 'address', redirectTo: 'province', pathMatch: 'full' },
  { path: 'province', component: ProvinceComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManageAddressRoutingModule {}
