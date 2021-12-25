import { LampComponent } from './components/lamp/lamp.component';
import { GeneralModule } from './../../partials/general/general.module';
import { CurtainComponent } from './components/curtain/curtain.component';
import { LampsComponent } from './components/lamps/lamps.component';
import { CardComponent } from './components/card/card.component';
import { LookupComponent } from './_lookup/lookup.component';
import { MapComponent } from '../../partials/general/_map/map.component';
import { HomeComponent } from './home.component';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { TranslateModule } from '@ngx-translate/core';
import { InlineSVGModule } from 'ng-inline-svg';
import { FormsModule } from '@angular/forms';
import { SmartPlugComponent } from './components/smart-plug/smart-plug.component';
import { RouterModule } from '@angular/router';
import { WindoorsComponent } from './components/windoors/windoors.component';
import { DetailsComponent } from './details/details.component';
import { AllWindoorsComponent } from '../dashboard/quick-access/all-windoors/all-windoors.component';
import { ColorHueModule } from 'ngx-color/hue';
import { RgbLampComponent } from './components/rgb/rgb.component';
import { RgbPresetColorComponent} from './components/rgb/preset-color/preset-color.component'
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    HomeComponent,
    LookupComponent,
    CardComponent,
    LampComponent,
    LampsComponent,
    CurtainComponent,
    SmartPlugComponent,
    WindoorsComponent ,
    DetailsComponent,
    AllWindoorsComponent ,
    RgbLampComponent ,
    RgbPresetColorComponent ,
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    HttpClientModule,
    TranslateModule,
    InlineSVGModule.forRoot(),
    GeneralModule ,
    ColorHueModule,
  ],
  providers: [],
  exports: [
    HomeComponent ,
    LookupComponent ,
    SmartPlugComponent ,
    LampComponent,
    LampsComponent ,
    CurtainComponent ,
    WindoorsComponent ,
    RgbLampComponent
  ]
})
export class HomeModule { }
