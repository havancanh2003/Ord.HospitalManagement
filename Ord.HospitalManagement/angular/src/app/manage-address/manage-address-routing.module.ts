import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProvinceComponent } from './province/province.component';
import { DistrictComponent } from './district/district.component';
import { WardComponent } from './ward/ward.component';

const routes: Routes = [
  { path: '', redirectTo: 'province', pathMatch: 'full' },
  { path: 'address', redirectTo: 'province', pathMatch: 'full' },
  { path: 'province', component: ProvinceComponent },
  { path: 'district', component: DistrictComponent },
  { path: 'ward', component: WardComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManageAddressRoutingModule {}
