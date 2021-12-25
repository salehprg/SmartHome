import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgApexchartsModule } from 'ng-apexcharts';
import { InlineSVGModule } from 'ng-inline-svg';
import { HomeModule } from '../home/home.module';
import { AllSmartPlugs } from './all-smart-plugs/all-sp.component';
import { SmartPlugChartsComponent } from './smartplug-details/smartplug-charts/smartplug-chart.component';
import { SmartPlugDetailsComponent } from './smartplug-details/smartplug-details.component';
import { GeneralModule } from 'src/app/partials/general/general.module';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import {  } from 'ngx-bootstrap/locale';
import { NgPersianDatepickerModule } from 'ng-persian-datepicker';

@NgModule({
  declarations: [
    SmartPlugDetailsComponent,
    SmartPlugChartsComponent,
    AllSmartPlugs,
  ],
  imports: [
    CommonModule,
    GeneralModule,
    InlineSVGModule,
    FormsModule,
    NgApexchartsModule,
    BsDatepickerModule.forRoot(),
    NgPersianDatepickerModule,
    NgbModule,
    HomeModule,
    TranslateModule,
    RouterModule.forChild([
      {
        path: 'details/:id',
        // path : '',
        component: SmartPlugDetailsComponent,
      },
      {
        path: 'smart-plugs',
        component: AllSmartPlugs,
      },
    ]),
  ],
})
export class SmartPlugModule {}
