import { AuthGuard } from './modules/home/_services/auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './pages/_layout/layout.component';
import { _404Component } from './pages/_404/_404.component';
import { WifiPasswordComponent } from './pages/_layout/wifi-password/wifi-password.component';
import { DetailsComponent } from './modules/home/details/details.component';
import { AllWindoorsComponent } from './modules/dashboard/quick-access/all-windoors/all-windoors.component';
import { ScenarioSettingsPage } from './modules/scenario/settings-page/settings-page.component';

export const routes: Routes = [
  {
    path: '404',
    component: _404Component
  },
  {
    path: 'details',
    component: DetailsComponent
  },
  {
    path: 'windoors/all',
    component: AllWindoorsComponent
  },
  {
    path: 'settings',
    loadChildren: () => import('./modules/settings/settings.module').then((m) => m.SettingsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'smartplug',
    loadChildren: () => import('./modules/smartplug/smartplug.module').then((m) => m.SmartPlugModule),
  },
  {
    path: 'wifi/:id',
    component: WifiPasswordComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'scenario/add',
    component: ScenarioSettingsPage,
    canActivate: [AuthGuard]
  },
  {
    path: 'scenario/:id',
    component: ScenarioSettingsPage,
    canActivate: [AuthGuard]
  },
  {
    path: '',
    component: LayoutComponent
  },


  { path: '**', redirectTo: '404' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
