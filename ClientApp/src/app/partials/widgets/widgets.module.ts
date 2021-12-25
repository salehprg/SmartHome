import { GeneralModule } from 'src/app/partials/general/general.module';
import { RouterModule } from '@angular/router';
import { EnergyWidget } from './energy-widget/energy-widget.component';
import { HttpClientModule } from '@angular/common/http';
import { WeatherWidget } from './weather-widget/weather-widget.component';
import { InlineSVGModule } from 'ng-inline-svg';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LottieModule } from 'ngx-lottie';
import { TranslateModule } from '@ngx-translate/core';

export function playerFactory() {
  return import(/* webpackChunkName: 'lottie-web' */ 'lottie-web/build/player/lottie_svg');
}

@NgModule({
  declarations: [
    WeatherWidget,
    EnergyWidget
  ],
  imports: [
    CommonModule,
    TranslateModule,
    HttpClientModule,
    InlineSVGModule,
    RouterModule,
    GeneralModule,
    LottieModule.forRoot({ player: playerFactory })
  ],
  providers: [],
  exports: [
    WeatherWidget,
    EnergyWidget
  ]
})
export class WidgetsModule { }
