import { WidgetsModule } from './../../partials/widgets/widgets.module';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { InlineSVGModule } from 'ng-inline-svg';
import { DashboardComponent } from './dashboard.component';
import { NotifierModule } from 'angular-notifier';
import { WindoorsQuickAccessComponent } from './components/windoors-quick-access/w-quick-access.component';
import { TranslateModule } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    DashboardComponent ,
    WindoorsQuickAccessComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    HttpClientModule,
    TranslateModule,
    InlineSVGModule.forRoot(),
    WidgetsModule,
    RouterModule
  ],
  providers: [],
  exports: [
    DashboardComponent
  ]
})
export class DashboardModule { }
