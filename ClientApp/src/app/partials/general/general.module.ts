import { MonthTextPipe } from './date-picker/month-text.pipe';
import { DatepickerComponent } from './date-picker/date-picker.component';
import { LoaderComponent } from './loader/loader.component';
import { SliderComponent } from './slider/slider.component';
import { InlineSVGModule } from 'ng-inline-svg';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SwitchComponent } from './switch/switch.component';
import { FormsModule } from '@angular/forms';
import { BarComponent } from './bar/bar.component';
import { MapComponent } from './_map/map.component';
import { TranslateModule } from '@ngx-translate/core';
import { CheckBoxComponent } from './checkbox/checkbox.component';
import { WifiSelectorComponent } from 'src/app/pages/_layout/header/wifi/wifi.component';
import { WifiCardComponent } from 'src/app/pages/_layout/header/wifi/wifi-card/wifi-card.component';
import { WifiPasswordComponent } from 'src/app/pages/_layout/wifi-password/wifi-password.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    SwitchComponent,
    SliderComponent,
    BarComponent,
    MapComponent ,
    CheckBoxComponent ,
    WifiSelectorComponent,
    WifiCardComponent,
    WifiPasswordComponent,
    LoaderComponent,
    DatepickerComponent,
    MonthTextPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    InlineSVGModule ,
    RouterModule
  ],
  providers: [],
  exports: [
    SwitchComponent,
    SliderComponent,
    BarComponent,
    MapComponent ,
    CheckBoxComponent ,
    WifiSelectorComponent ,
    WifiCardComponent,
    LoaderComponent,
    DatepickerComponent
  ]
})
export class GeneralModule { }
